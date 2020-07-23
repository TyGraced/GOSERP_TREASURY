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
using Puchase_and_payables.Requests;
using System;
using System.Collections.Generic;
using System.Data;
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
        public AdditionService(DataContext dataContext,
            IIdentityService identityService,
            IIdentityServerRequest serverRequest,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _serverRequest = serverRequest;
            _identityService = identityService;
        }
        public async Task<AdditionFormRegRespObj> AddUpdateAdditionAsync(ppe_additionform model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();

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

                GoForApprovalRequest wfRequest = new GoForApprovalRequest
                {
                    Comment = "PPE Addition",
                    OperationId = (int)OperationsEnum.PPEAdditionApproval,
                    TargetId = model.AdditionFormId,
                    ApprovalStatus = (int)ApprovalStatus.Pending,
                    DeferredExecution = true,
                    StaffId = user.StaffId,
                    CompanyId = 1,
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
            return await _dataContext.ppe_additionform.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadAdditionAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<ppe_additionform> uploadedRecord = new List<ppe_additionform>();
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
                                var item = new ppe_additionform
                                {
                                    LpoNumber = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    DateOfPurchase = workSheet.Cells[i, 2].Value != null ? DateTime.Parse(workSheet.Cells[i, 2].Value.ToString()) : DateTime.Now,
                                    Description = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                                    Quantity = workSheet.Cells[i, 4].Value != null ? int.Parse(workSheet.Cells[i, 4].Value.ToString()) : 0,
                                    Cost = workSheet.Cells[i, 5].Value != null ? decimal.Parse(workSheet.Cells[i, 5].Value.ToString()) : 0,
                                    SubGlAddition = workSheet.Cells[i, 6].Value != null ? int.Parse(workSheet.Cells[i, 6].Value.ToString()) : 0,
                                    Location = workSheet.Cells[i, 7].Value != null ? workSheet.Cells[i, 7].Value.ToString() : null,
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
                        var category = _dataContext.ppe_additionform.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.LpoNumber = item.LpoNumber;
                            category.DateOfPurchase = item.DateOfPurchase;
                            category.Description = item.Description;
                            category.Quantity = item.Quantity;
                            category.Cost = item.Cost;
                            category.SubGlAddition = item.SubGlAddition;
                            category.Location = item.Location;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var addition = new ppe_additionform
                            {
                                LpoNumber = item.LpoNumber,
                                DateOfPurchase = item.DateOfPurchase,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                SubGlAddition = item.SubGlAddition,
                                Location = item.Location,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
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
            dt.Columns.Add("SubGl Addition");
            dt.Columns.Add("Location");
            var category = (from a in _dataContext.ppe_additionform
                            where a.Deleted == false
                            select new ppe_additionform
                            {
                                LpoNumber = a.LpoNumber,
                                DateOfPurchase = a.DateOfPurchase,
                                Description = a.Description,
                                Quantity = a.Quantity,
                                Cost = a.Cost,
                                SubGlAddition = a.SubGlAddition,
                                Location = a.Location,
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Lpo Number"] = kk.LpoNumber;
                row["Date Of Purchase"] = kk.DateOfPurchase;
                row["Description"] = kk.Description;
                row["Quantity"] = kk.Quantity;
                row["Cost"] = kk.Cost;
                row["SubGl Addition"] = kk.SubGlAddition;
                row["Location"] = kk.Location;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
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
                            await  _dataContext.cor_approvaldetail.AddAsync(details);
                            currentItem.ApprovalStatusId = (int)ApprovalStatus.Processing;
                            currentItem.WorkflowToken = response.Status.CustomToken;

                            var itemToUpdate = await _dataContext.ppe_additionform.FindAsync(currentItem.AdditionFormId);
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

                            var itemToUpdate = await _dataContext.ppe_additionform.FindAsync(currentItem.AdditionFormId);
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

                            decimal monthlyDepreciation = ((currentItem.Cost - currentItem.ResidualValue) / currentItem.UsefulLife);
                            decimal dailyDepreciationCharge = (monthlyDepreciation / 30);
                            var assetNumber = AssetNumber.Generate();

                            var depreciationStartDate = currentItem.DepreciationStartDate;
                            int dailyPeriod = currentItem.UsefulLife * 30;
                            decimal dailyCB = currentItem.Cost;
                            decimal accdailyDepreciationCharge = 0;
                            decimal accdailyAccumilative = 0;

                            var res = GenerateInvestmentDailySchedule(currentItem.AdditionFormId);

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
                                DepreciationStartDate = currentItem.DepreciationStartDate,
                                DepreciationForThePeriod = dailyDepreciationCharge + accdailyAccumilative,
                                NetBookValue = (dailyCB - dailyDepreciationCharge),
                                AccumulatedDepreciation = dailyDepreciationCharge + accdailyDepreciationCharge,
                                UsefulLife = currentItem.UsefulLife,
                                ResidualValue = currentItem.ResidualValue,
                                AssetNumber = assetNumber,

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

            var depreciationStartDate = currentItem.DepreciationStartDate;
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

    }
}
