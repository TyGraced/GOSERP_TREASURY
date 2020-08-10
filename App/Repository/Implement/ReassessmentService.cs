using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using PPE.AuthHandler;
using PPE.Contracts.Response;
using PPE.Data;
using PPE.DomainObjects.Approval;
using PPE.DomainObjects.PPE;
using PPE.Repository.Interface;
using Puchase_and_payables.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Implement
{
    public class ReassessmentService : IReassessmentService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        public ReassessmentService(
            DataContext dataContext,
            IIdentityServerRequest serverRequest,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _serverRequest = serverRequest;
            _identityService = identityService;
        }
        

        public async Task<ReassessmentRegRespObj> AddUpdateReassessmentAsync(ppe_reassessment model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();

                if (model.ReassessmentId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(model.ReassessmentId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                {
                    await _dataContext.ppe_reassessment.AddAsync(model);
                }
                await _dataContext.SaveChangesAsync();

                GoForApprovalRequest wfRequest = new GoForApprovalRequest
                {
                    Comment = "PPE Reassessment",
                    OperationId = (int)OperationsEnum.PPEReassessment,
                    TargetId = model.ReassessmentId,
                    ApprovalStatus = (int)ApprovalStatus.Pending,
                    DeferredExecution = true,
                    StaffId = user.StaffId,
                    CompanyId = 1,
                    EmailNotification = true,
                    ExternalInitialization = false,
                    StatusId = (int)ApprovalStatus.Processing,
                };
                var result = await _serverRequest.GotForApprovalAsync(wfRequest);

                using (var _trans = _dataContext.Database.BeginTransaction())
                {
                    if (!result.IsSuccessStatusCode)
                    {
                        new ReassessmentRegRespObj
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
                        if (model.ReassessmentId > 0)
                        {
                            var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(model.ReassessmentId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                        }
                        else
                        {
                            await _dataContext.ppe_reassessment.AddAsync(model);
                        }
                        await _dataContext.SaveChangesAsync();

                        await _trans.CommitAsync();
                        return new ReassessmentRegRespObj
                        {
                            ReassessmentId = res.TargetId,
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
                        return new ReassessmentRegRespObj
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
                        return new ReassessmentRegRespObj
                        {
                            ReassessmentId = model.ReassessmentId,
                            Status = res.Status
                        };
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new ReassessmentRegRespObj
            {
                ReassessmentId = model.ReassessmentId,
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
            };
        }

        //public async Task<bool> DeleteReassessmentAsync(int id)
        //{
        //    var itemToDelete = await _dataContext.ppe_reassessment.FindAsync(id);
        //    itemToDelete.Deleted = true;
        //    _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
        //    return await _dataContext.SaveChangesAsync() > 0;
        //}

        public async Task<ppe_reassessment> GetReassessmentByIdAsync(int id)
        {
            return await _dataContext.ppe_reassessment.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_reassessment>> GetAllReassessmentAsync()
        {
            return await _dataContext.ppe_reassessment.Where(d => d.Deleted == false).ToListAsync();
        }

        //public async Task<bool> UploadReassessmentAsync(byte[] record, string createdBy)
        //{
        //    try
        //    {
        //        if (record == null) return false;
        //        List<ppe_reassessment> uploadedRecord = new List<ppe_reassessment>();
        //        using (MemoryStream stream = new MemoryStream(record))
        //        using (ExcelPackage excelPackage = new ExcelPackage(stream))
        //        {
        //            //Use first sheet by default
        //            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
        //            int totalRows = workSheet.Dimension.Rows;
        //            //First row is considered as the header
        //            for (int i = 2; i <= totalRows; i++)
        //            {
        //                uploadedRecord.Add(new ppe_reassessment
        //                {
        //                    AssetNumber = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
        //                    LpoNumber = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
        //                    AssetClassificationId = workSheet.Cells[i, 3].Value != null ? int.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
        //                    Description = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,
        //                    Cost = workSheet.Cells[i, 5].Value != null ? decimal.Parse(workSheet.Cells[i, 5].Value.ToString()) : 0,
        //                    DateOfPurchaase = workSheet.Cells[i, 6].Value != null ? DateTime.Parse(workSheet.Cells[i, 6].Value.ToString()) : DateTime.Now,
        //                    Quantity = workSheet.Cells[i, 7].Value != null ? int.Parse(workSheet.Cells[i, 7].Value.ToString()) : 0,
        //                    DepreciationStartDate = workSheet.Cells[i, 8].Value != null ? DateTime.Parse(workSheet.Cells[i, 8].Value.ToString()) : DateTime.Now,
        //                    UsefulLife = workSheet.Cells[i, 9].Value != null ? int.Parse(workSheet.Cells[i, 9].Value.ToString()) : 0,
        //                    ResidualValue = workSheet.Cells[i, 10].Value != null ? decimal.Parse(workSheet.Cells[i, 10].Value.ToString()) : 0,
        //                    Location = workSheet.Cells[i, 11].Value != null ? workSheet.Cells[i, 11].Value.ToString() : null,
        //                    DepreciationForThePeriod = workSheet.Cells[i, 12].Value != null ? decimal.Parse(workSheet.Cells[i, 12].Value.ToString()) : 0,
        //                    AccumulatedDepreciation = workSheet.Cells[i, 13].Value != null ? decimal.Parse(workSheet.Cells[i, 13].Value.ToString()) : 0,
        //                    NetBookValue = workSheet.Cells[i, 14].Value != null ? decimal.Parse(workSheet.Cells[i, 14].Value.ToString()) : 0,
        //                    RemainingUsefulLife = workSheet.Cells[i, 15].Value != null ? int.Parse(workSheet.Cells[i, 15].Value.ToString()) : 0,
        //                    ProposedUsefulLife = workSheet.Cells[i, 16].Value != null ? int.Parse(workSheet.Cells[i, 16].Value.ToString()) : 0,
        //                });
        //            }
        //        }
        //        if (uploadedRecord.Count > 0)
        //        {
        //            foreach (var item in uploadedRecord)
        //            {
        //                var category = _dataContext.ppe_reassessment.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
        //                if (category != null)
        //                {
        //                    category.AssetNumber = item.AssetNumber;
        //                    category.LpoNumber = item.LpoNumber;
        //                    category.AssetClassificationId = item.AssetClassificationId;
        //                    category.Description = item.Description;
        //                    category.Cost = item.Cost;
        //                    category.DateOfPurchaase = item.DateOfPurchaase;
        //                    category.Quantity = item.Quantity;
        //                    category.DepreciationStartDate = item.DepreciationStartDate;
        //                    category.UsefulLife = item.UsefulLife;
        //                    category.ResidualValue = item.ResidualValue;
        //                    category.Location = item.Location;
        //                    category.DepreciationForThePeriod = item.DepreciationForThePeriod;
        //                    category.AccumulatedDepreciation = item.AccumulatedDepreciation;
        //                    category.NetBookValue = item.NetBookValue;
        //                    category.RemainingUsefulLife = item.RemainingUsefulLife;
        //                    category.ProposedUsefulLife = item.ProposedUsefulLife;
        //                    category.Active = true;
        //                    category.Deleted = false;
        //                    category.UpdatedBy = createdBy;
        //                    category.UpdatedOn = DateTime.Now;
        //                }
        //                else
        //                {
        //                    var structure = new ppe_reassessment
        //                    {
        //                        AssetNumber = item.AssetNumber,
        //                        LpoNumber = item.LpoNumber,
        //                        AssetClassificationId = item.AssetClassificationId,
        //                        Description = item.Description,
        //                        Cost = item.Cost,
        //                        DateOfPurchaase = item.DateOfPurchaase,
        //                        Quantity = item.Quantity,
        //                        DepreciationStartDate = item.DepreciationStartDate,
        //                        UsefulLife = item.UsefulLife,
        //                        ResidualValue = item.ResidualValue,
        //                        Location = item.Location,
        //                        DepreciationForThePeriod = item.DepreciationForThePeriod,
        //                        AccumulatedDepreciation = item.AccumulatedDepreciation,
        //                        NetBookValue = item.NetBookValue,
        //                        RemainingUsefulLife = item.RemainingUsefulLife,
        //                        ProposedUsefulLife = item.ProposedUsefulLife,
        //                        Active = true,
        //                        Deleted = false,
        //                        CreatedBy = createdBy,
        //                        CreatedOn = DateTime.Now,
        //                    };
        //                    await _dataContext.ppe_reassessment.AddAsync(structure);
        //                }
        //            }
        //        }

        //        var response = _dataContext.SaveChanges() > 0;
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public byte[] GenerateExportReassessment()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Asset Number");
        //    dt.Columns.Add("LPO Number");
        //    dt.Columns.Add("Classification");
        //    dt.Columns.Add("Description");
        //    dt.Columns.Add("Cost");
        //    dt.Columns.Add("Date Of Purchase");
        //    dt.Columns.Add("Quantity");
        //    dt.Columns.Add("Depreciation Start Date");
        //    dt.Columns.Add("Useful Life");
        //    dt.Columns.Add("Residual Value");
        //    dt.Columns.Add("Location");
        //    dt.Columns.Add("Depreciation For The Period");
        //    dt.Columns.Add("Accumulated Depreciation");
        //    dt.Columns.Add("Net Book Value");
        //    dt.Columns.Add("Remaining Useful Life");
        //    dt.Columns.Add("Proposed Useful Life");
        //    var category = (from a in _dataContext.ppe_reassessment
        //                    where a.Deleted == false
        //                    select new ppe_reassessment
        //                    {
        //                        ReassessmentId = a.ReassessmentId,
        //                        AssetNumber = a.AssetNumber,
        //                        LpoNumber = a.LpoNumber,
        //                        AssetClassificationId = a.AssetClassificationId,
        //                        Description = a.Description,
        //                        Cost = a.Cost,
        //                        DateOfPurchaase = a.DateOfPurchaase,
        //                        Quantity = a.Quantity,
        //                        DepreciationStartDate = a.DepreciationStartDate,
        //                        UsefulLife = a.UsefulLife,
        //                        ResidualValue = a.ResidualValue,
        //                        Location = a.Location,
        //                        DepreciationForThePeriod = a.DepreciationForThePeriod,
        //                        AccumulatedDepreciation = a.AccumulatedDepreciation,
        //                        NetBookValue = a.NetBookValue,
        //                        RemainingUsefulLife = a.RemainingUsefulLife,
        //                        ProposedUsefulLife = a.ProposedUsefulLife,
        //                    }).ToList();
        //    foreach (var kk in category)
        //    {
        //        var row = dt.NewRow();
        //        row["Asset Number"] = kk.AssetNumber;
        //        row["LPO Number"] = kk.LpoNumber;
        //        row["Classification"] = kk.AssetClassificationId;
        //        row["Description"] = kk.Description;
        //        row["Cost"] = kk.Cost;
        //        row["Date Of Purchase"] = kk.DateOfPurchaase;
        //        row["Quantity"] = kk.Quantity;
        //        row["Depreciation Start Date"] = kk.DepreciationStartDate;
        //        row["Useful Life"] = kk.UsefulLife;
        //        row["Residual Value"] = kk.ResidualValue;
        //        row["Location"] = kk.Location;
        //        row["Depreciation For The Period"] = kk.DepreciationForThePeriod;
        //        row["Accumulated Depreciation"] = kk.AccumulatedDepreciation;
        //        row["Net Book Value"] = kk.NetBookValue;
        //        row["Remaining Useful Life"] = kk.RemainingUsefulLife;
        //        row["Proposed Useful Life"] = kk.ProposedUsefulLife;
        //        dt.Rows.Add(row);
        //    }
        //    Byte[] fileBytes = null;

        //    if (category != null)
        //    {
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //        using (ExcelPackage pck = new ExcelPackage())
        //        {
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Assessment");
        //            ws.DefaultColWidth = 20;
        //            ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
        //            fileBytes = pck.GetAsByteArray();
        //        }
        //    }
        //    return fileBytes;
        //}

        public async Task<StaffApprovalRegRespObj> ReassessmentStaffApprovals(StaffApprovalObj request)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _serverRequest.UserDataAsync();

                var currentItem = await _dataContext.ppe_reassessment.FindAsync(request.TargetId);

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
                            currentItem.WorkflowToken = response.Status.CustomToken;

                            var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(currentItem.ReassessmentId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(currentItem);
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
                            currentItem.WorkflowToken = response.Status.CustomToken;

                            var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(currentItem.ReassessmentId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(currentItem);
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

                            var disposal = new ppe_disposal
                            {
                                Active = true,
                                AssetClassificationId = currentItem.AssetClassificationId,
                                Cost = currentItem.Cost,
                                CreatedBy = user.UserName,
                                DateOfPurchaase = currentItem.DateOfPurchaase,
                                Description = currentItem.Description,
                                Location = currentItem.Location,
                                Quantity = currentItem.Quantity,
                                AssetNumber = currentItem.AssetNumber,
                                DepreciationStartDate = currentItem.DepreciationStartDate,
                                ResidualValue = currentItem.ResidualValue,
                                DepreciationForThePeriod = currentItem.DepreciationForThePeriod,
                                AccumulatedDepreciation = currentItem.AccumulatedDepreciation,
                                NetBookValue = currentItem.NetBookValue,
                                CreatedOn = currentItem.CreatedOn,
                                UpdatedBy = user.UserName,
                                UpdatedOn = currentItem.ReassessmentId > 0 ? DateTime.Today : DateTime.Today

                            };
                            
                            await AddUpdateDisposalAsync(disposal);
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

                            var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(currentItem.ReassessmentId);
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

        private async Task<bool> AddUpdateDisposalAsync(ppe_disposal model)
        {
            try
            {
                if (model.DisposalId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_disposal.FindAsync(model.DisposalId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_disposal.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ppe_reassessment>> GetReassessmentAwaitingApprovals(List<int> reassessmentIds, List<string> tokens)
        {
            var item = await _dataContext.ppe_reassessment.Where(s => reassessmentIds.Contains(s.ReassessmentId) && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }
    }
}