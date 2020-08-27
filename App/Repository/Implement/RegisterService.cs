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
using PPE.Requests;
using Puchase_and_payables.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Implement
{
    public class RegisterService : IRegisterService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFinanceServerRequest _financeRequest;
        private readonly IIdentityService _identityService;

        public RegisterService(DataContext dataContext, IIdentityServerRequest serverRequest, IHttpContextAccessor httpContextAccessor, IFinanceServerRequest financeRequest, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _serverRequest = serverRequest;
            _httpContextAccessor = httpContextAccessor;
            _financeRequest = financeRequest;
            _identityService = identityService;
        }
        public async Task<bool> AddUpdateRegisterAsync(ppe_register model)
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
                await _dataContext.SaveChangesAsync();
                var res = GenerateInvestmentDailySchedule(model.RegisterId);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<RegisterRegRespObj> UpdateReassessmentAsync(ppe_register model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();

                if (model.RegisterId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_register.FindAsync(model.RegisterId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                {
                    await _dataContext.ppe_register.AddAsync(model);
                }
                await _dataContext.SaveChangesAsync();
                var targetIds = new List<int>();
                targetIds.Add(model.RegisterId);

                GoForApprovalRequest wfRequest = new GoForApprovalRequest
                {
                    Comment = "PPE Reassessment",
                    OperationId = (int)OperationsEnum.PPEReassessment,
                    TargetId = targetIds,
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
                        new RegisterRegRespObj
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
                        if (model.RegisterId > 0)
                        {
                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(model.RegisterId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                        }
                        else
                        {
                            await _dataContext.ppe_register.AddAsync(model);
                        }
                        await _dataContext.SaveChangesAsync();

                        await _trans.CommitAsync();
                        return new RegisterRegRespObj
                        {
                            RegisterId = res.TargetId,
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
                        return new RegisterRegRespObj
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
                        return new RegisterRegRespObj
                        {
                            RegisterId = model.RegisterId,
                            Status = res.Status
                        };
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new RegisterRegRespObj
            {
                RegisterId = model.RegisterId,
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
            };
        }

        public async Task<RegisterRegRespObj> UpdateDisposalAsync(ppe_register model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();

                if (model.RegisterId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_register.FindAsync(model.RegisterId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                {
                    await _dataContext.ppe_register.AddAsync(model);
                }
                await _dataContext.SaveChangesAsync();
                var targetIds = new List<int>();
                targetIds.Add(model.RegisterId);

                GoForApprovalRequest wfRequest = new GoForApprovalRequest
                {
                    Comment = "PPE Disposal",
                    OperationId = (int)OperationsEnum.PPEDisposal,
                    TargetId = targetIds,
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
                        new RegisterRegRespObj
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
                        if (model.RegisterId > 0)
                        {
                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(model.RegisterId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                        }
                        else
                        {
                            await _dataContext.ppe_register.AddAsync(model);
                        }
                        await _dataContext.SaveChangesAsync();

                        await _trans.CommitAsync();
                        return new RegisterRegRespObj
                        {
                            RegisterId = res.TargetId,
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
                        return new RegisterRegRespObj
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
                        return new RegisterRegRespObj
                        {
                            RegisterId = model.RegisterId,
                            Status = res.Status
                        };
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new RegisterRegRespObj
            {
                RegisterId = model.RegisterId,
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
            };
        }

        public IEnumerable<RegisterObj> GetRegisterByIdAsync(int id)
        {
            try
            {
                var currentItem = _dataContext.ppe_register.Where(a => a.RegisterId == id).FirstOrDefault();
                TimeSpan usedLifeOfAsset = (DateTime.Today - currentItem.DepreciationStartDate);
                int differenceInDays = usedLifeOfAsset.Days;
                int remainingUsefulLife = currentItem.UsefulLife - differenceInDays;
                var now = DateTime.Now.Date;
                var Application = (from a in _dataContext.ppe_register
                                   join e in _dataContext.ppe_dailyschedule on a.AdditionFormId equals e.AdditionId
                                   where a.Deleted == false && e.PeriodDate.Value.Date == now.Date
                                   && a.RegisterId == id
                                   orderby a.CreatedOn ascending
                                   select new RegisterObj
                                   {
                                       RegisterId = a.RegisterId,
                                       AdditionFormId = a.AdditionFormId,
                                       AssetClassificationId = a.AssetClassificationId,
                                       AssetNumber = a.AssetNumber,
                                       LpoNumber = a.LpoNumber,
                                       Description = a.Description,
                                       Cost = a.Cost,
                                       DateOfPurchaase = a.DateOfPurchaase,
                                       Quantity = a.Quantity,
                                       DepreciationStartDate = a.DepreciationStartDate,
                                       UsefulLife = a.UsefulLife,
                                       ResidualValue = a.ResidualValue,
                                       Location = a.Location,
                                       DepreciationForThePeriod = e.DepreciationForThePeriod,
                                       AccumulatedDepreciation = e.AccumulatedDepreciation,
                                       ProposedUsefulLife = a.ProposedUsefulLife,
                                       ProposedResidualValue = a.ProposedResidualValue,
                                       NetBookValue = e.CB,
                                       Active = true,
                                       CreatedBy = a.CreatedBy,
                                       CreatedOn = a.CreatedOn,
                                       UpdatedBy = a.UpdatedBy,
                                       UpdatedOn = a.UpdatedOn,
                                       RemainingUsefulLife = remainingUsefulLife,
                                   }).ToList();

                var res = Application;
                foreach (var j in res)
                {
                    j.ClassificationName = _dataContext.ppe_assetclassification.Where(c => c.AsetClassificationId == j.AssetClassificationId).FirstOrDefault()?.ClassificationName ?? null;
                }

                return Application;




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<RegisterObj> GetAllRegister()
        {
            try
            {
                //var now = DateTime.Now.Date;
                var Application = (from a in _dataContext.ppe_register
                                   //join e in _dataContext.ppe_dailyschedule on a.AdditionFormId equals //e.AdditionId
                                   where a.Deleted == false // && e.PeriodDate.Value.Date == now.Date
                                   orderby a.CreatedOn ascending
                                   select new RegisterObj


                                   {
                                       RegisterId = a.RegisterId,
                                       AdditionFormId = a.AdditionFormId,
                                       AssetClassificationId = a.AssetClassificationId,
                                       AssetNumber = a.AssetNumber,
                                       LpoNumber = a.LpoNumber,
                                       Description = a.Description,
                                       Cost = a.Cost,
                                       DateOfPurchaase = a.DateOfPurchaase,
                                       Quantity = a.Quantity,
                                       DepreciationStartDate = a.DepreciationStartDate,
                                       UsefulLife = a.UsefulLife,
                                       ResidualValue = a.ResidualValue,
                                       Location = a.Location,
                                       //DepreciationForThePeriod = e.DepreciationForThePeriod,
                                       DepreciationForThePeriod = a.DepreciationForThePeriod,
                                       //AccumulatedDepreciation = e.AccumulatedDepreciation,
                                       AccumulatedDepreciation = a.AccumulatedDepreciation,
                                       ProposedUsefulLife = a.ProposedUsefulLife,
                                       ProposedResidualValue = a.ProposedResidualValue,
                                       //NetBookValue = e.CB,
                                       NetBookValue = a.NetBookValue,
                                       Active = true,
                                       CreatedBy = a.CreatedBy,
                                       CreatedOn = a.CreatedOn,
                                       UpdatedBy = a.UpdatedBy,
                                       UpdatedOn = a.UpdatedOn
                                       //RemainingUsefulLife = remainingUsefulLife,
                                   }).ToList();
                var res = Application;
                foreach (var j in res)
                {
                    j.ClassificationName = _dataContext.ppe_assetclassification.Where(c => c.AsetClassificationId == j.AssetClassificationId).FirstOrDefault()?.ClassificationName ?? null;
                }
                return Application;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UploadRegisterAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                var subGlResponse = await _financeRequest.GetAllSubGlAsync();
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<RegisterObj> uploadedRecord = new List<RegisterObj>();
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
                                var item = new RegisterObj
                                {
                                    AssetNumber = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    LpoNumber = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                                    ClassificationName = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                                    Description = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,
                                    Cost = workSheet.Cells[i, 5].Value != null ? decimal.Parse(workSheet.Cells[i, 5].Value.ToString()) : 0,
                                    DateOfPurchaase = workSheet.Cells[i, 6].Value != null ? Convert.ToDateTime(workSheet.Cells[i, 6]?.Value) : DateTime.Now,
                                    Quantity = workSheet.Cells[i, 7].Value != null ? int.Parse(workSheet.Cells[i, 7].Value.ToString()) : 0,
                                    DepreciationStartDate = workSheet.Cells[i, 8].Value != null ? Convert.ToDateTime(workSheet.Cells[i, 8]?.Value) : DateTime.Now,
                                    UsefulLife = workSheet.Cells[i, 9].Value != null ? int.Parse(workSheet.Cells[i, 9].Value.ToString()) : 0,
                                    ResidualValue = workSheet.Cells[i, 10].Value != null ? int.Parse(workSheet.Cells[i, 10].Value.ToString()) : 0,
                                    Location = workSheet.Cells[i, 11].Value != null ? workSheet.Cells[i, 11].Value.ToString() : null,
                                    DepreciationForThePeriod = workSheet.Cells[i, 12].Value != null ? decimal.Parse(workSheet.Cells[i, 12].Value.ToString()) : 0,
                                    AccumulatedDepreciation = workSheet.Cells[i, 13].Value != null ? decimal.Parse(workSheet.Cells[i, 13].Value.ToString()) : 0,
                                    NetBookValue = workSheet.Cells[i, 14].Value != null ? decimal.Parse(workSheet.Cells[i, 14].Value.ToString()) : 0,
                                    
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
                        var classificationName = _dataContext.ppe_assetclassification.Where(c => c.ClassificationName == item.ClassificationName).FirstOrDefault()?.AsetClassificationId ?? 0;
                        //var SubGlAdditionCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlAdditionCode)?.SubGLId ?? 0;
                        //var SubGlDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlDepreciationCode)?.SubGLId ?? 0;
                        //var SubGlAccumulatedDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlAccumulatedDepreciationCode)?.SubGLId ?? 0;
                        //var SubGlDisposalCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlDisposalCode)?.SubGLId ?? 0;
                        var register = _dataContext.ppe_register.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
                        if (register != null)
                        {
                            register.AssetNumber = item.AssetNumber;
                            register.LpoNumber = item.LpoNumber;
                            register.AssetClassificationId = classificationName;
                            register.Description = item.Description;
                            register.Cost = item.Cost;
                            register.DateOfPurchaase = item.DateOfPurchaase;
                            register.Quantity = item.Quantity;
                            register.DepreciationStartDate = item.DepreciationStartDate;
                            register.UsefulLife = item.UsefulLife;
                            register.ResidualValue = item.ResidualValue;
                            register.DepreciationForThePeriod = item.DepreciationForThePeriod;
                            register.AccumulatedDepreciation = item.AccumulatedDepreciation;
                            register.NetBookValue = item.NetBookValue;
                            //register.SubGlAddition = SubGlAdditionCode;
                            //register.SubGlDepreciation = SubGlDepreciationCode;
                            //register.SubGlAccumulatedDepreciation = SubGlAccumulatedDepreciationCode;
                            //register.SubGlDisposal = SubGlDisposalCode;
                            register.Location = item.Location;
                            register.Active = true;
                            register.Deleted = false;
                            register.UpdatedBy = createdBy;
                            register.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var registers = new ppe_register();

                            registers.AssetNumber = item.AssetNumber;
                            registers.LpoNumber = item.LpoNumber;
                            registers.AssetClassificationId = classificationName;
                            registers.Description = item.Description;
                            registers.Cost = item.Cost;
                            registers.DateOfPurchaase = item.DateOfPurchaase;
                            registers.Quantity = item.Quantity;
                            registers.DepreciationStartDate = item.DepreciationStartDate;
                            registers.UsefulLife = item.UsefulLife;
                            registers.ResidualValue = item.ResidualValue;
                            registers.DepreciationForThePeriod = item.DepreciationForThePeriod;
                            registers.AccumulatedDepreciation = item.AccumulatedDepreciation;
                            registers.NetBookValue = item.NetBookValue;
                            //registers.SubGlAddition = SubGlAdditionCode;
                            //registers.SubGlDepreciation = SubGlDepreciationCode;
                            //registers.SubGlAccumulatedDepreciation = SubGlAccumulatedDepreciationCode;
                            //registers.SubGlDisposal = SubGlDisposalCode;
                            registers.Location = item.Location;
                            registers.Active = true;
                            registers.Deleted = false;
                            registers.CreatedBy = createdBy;
                            registers.CreatedOn = DateTime.Now;

                            await _dataContext.ppe_register.AddAsync(register);
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

        public byte[] GenerateExportRegister()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Asset Number");
            dt.Columns.Add("LPO Number");
            dt.Columns.Add("Classification Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("Cost");
            dt.Columns.Add("Date Of Purchase");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Depreciation Start Date");
            dt.Columns.Add("Useful Life");
            dt.Columns.Add("Residual Value");
            dt.Columns.Add("Location");
            dt.Columns.Add("Depreciation For The Period");
            dt.Columns.Add("Accumulated Depreciation");
            dt.Columns.Add("Net Book Value");
            //dt.Columns.Add("SubGL Addition");
            //dt.Columns.Add("SubGL Depreciation");
            //dt.Columns.Add("SubGL Accumulated Depreciation");
            //dt.Columns.Add("SubGL Disposal");
            var subGlResponse = _financeRequest.GetAllSubGlAsync().Result;
            var register = (from a in _dataContext.ppe_register
                            where a.Deleted == false
                            select new RegisterObj
                            {
                                RegisterId = a.RegisterId,
                                AssetNumber = a.AssetNumber,
                                LpoNumber = a.LpoNumber,
                                AssetClassificationId = a.AssetClassificationId,
                                Description = a.Description,
                                Cost = a.Cost,
                                DateOfPurchaase = a.DateOfPurchaase,
                                Quantity = a.Quantity,
                                DepreciationStartDate = a.DepreciationStartDate,
                                UsefulLife = a.UsefulLife,
                                ResidualValue = a.ResidualValue,
                                Location = a.Location,
                                DepreciationForThePeriod = a.DepreciationForThePeriod,
                                AccumulatedDepreciation = a.AccumulatedDepreciation,
                                NetBookValue = a.NetBookValue,
                                //SubGlAddition = a.SubGlAddition,
                                //SubGlDepreciation = a.SubGlDepreciation,
                                //SubGlAccumulatedDepreciation = a.SubGlAccumulatedDepreciation,
                                //SubGlDisposal = a.SubGlDisposal,
                            }).ToList();
            //if (register.Count() > 0)
            //{
            //    foreach (var res in register)
            //    {
            //        res.SubGlAdditionCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAddition)?.SubGLCode;
            //        res.SubGlDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlDepreciation)?.SubGLCode;
            //        res.SubGlAccumulatedDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAccumulatedDepreciation)?.SubGLCode;
            //        res.SubGlDisposalCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlDisposal)?.SubGLCode;
            //    }
            //}
            var classificationName = _dataContext.ppe_assetclassification.Where(c => c.Deleted == false).ToList();

            foreach (var kk in register)

            {
                var row = dt.NewRow();
                row["Asset Number"] = kk.AssetNumber;
                row["LPO Number"] = kk.LpoNumber;
                row["Classification Name"] = classificationName.FirstOrDefault(a => a.AsetClassificationId == kk.AssetClassificationId)?.ClassificationName;
                row["Description"] = kk.Description;
                row["Cost"] = kk.Cost;
                row["Date Of Purchase"] = kk.DateOfPurchaase;
                row["Quantity"] = kk.Quantity;
                row["Depreciation Start Date"] = kk.DepreciationStartDate;
                row["Useful Life"] = kk.UsefulLife;
                row["Residual Value"] = kk.ResidualValue;
                row["Location"] = kk.Location;
                row["Depreciation For The Period"] = kk.DepreciationForThePeriod;
                row["Accumulated Depreciation"] = kk.AccumulatedDepreciation;
                row["Net Book Value"] = kk.NetBookValue;
                //row["SubGL Addition"] = kk.SubGlAdditionCode;
                //row["SubGL Depreciation"] = kk.SubGlDepreciationCode;
                //row["SubGL Accumulated Depreciation"] = kk.SubGlAccumulatedDepreciationCode;
                //row["SubGL Disposal"] = kk.SubGlDisposalCode;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (register != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Register");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public async Task<StaffApprovalRegRespObj> ReassessmentStaffApprovals(StaffApprovalObj request)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _serverRequest.UserDataAsync();

                var currentItem = await _dataContext.ppe_register.FindAsync(request.TargetId);

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

                using (var _trans = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(currentItem);

                            decimal monthlyDepreciation = ((currentItem.Cost - currentItem.ResidualValue) / currentItem.UsefulLife);
                            decimal dailyDepreciationCharge = (monthlyDepreciation / 30);
                            //var residlValue = _dataContext.ppe_assetclassification.Where(c => c.AsetClassificationId == currentItem.AssetClassificationId).FirstOrDefault().ResidualValue;
                            //var residualValue = ((residlValue * currentItem.Cost)/100);
                            var depreciationStartDate = DateTime.Now;
                            int dailyPeriod = currentItem.UsefulLife * 30;
                            decimal dailyCB = currentItem.Cost;
                            decimal accdailyDepreciationCharge = 0;
                            decimal accdailyAccumilative = 0;
                            var res = GenerateInvestmentDailySchedule(currentItem.RegisterId);

                            itemToUpdate.RegisterId = currentItem.RegisterId;
                            itemToUpdate.UsefulLife = currentItem.ProposedUsefulLife > 0 ? currentItem.ProposedUsefulLife : itemToUpdate.UsefulLife;
                            itemToUpdate.ResidualValue = currentItem.ProposedResidualValue > 0 ? currentItem.ProposedResidualValue : itemToUpdate.ResidualValue;
                            itemToUpdate.DepreciationForThePeriod = dailyDepreciationCharge + accdailyAccumilative;
                            itemToUpdate.NetBookValue = (dailyCB - dailyDepreciationCharge);
                            itemToUpdate.AccumulatedDepreciation = dailyDepreciationCharge + accdailyDepreciationCharge;
                            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(itemToUpdate);
                            _dataContext.SaveChanges();

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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
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

        public async Task<StaffApprovalRegRespObj> DisposalStaffApprovals(StaffApprovalObj request)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _serverRequest.UserDataAsync();

                var currentItem = await _dataContext.ppe_register.FindAsync(request.TargetId);

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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
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

                            var itemToUpdate = await _dataContext.ppe_register.FindAsync(currentItem.RegisterId);
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

        public async Task<IEnumerable<ppe_register>> GetDisposalAwaitingApprovals(List<int> disposalIds, List<string> tokens)
        {
            var item = await _dataContext.ppe_register.Where(s => disposalIds.Contains(s.RegisterId) && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public async Task<IEnumerable<ppe_register>> GetReassessmentAwaitingApprovals(List<int> reassessmentIds, List<string> tokens)
        {
            var item = await _dataContext.ppe_register.Where(s => reassessmentIds.Contains(s.RegisterId) && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public IEnumerable<TransactionObj> GetEndOfMonthDepreciation(DateTime date)
        {
            try
            {
                var data = (from a in _dataContext.ppe_register 
                            join b in _dataContext.ppe_dailyschedule on a.AdditionFormId equals b.AdditionId
                            //join c in _dataContext.ppe_additionform on a.AdditionFormId equals c.AdditionFormId
                            where a.Deleted == false && b.PeriodDate.Value.Date == date.Date && b.EndPeriod == true                         
                            select new TransactionObj
                            {
                                IsApproved = false,
                                CasaAccountNumber = string.Empty,
                                CompanyId = a.CompanyId,
                                Amount = b.DepreciationForThePeriod,
                                CurrencyId = 0,
                                Description = "PPE Monthly Depreciation",
                                DebitGL = _dataContext.ppe_assetclassification.FirstOrDefault(x=>x.AsetClassificationId == a.AssetClassificationId).SubGlDepreciation,
                                CreditGL = _dataContext.ppe_assetclassification.FirstOrDefault(x => x.AsetClassificationId == a.AssetClassificationId).SubGlAccumulatedDepreciation,
                                ReferenceNo = a.AssetNumber,
                                OperationId = (int)OperationsEnum.PPEAdditionApproval,
                                JournalType = "System",
                                RateType = 1,//Buying Rate
                            }).ToList();

                foreach(var item in data)
                {
                    var res = _financeRequest.PassEntryToFinance(item).Result;
                    if (res.Status.IsSuccessful)
                    {

                    }
                }

                return data;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool GenerateInvestmentDailySchedule(int RegisterId)
        {
            var currentItem = _dataContext.ppe_register.Find(RegisterId);
            decimal monthlyDepreciation = ((currentItem.Cost - currentItem.ResidualValue) / currentItem.UsefulLife);
            decimal dailyDepreciationCharge = (monthlyDepreciation / 30);
            var day = DateTime.UtcNow.Date;
            var noOfDaysInThePeriod = day.ToString("D").Split()[0];
            TimeSpan usedLifeOfAsset = (DateTime.Today - currentItem.DepreciationStartDate);
            int differenceInDays = usedLifeOfAsset.Days;
            decimal accumulatedDepreciation = (dailyDepreciationCharge * (differenceInDays));
            decimal netBookValue = currentItem.Cost - accumulatedDepreciation;

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
                    dailyschedule.RegisterId = currentItem.RegisterId;
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