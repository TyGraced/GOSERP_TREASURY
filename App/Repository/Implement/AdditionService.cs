﻿using AutoMapper;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using PPE.AuthHandler;
using PPE.Contracts.Response;
using PPE.Data;
using PPE.DomainObjects.Approval;
using PPE.DomainObjects.PPE;
using PPE.Repository.Implement.Addition;
using PPE.Repository.Interface;
using PPE.Requests;
using Puchase_and_payables.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Implement
{
    public class AdditionService : IAdditionService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFinanceServerRequest _financeRequest;
        public AdditionService(DataContext dataContext,
            IIdentityService identityService,
            IIdentityServerRequest serverRequest,
            IFinanceServerRequest financeRequest,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _serverRequest = serverRequest;
            _identityService = identityService;
            _financeRequest = financeRequest;
        }
        public async Task<AdditionFormRegRespObj> AddUpdateAdditionAsync(ppe_additionform model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                //ppe_additionform itemToUpdate = new ppe_additionform();
                if (model.AdditionFormId > 0)
                {
                     var itemToUpdate = await _dataContext.ppe_additionform.FindAsync(model.AdditionFormId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                {
                    await _dataContext.ppe_additionform.AddAsync(model);
                }
                
                await _dataContext.SaveChangesAsync();
                var targetIds = new List<int>();
                targetIds.Add(model.AdditionFormId);

                GoForApprovalRequest wfRequest = new GoForApprovalRequest
                {
                    Comment = "PPE Addition",
                    OperationId = (int)OperationsEnum.PPEAdditionApproval,
                    TargetId = targetIds,
                    ApprovalStatus = (int)ApprovalStatus.Pending,
                    DeferredExecution = true,
                    StaffId = user.StaffId,
                    CompanyId = user.CompanyId,
                    EmailNotification = true,
                    ExternalInitialization = false,
                    StatusId = (int)ApprovalStatus.Processing,
                };
                var result = await _serverRequest.GotForApprovalAsync(wfRequest);

                using(var _trans = _dataContext.Database.BeginTransaction())
                {
                    if (!result.IsSuccessStatusCode)
                    {
                        new AdditionFormRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage { FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}" }
                            }
                        };
                    }

                    var stringData = await result.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                   

                    if (res.ApprovalProcessStarted)
                    {
                        model.ApprovalStatusId = (int)ApprovalStatus.Processing;
                        model.WorkflowToken = res.Status.CustomToken;
                        if (model.AdditionFormId > 0)
                        {
                            var itemToUpdate = await _dataContext.ppe_additionform.FindAsync(model.AdditionFormId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                        }
                        else
                        {
                            await _dataContext.ppe_additionform.AddAsync(model);
                        }
                        var lpo = _dataContext.ppe_lpo.FirstOrDefault(q => q.LPONumber.ToLower().Trim() == model.LpoNumber.ToLower().Trim());
                        if (lpo != null)
                        {
                            lpo.IsUsed = true;
                            _dataContext.Entry(lpo).CurrentValues.SetValues(lpo);
                        }
                        await _dataContext.SaveChangesAsync();

                        await _trans.CommitAsync();
                        return new AdditionFormRegRespObj
                        {
                            AdditionFormId  = res.TargetId,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = res.Status.IsSuccessful,
                                Message = res.Status.Message
                            }
                        };
                    }

                    if (res.EnableWorkflow || !res.HasWorkflowAccess)
                    {
                        await _trans.RollbackAsync();
                        return new AdditionFormRegRespObj
                        { 
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = res.Status.IsSuccessful,
                                Message = res.Status.Message
                            }
                        };
                    }
                    if (!res.EnableWorkflow)
                    {
                       
                        await _trans.CommitAsync();
                        return new AdditionFormRegRespObj
                        {
                            AdditionFormId = model.AdditionFormId,
                            Status = res.Status
                        };
                    }
                     
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new AdditionFormRegRespObj
            {
                AdditionFormId = model.AdditionFormId,
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
            };
        }

        public async Task<bool> DeleteAdditionAsync(int id)
        {
            var itemToDelete = await _dataContext.ppe_additionform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ppe_additionform> GetAdditionByIdAsync(int id)
        {
            return await _dataContext.ppe_additionform.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_additionform>> GetAllAdditionAsync()
        {
            return await _dataContext.ppe_additionform.Where(d => d.Deleted == false && d.ApprovalStatusId !=  (int)ApprovalStatus.Approved).ToListAsync();
        }

        public async Task<bool> UploadAdditionAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                var subGlResponse = await _financeRequest.GetAllSubGlAsync();
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<AdditionFormObj> uploadedRecord = new List<AdditionFormObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new AdditionFormObj
                                {
                                    LpoNumber = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    DateOfPurchasse = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : string.Empty,
                                    //Description = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                                    //Quantity = workSheet.Cells[i, 3].Value != null ? int.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
                                    //Cost = workSheet.Cells[i, 4].Value != null ? decimal.Parse(workSheet.Cells[i, 4].Value.ToString()) : 0,
                                    //SubGlAdditionName = workSheet.Cells[i, 5].Value != null ? workSheet.Cells[i, 5].Value.ToString() : null,
                                    //SubGlAdditionCode = workSheet.Cells[i, 7].Value != null ? workSheet.Cells[i, 7].Value.ToString() : null,
                                    //Location = workSheet.Cells[i, 6].Value != null ? workSheet.Cells[i, 6].Value.ToString() : null,
                                    //ClassificationName = workSheet.Cells[i, 7].Value != null ? workSheet.Cells[i, 7].Value.ToString() : null,
                                    //UsefulLife = workSheet.Cells[i, 8].Value != null ? int.Parse(workSheet.Cells[i, 8].Value.ToString()) : 0,
                                    //ResidualValue = workSheet.Cells[i, 9].Value != null ? decimal.Parse(workSheet.Cells[i, 9].Value.ToString()) : 0,
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                    
                }
                if (uploadedRecord.Count > 0)
                {   
                    foreach (var item in uploadedRecord)
                    {
                        var dateFormat = item.DateOfPurchasse.Split(" ")[0];
                        var datetime = item.DateOfPurchasse.Split(" ")[1];
                        var D = dateFormat.Split("/")[1];
                        var M = dateFormat.Split("/")[0];
                        var Y = dateFormat.Split("/")[2]; 
                        var date = Convert.ToDateTime( $"{D}/{M}/{Y} {datetime}");

                        //var subGlAdditionCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlAdditionCode)?.SubGLId ?? 0;
                        //var subGlAdditionName = subGlResponse.subGls.FirstOrDefault(d => d.SubGLName == item.SubGlAdditionName)?.SubGLId ?? 0;
                        //var classificationName = _dataContext.ppe_assetclassification.Where(c => c.ClassificationName == item.ClassificationName).FirstOrDefault()?.AsetClassificationId ?? 0;
                        var category = _dataContext.ppe_additionform.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.LpoNumber = item.LpoNumber;
                            category.DateOfPurchase = date;
                            //category.Description = item.Description;
                            //category.Quantity = item.Quantity;
                            //category.Cost = item.Cost;
                            //category.SubGlAddition = subGlAdditionName;
                            //category.SubGlAddition = subGlAdditionCode;
                            //category.Location = item.Location;
                            //category.AssetClassificationId = classificationName;
                            //category.UsefulLife = item.UsefulLife;
                            //category.ResidualValue = item.ResidualValue;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var addition = new ppe_additionform();

                            addition.AdditionFormId = item.AdditionFormId;
                            addition.LpoNumber = item.LpoNumber;
                            addition.DateOfPurchase = date;
                            //addition.Description = item.Description;
                            //addition.Quantity = item.Quantity;
                            //addition.Cost = item.Cost;
                            //addition.SubGlAddition = subGlAdditionName;
                            //addition.SubGlAddition = subGlAdditionCode;
                            //addition.Location = item.Location;
                            //addition.AssetClassificationId = item.AssetClassificationId;
                            //addition.UsefulLife = item.UsefulLife;
                            //addition.ResidualValue = item.ResidualValue;
                            addition.Active = true;
                            addition.Deleted = false;
                            addition.CreatedBy = createdBy;
                            addition.CreatedOn = DateTime.Now;

                            await _dataContext.ppe_additionform.AddAsync(addition);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] GenerateExportAddition()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Lpo Number");
            dt.Columns.Add("Date Of Purchase");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Cost");
            dt.Columns.Add("SubGL Addition Name");
            dt.Columns.Add("SubGL Addition Code");
            dt.Columns.Add("Location");
            dt.Columns.Add("Classification Name");
            dt.Columns.Add("Useful Life");
            dt.Columns.Add("Residual Value");
            var subGlResponse = _financeRequest.GetAllSubGlAsync().Result;
            var additions = (from a in _dataContext.ppe_additionform
                            where a.Deleted == false
                            select new AdditionFormObj
                            {
                                LpoNumber = a.LpoNumber,
                                DateOfPurchase = a.DateOfPurchase,
                                Description = a.Description,
                                Quantity = a.Quantity,
                                Cost = a.Cost,
                                SubGlAddition = a.SubGlAddition,
                                Location = a.Location,
                                AssetClassificationId = a.AssetClassificationId,
                                UsefulLife = a.UsefulLife,
                                ResidualValue = a.ResidualValue,
                            }).ToList();
             

            if (additions.Count() > 0)
            {  
                foreach (var res in additions)
                {
                    res.SubGlAdditionCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAddition)?.SubGLCode;
                    res.SubGlAdditionName = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAddition)?.SubGLName;
                    

                }
            }
            var classificationName = _dataContext.ppe_assetclassification.Where(c => c.Deleted == false).ToList();
            var formattedDate = DateTimeOffset.Now.ToString("d");


            foreach (var kk in additions)
            {
                var row = dt.NewRow();
                row["Lpo Number"] = kk.LpoNumber;
                row["Date Of Purchase"] = kk.DateOfPurchase.Date.ToString("dd/MM/yyyy");
                row["Description"] = kk.Description;
                row["Quantity"] = kk.Quantity;
                row["Cost"] = kk.Cost;
                row["SubGL Addition Name"] = kk.SubGlAdditionName;
                row["SubGL Addition Code"] = kk.SubGlAdditionCode;
                row["Location"] = kk.Location;
                row["Classification Name"] = classificationName.FirstOrDefault(a => a.AsetClassificationId == kk.AssetClassificationId)?.ClassificationName;
                row["Useful Life"] = kk.UsefulLife;
                row["Residual Value"] = kk.ResidualValue;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (additions != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Addition Form");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public async Task<StaffApprovalRegRespObj> AdditionStaffApprovals(StaffApprovalObj request)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _serverRequest.UserDataAsync();

                var currentItem = await _dataContext.ppe_additionform.FindAsync(request.TargetId);

                using (var _trans = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var details = new cor_approvaldetail
                        {
                            Comment = request.ApprovalComment,
                            Date = DateTime.Today,
                            StatusId = request.ApprovalStatus,
                            TargetId = request.TargetId,
                            StaffId = user.StaffId,
                            WorkflowToken = currentItem.WorkflowToken
                        };

                        var req = new IdentityServerApprovalCommand
                        {
                            ApprovalComment = request.ApprovalComment,
                            ApprovalStatus = request.ApprovalStatus,
                            TargetId = request.TargetId,
                            WorkflowToken = currentItem.WorkflowToken,
                        };
                        var result = await _serverRequest.StaffApprovalRequestAsync(req);
                        
                        if (!result.IsSuccessStatusCode)
                        {
                            return new StaffApprovalRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = false,
                                    Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                }
                            };
                        }

                        var stringData = await result.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<StaffApprovalRegRespObj>(stringData);

                        if (!response.Status.IsSuccessful)
                        {
                            return new StaffApprovalRegRespObj
                            {
                                Status = response.Status
                            };
                        }
                        if (response.ResponseId == (int)ApprovalStatus.Processing)
                        {
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            currentItem.ApprovalStatusId = (int)ApprovalStatus.Processing;  
                            _dataContext.Entry(currentItem).CurrentValues.SetValues(currentItem);
                            await _dataContext.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new StaffApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Processing,
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = true,
                                    Message = response.Status.Message
                                }
                            };
                        }
                        if (response.ResponseId == (int)ApprovalStatus.Revert)
                        {
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            currentItem.ApprovalStatusId = (int)ApprovalStatus.Revert;
                            _dataContext.Entry(currentItem).CurrentValues.SetValues(currentItem);
                            await _dataContext.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new StaffApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Revert,
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = true,
                                    Message =
                            response.Status.Message
                                }
                            };
                        }
                        if (response.ResponseId == (int)ApprovalStatus.Approved)
                        {
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            currentItem.ApprovalStatusId = (int)ApprovalStatus.Approved;
                            currentItem.WorkflowToken = response.Status.CustomToken;

                            
                            var assetNumber = AssetNumber.Generate();
                            
                            var depreciationStartDate = DateTime.Now;
                            
                            var depreciable = _dataContext.ppe_assetclassification.Find(currentItem.AssetClassificationId);
                            if (depreciable.Depreciable)
                            {
                                var res = GenerateInvestmentDailySchedule(currentItem.AdditionFormId);
                            }
                            var register = new ppe_register
                            {
                                Active = true,
                                AssetClassificationId = currentItem.AssetClassificationId,
                                AdditionFormId = currentItem.AdditionFormId,
                                Cost = currentItem.Cost,
                                CreatedBy = user.UserName,
                                DateOfPurchaase = currentItem.DateOfPurchase,
                                Description = currentItem.Description,
                                Location = currentItem.Location,
                                LpoNumber = currentItem.LpoNumber,
                                Quantity = currentItem.Quantity,
                                DepreciationStartDate = depreciationStartDate,
                                UsefulLife = currentItem.UsefulLife,
                                ResidualValue = currentItem.ResidualValue,
                                AssetNumber = assetNumber,
                                SubGlDepreciation = currentItem.SubGlDepreciation,
                                SubGlAccumulatedDepreciation = currentItem.SubGlAccumulatedDepreciation,
                                WorkflowToken = currentItem.WorkflowToken,
                                SubGlDisposal = currentItem.SubGlDisposal,
                                CreatedOn = currentItem.CreatedOn,
                                UpdatedBy = user.UserName,
                                UpdatedOn = currentItem.AdditionFormId > 0 ? DateTime.Today : DateTime.Today

                            };

                            await AddUpdateRegisterAsync(register);
                            await _dataContext.SaveChangesAsync();

                            await _trans.CommitAsync();

                            return new StaffApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Approved,
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = true,
                                    Message = response.Status.Message
                                }
                            };
                        }
                        if (response.ResponseId == (int)ApprovalStatus.Disapproved)
                        {
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            currentItem.ApprovalStatusId = (int)ApprovalStatus.Disapproved;
                            currentItem.WorkflowToken = response.Status.CustomToken;

                            var itemToUpdate = await _dataContext.ppe_additionform.FindAsync(currentItem.AdditionFormId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(currentItem);
                            await _trans.CommitAsync();
                            return new StaffApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Disapproved,
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = true,
                                    Message =
                            response.Status.Message
                                }
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        await _trans.RollbackAsync();
                        throw ex;
                    }
                    finally { await _trans.DisposeAsync(); }

                }

                //using (var _trans = await _dataContext.Database.BeginTransactionAsync())
                //{
                //    try
                //    {
                //        rejectedLpoBid.IsRejected = true;
                //        await _purchaseService.AddUpdateBidAndTender(rejectedLpoBid);
                //        var lostBid = await _dataContext.cor_bid_and_tender.Where(q => q.PLPOId == request.LPOId && q.BidAndTenderId != rejectedLpoBid.BidAndTenderId).ToListAsync();
                //        targetList.AddRange(lostBid.Select(q => q.BidAndTenderId));
                //        GoForApprovalRequest wfRequest = new GoForApprovalRequest
                //        {
                //            Comment = "Re-Selection of Bid",
                //            OperationId = (int)OperationsEnum.PurchasePRNApproval,
                //            TargetId = targetList,
                //            ApprovalStatus = (int)ApprovalStatus.Pending,
                //            DeferredExecution = true,
                //            StaffId = 0,
                //            CompanyId = 0,
                //            EmailNotification = true,
                //            ExternalInitialization = false,
                //            StatusId = (int)ApprovalStatus.Processing,
                //        };
                //        var result = await _serverRequest.GotForApprovalAsync(wfRequest);
                //        if (!result.IsSuccessStatusCode)
                //        {
                //            response.Status.Message = new APIResponseMessage { FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}" };
                //            return response;
                //        }
                //        var stringData = await result.Content.ReadAsStringAsync();
                //        var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                //        if (res.ApprovalProcessStarted)
                //        {
                //            foreach (var id in targetList)
                //            {
                //                var item = await _dataContext.cor_bid_and_tender.FindAsync(id);
                //                if (item != null)
                //                {
                //                    item.ApprovalStatusId = (int)ApprovalStatus.Processing;
                //                    item.WorkflowToken = res.Status.CustomToken;
                //                }
                //                await _purchaseService.AddUpdateBidAndTender(item);
                //            }

                //            await _trans.CommitAsync();
                //            response.BidAndTenderId = bidAndTenderObj.BidAndTenderId;
                //            response.Status.IsSuccessful = res.Status.IsSuccessful;
                //            response.Status.Message = res.Status.Message;
                //            return response;
                //        }
                //        if (res.EnableWorkflow || !res.HasWorkflowAccess)
                //        {
                //            await _trans.RollbackAsync();
                //            return new BidAndTenderRegRespObj
                //            {
                //                BidAndTenderId = bidAndTenderObj.BidAndTenderId,
                //                Status = new APIResponseStatus
                //                {
                //                    IsSuccessful = res.Status.IsSuccessful,
                //                    Message = res.Status.Message
                //                }
                //            };
                //        }
                //        if (!res.EnableWorkflow)
                //        {
                //            await _trans.CommitAsync();
                //            return new BidAndTenderRegRespObj
                //            {
                //                BidAndTenderId = bidAndTenderObj.BidAndTenderId,
                //                Status = new APIResponseStatus
                //                {
                //                    IsSuccessful = true,
                //                    Message = new APIResponseMessage
                //                    {
                //                        FriendlyMessage = "Successful"
                //                    }
                //                }
                //            };
                //        }
                //    }
                //    catch (SqlException ex)
                //    {
                //        await _trans.RollbackAsync();
                //        var errorCode = ErrorID.Generate(4);
                //        _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                //        throw ex;
                //    }
                //    finally { await _trans.DisposeAsync(); }
                //}

                return new StaffApprovalRegRespObj
                {
                    ResponseId = request.TargetId,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> AddUpdateRegisterAsync(ppe_register model)
        {
            try
            {
                if (model.RegisterId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_register.FindAsync(model.RegisterId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_register.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<IEnumerable<ppe_additionform>> GetAdditionAwaitingApprovals(List<int> additonIds, List<string> tokens)
        {
            var item = await _dataContext.ppe_additionform.Where(s => additonIds.Contains(s.AdditionFormId) && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public IEnumerable<ApprovalDetailsObj> GetApprovalTrail(int targetId, string workflowToken)
        {
            var staffResponse = _serverRequest.GetAllStaffAsync().Result;
            var trail = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(workflowToken) && x.TargetId == targetId);

            var data = trail.Select(x => new ApprovalDetailsObj
            {
                ApprovalDetailId = x.ApprovalDetailId,
                Comment = x.Comment,
                TargetId = x.TargetId,
                StaffId = x.StaffId,
                WorkflowToken = x.WorkflowToken,
                Date = x.Date,
                //ArrivalDate = x.ArrivalDate,
                //ActualArrivalDate = x.ActualArrivalDate,
                //ResponseStaffId = x.ResponseStaffId,
                //RequestStaffId = x.RequestStaffId,
                //FromApprovalLevelId = x.FromWorkflowLevelId,
                //FromApprovalLevelName = x.FromWorkflowLevelId == null ? "N/A" : x.cor_workflowlevel.WorkflowLevelName,
                //ToApprovalLevelName = x.ToWorkflowLevelId == null ? "N/A" : x.cor_workflowlevel1.WorkflowLevelName,
                //ToApprovalLevelId = (int)x.ToWorkflowLevelId,
                //ApprovalStatusId = x.StatusId,
                //ApprovalStatus = x.cor_approvalstatus.ApprovalStatusName,
                //ToStaffName = x.cor_staff1.FirstName + " " + x.cor_staff1.LastName,
                //FromStaffName = x.cor_staff.FirstName + " " + x.cor_staff.LastName,
            }).OrderByDescending(x => x.ApprovalDetailId).ToList();
            foreach (var item in data)
            {
                item.FirstName = staffResponse.staff.FirstOrDefault(d => d.staffId == item.StaffId).firstName;
                item.LastName = staffResponse.staff.FirstOrDefault(d => d.staffId == item.StaffId).lastName;
            }
            return data;
        }

        private bool GenerateInvestmentDailySchedule(int AdditionId)
        {

            var currentItem =  _dataContext.ppe_additionform.Find(AdditionId);

                decimal monthlyDepreciation = ((currentItem.Cost - currentItem.ResidualValue) / currentItem.UsefulLife);
                decimal dailyDepreciationCharge = (monthlyDepreciation / 30);
                var day = DateTime.UtcNow.Date;
                var noOfDaysInThePeriod = day.ToString("D").Split()[0];
                
                //decimal depreciationForThePeriod = (dailyDepreciationCharge * Convert.ToInt32(noOfDaysInThePeriod));
                TimeSpan usedLifeOfAsset = (DateTime.Today - currentItem.DepreciationStartDate);
                int differenceInDays = usedLifeOfAsset.Days;
                decimal accumulatedDepreciation = (dailyDepreciationCharge * (differenceInDays));
                decimal netBookValue = currentItem.Cost - accumulatedDepreciation;
                var assetNumber = AssetNumber.Generate();

                var depreciationStartDate = DateTime.Now;
                var freq = 30;
                int dailyPeriod = currentItem.UsefulLife * 30;
                decimal dailyCB = currentItem.Cost;
                int i = 1;
                int count = 0;
                decimal accdailyDepreciationCharge = 0;
                decimal accdailyAccumilative = 0;

                for (int k = 0; k <= dailyPeriod; k++)
                {
                    ppe_dailyschedule dailyschedule = new ppe_dailyschedule();
                    if (count == freq)
                    {
                        i++;
                        count = 0;
                        dailyschedule.EndPeriod = true;
                    }
                    if (k == 0)
                    {
                        dailyschedule.Period = k;
                        dailyschedule.PeriodId = i;
                        dailyschedule.OB = dailyCB;
                        dailyschedule.DailyDepreciation = 0;
                        dailyschedule.AccumulatedDepreciation = 0;
                        dailyschedule.CB = dailyCB;
                        dailyschedule.PeriodDate = depreciationStartDate.AddDays(k);
                        dailyschedule.AdditionId = currentItem.AdditionFormId;
                        dailyschedule.EndPeriod = true;
                        _dataContext.ppe_dailyschedule.Add(dailyschedule);
                        _dataContext.SaveChanges();
                    }
                    else if (k == 1 || k <= dailyPeriod)
                    {
                        dailyschedule.Period = k;
                        dailyschedule.PeriodId = i;
                        dailyschedule.OB = dailyCB;
                        dailyschedule.DailyDepreciation = (dailyDepreciationCharge);
                        dailyschedule.AccumulatedDepreciation = dailyDepreciationCharge + accdailyDepreciationCharge;
                        dailyschedule.DepreciationForThePeriod = dailyDepreciationCharge + accdailyAccumilative;
                        dailyschedule.CB = (dailyCB - dailyDepreciationCharge);
                        dailyschedule.PeriodDate = depreciationStartDate.AddDays(k);
                        dailyCB = (dailyCB - dailyDepreciationCharge);
                        accdailyDepreciationCharge = dailyDepreciationCharge + accdailyDepreciationCharge;
                        accdailyAccumilative = dailyDepreciationCharge + accdailyAccumilative;
                        dailyschedule.AdditionId = currentItem.AdditionFormId;
                        if (k == freq)
                        {
                            //dailyschedule.DepreciationForThePeriod = 0;
                            accdailyAccumilative = 0;
                        }
                        _dataContext.ppe_dailyschedule.Add(dailyschedule);
                        _dataContext.SaveChanges();
                    }

                    count++;
                }
            
            return true;
        }

        public async Task<bool> AddUpdateLpoNumber(ppe_lpo model)
        {
            try
            {

                if (model.PLPOId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_lpo.FindAsync(model.PLPOId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_lpo.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<LpoObj> GetAllLpo()
        {
            try
            {

                var Lpo = (from a in _dataContext.ppe_lpo
                                   where a.IsUsed == false 
                                   select new LpoObj
                                   {
                                       PLPOId = a.PLPOId,
                                       SupplierAddress = a.SupplierAddress,
                                       SupplierIds = a.SupplierIds,
                                       SupplierNumber = a.SupplierNumber,
                                       WinnerSupplierId = a.WinnerSupplierId,
                                       Tax = a.Tax,
                                       Taxes = a.Taxes,
                                       Total = a.Total,
                                       DeliveryDate = a.DeliveryDate,
                                       ApprovalStatusId = a.ApprovalStatusId,
                                       GrossAmount = a.GrossAmount,
                                       JobStatus = a.JobStatus,
                                       BidAndTenderId = a.BidAndTenderId,
                                       BidComplete = a.BidComplete,
                                       WorkflowToken = a.WorkflowToken,
                                       Location = a.WorkflowToken,
                                       PurchaseReqNoteId = a.PurchaseReqNoteId,
                                       DebitGl = a.DebitGl,
                                       ServiceTerm = a.ServiceTerm,
                                       LPONumber = a.LPONumber,
                                       IsUsed = a.IsUsed,
                                       Name = a.Name,
                                       Description = a.Description,
                                       Quantity = a.Quantity,
                                       AmountPayable = a.AmountPayable,
                                       Address = a.Address,
                                       RequestDate = a.RequestDate
                                   }).ToList();

                return Lpo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
