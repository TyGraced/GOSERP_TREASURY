using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
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
    public class RegisterService : IRegisterService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterService(DataContext dataContext, IIdentityServerRequest serverRequest, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _serverRequest = serverRequest;
            _httpContextAccessor = httpContextAccessor;
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
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteRegisterAsync(int id)
        {
            var itemToDelete = await _dataContext.ppe_register.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ppe_register> GetRegisterByIdAsync(int id)
        {
            return await _dataContext.ppe_register.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_register>> GetAllRegisterAsync()
        {
            return await _dataContext.ppe_register.Where(d => d.Deleted == false).ToListAsync();
        }

        public byte[] GenerateExportRegister()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Asset Number");
            dt.Columns.Add("LPO Number");
            dt.Columns.Add("Classification");
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
            var category = (from a in _dataContext.ppe_register
                            where a.Deleted == false
                            select new ppe_register
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
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Asset Number"] = kk.AssetNumber;
                row["LPO Number"] = kk.LpoNumber;
                row["Classification"] = kk.AssetClassificationId;
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
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
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

        public async Task<StaffApprovalRegRespObj> RegisterStaffApprovals(StaffApprovalObj request)
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

                            TimeSpan usedLifeOfAsset = (currentItem.DepreciationStartDate - DateTime.Today);
                            int differenceInDays = usedLifeOfAsset.Days;
                            int remainingUsefulLife = currentItem.UsefulLife - differenceInDays;
                 
                            var reassessment = new ppe_reassessment
                            {
                                Active = true,
                                AssetClassificationId = currentItem.AssetClassificationId,
                                Cost = currentItem.Cost,
                                CreatedBy = user.UserName,
                                DateOfPurchaase = currentItem.DateOfPurchaase,
                                Description = currentItem.Description,
                                Location = currentItem.Location,
                                LpoNumber = currentItem.LpoNumber,
                                Quantity = currentItem.Quantity,
                                DepreciationForThePeriod = currentItem.DepreciationForThePeriod,
                                AccumulatedDepreciation = currentItem.AccumulatedDepreciation,
                                NetBookValue = currentItem.NetBookValue,
                                UsefulLife = currentItem.UsefulLife,
                                ResidualValue = currentItem.ResidualValue,
                                RemainingUsefulLife = remainingUsefulLife,
                                AssetNumber = currentItem.AssetNumber,
                            };



                            await AddUpdateReassessmentAsync(reassessment);

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

        private async Task<bool> AddUpdateReassessmentAsync(ppe_reassessment model)
        {
            try
            {
                if (model.ReassessmentId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(model.ReassessmentId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_reassessment.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ppe_register>> GetRegisterAwaitingApprovals(List<int> registerIds, List<string> tokens)
        {
            var item = await _dataContext.ppe_register.Where(s => registerIds.Contains(s.RegisterId) && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }
    }
}