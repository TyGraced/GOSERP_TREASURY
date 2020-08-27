using AutoMapper;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PPE.AuthHandler;
using PPE.Contracts.Response;
using PPE.Contracts.V1;
using PPE.Data;
using PPE.DomainObjects.PPE;
using PPE.Repository.Interface;
using Puchase_and_payables.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Controllers.V1
{
    [Authorize]
    public class RegisterController : Controller
    {
        private readonly IRegisterService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly DataContext _dataContext;
        public RegisterController(IRegisterService registerService, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest serverRequest, DataContext dataContext)
        {
            _mapper = mapper;
            _repo = registerService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
            _dataContext = dataContext;
        }

        [HttpGet(ApiRoutes.Register.GET_ALL_REGISTER)]
        public async Task<ActionResult<RegisterRespObj>> GetAllRegister()
        {
            try
            {
                var response =  _repo.GetAllRegister();
                return new RegisterRespObj
                {
                    Registers = _mapper.Map<List<RegisterObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new RegisterRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Register.GET_REGISTER_BY_ID)]
        public async Task<ActionResult<RegisterRespObj>> GetRegisterByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new RegisterRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Reassessment Id is required" } }
                };
            }

            var response =   _repo.GetRegisterByIdAsync(search.SearchId);
            //var resplist = new List<ppe_register> { response };
            return new RegisterRespObj
            {
                Registers = _mapper.Map<List<RegisterObj>>(response),
            };
        }


        [HttpPost(ApiRoutes.Register.ADD_UPDATE_REGISTER)]
        public async Task<ActionResult<RegisterRegRespObj>> AddUpdateRegisterAsync([FromBody] AddUpdateRegisterObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                IEnumerable<RegisterObj> item = new List<RegisterObj>();
                if (model.RegisterId > 0)
                {
                    item = _repo.GetRegisterByIdAsync(model.RegisterId);  //_repo.GetRegisterByIdAsync(model.RegisterId);
                    if (item == null)
                        return new RegisterRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new ppe_register();
                domainObj.RegisterId = model.RegisterId > 0 ? model.RegisterId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.AdditionFormId = model.AdditionFormId;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.AssetNumber = model.AssetNumber;
                domainObj.LpoNumber = model.LpoNumber;
                domainObj.AssetClassificationId = model.AssetClassificationId;
                domainObj.Description = model.Description;
                domainObj.Cost = model.Cost;
                domainObj.DateOfPurchaase = model.DateOfPurchaase;
                domainObj.Quantity = model.Quantity;
                domainObj.DepreciationStartDate = model.DepreciationStartDate;
                domainObj.UsefulLife = model.UsefulLife;
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.Location = model.Location;
                //domainObj.DepreciationForThePeriod = model.DepreciationForThePeriod;
                //domainObj.AccumulatedDepreciation = model.AccumulatedDepreciation;
                //domainObj.NetBookValue = model.NetBookValue;
                domainObj.ProposedResidualValue = model.ProposedResidualValue;
                domainObj.ProposedUsefulLife = model.ProposedUsefulLife;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.RegisterId > 0 ? DateTime.Today : DateTime.Today;

               await _repo.AddUpdateRegisterAsync(domainObj);
                return new RegisterRegRespObj
                {
                    RegisterId = domainObj.RegisterId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } } };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        //         foreach (var item in multipleUsefulLife)
        //                {
        //                    if (item.RegisterId > 0)
        //                    {
        //                        var itemToUpdate = _dataContext.ppe_register.Find(item.RegisterId);
        //        itemToUpdate.RegisterId = item.RegisterId;
        //                        itemToUpdate.ProposedUsefulLife = item.ProposedUsefulLife;
        //                        _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(itemToUpdate);
        //                        _dataContext.SaveChanges();
        //                    }
        //};
        //                return true;

        [HttpPost(ApiRoutes.Register.UPDATE_MULTIPLE_USEFULLIFE)]
        public async Task<ActionResult<RegisterRegRespObj>> UpdateMultipleUsefulLife([FromBody] List<RegisterObj> model)
        {
            try
            {
                foreach (var item in model)
                {
                    if (item.RegisterId > 0)
                    {
                        var itemToUpdate = _dataContext.ppe_register.Find(item.RegisterId);
                        itemToUpdate.RegisterId = item.RegisterId;
                        itemToUpdate.ProposedUsefulLife = item.ProposedUsefulLife;
                        await _repo.UpdateReassessmentAsync(itemToUpdate);
                    }
                };
                var domainObj = new ppe_register();
                return new RegisterRegRespObj
                {
                    RegisterId = domainObj.RegisterId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Gone for approval" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Register.UPDATE_MULTIPLE_RESIDUALVALUE)]
        public async Task<ActionResult<RegisterRegRespObj>> UpdateMultipleResidualValue([FromBody] List<RegisterObj> model)
        {
            try
            {
                foreach (var item in model)
                {
                    if (item.RegisterId > 0)
                    {
                        var itemToUpdate = _dataContext.ppe_register.Find(item.RegisterId);
                        itemToUpdate.RegisterId = item.RegisterId;
                        itemToUpdate.ProposedResidualValue = item.ProposedResidualValue;
                        await _repo.UpdateReassessmentAsync(itemToUpdate);
                    }
                };
                var domainObj = new ppe_register();
                return new RegisterRegRespObj
                {
                    RegisterId = domainObj.RegisterId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Gone for approval" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Register.UPDATE_REASSESSMENT)]
        public async Task<ActionResult<RegisterRegRespObj>> UpdateReassessment([FromBody] AddUpdateRegisterObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                IEnumerable<RegisterObj> item = new List<RegisterObj>();
                if (model.RegisterId > 0)
                {
                    item = _repo.GetRegisterByIdAsync(model.RegisterId);
                    if (item == null)
                        return new RegisterRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new ppe_register();
                domainObj.RegisterId = model.RegisterId > 0 ? model.RegisterId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.AssetNumber = model.AssetNumber;
                domainObj.LpoNumber = model.LpoNumber;
                domainObj.AssetClassificationId = model.AssetClassificationId;
                domainObj.Description = model.Description;
                domainObj.Cost = model.Cost;
                domainObj.DateOfPurchaase = model.DateOfPurchaase;
                domainObj.Quantity = model.Quantity;
                domainObj.DepreciationStartDate = model.DepreciationStartDate;
                domainObj.UsefulLife = model.UsefulLife;
                domainObj.ProposedResidualValue = model.ProposedResidualValue;
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.Location = model.Location;
                domainObj.DepreciationForThePeriod = model.DepreciationForThePeriod;
                domainObj.AccumulatedDepreciation = model.AccumulatedDepreciation;
                domainObj.NetBookValue = model.NetBookValue;
                domainObj.RemainingUsefulLife = model.RemainingUsefulLife;
                domainObj.ProposedUsefulLife = model.ProposedUsefulLife;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.RegisterId > 0 ? DateTime.Today : DateTime.Today;

                await _repo.UpdateReassessmentAsync(domainObj);
                return new RegisterRegRespObj
                {
                    RegisterId = domainObj.RegisterId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Gone for approval" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Register.UPDATE_DISPOSAL)]
        public async Task<ActionResult<RegisterRegRespObj>> AddUpDateDisposalAsync([FromBody] AddUpdateRegisterObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                IEnumerable<RegisterObj> item = new List<RegisterObj>();
                if (model.RegisterId > 0)
                {
                    item =  _repo.GetRegisterByIdAsync(model.RegisterId);
                    if (item == null)
                        return new RegisterRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new ppe_register();
                domainObj.RegisterId = model.RegisterId > 0 ? model.RegisterId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.AssetNumber = model.AssetNumber;
                domainObj.AssetClassificationId = model.AssetClassificationId;
                domainObj.Description = model.Description;
                domainObj.Cost = model.Cost;
                domainObj.DateOfPurchaase = model.DateOfPurchaase;
                domainObj.Quantity = model.Quantity;
                domainObj.DepreciationStartDate = model.DepreciationStartDate;
                domainObj.UsefulLife = model.UsefulLife;
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.Location = model.Location;
                domainObj.DepreciationForThePeriod = model.DepreciationForThePeriod;
                domainObj.AccumulatedDepreciation = model.AccumulatedDepreciation;
                domainObj.NetBookValue = model.NetBookValue;
                /*domainObj.ProceedFromDisposal = model.ProceedFromDisposal;
                domainObj.ProposedDisposalDate = model.ProposedDisposalDate;
                domainObj.ReasonForDisposal = model.ReasonForDisposal;*/
                domainObj.SubGlDisposal = model.SubGlDisposal;
                domainObj.SubGlDepreciation = model.SubGlDepreciation;
                domainObj.SubGlAccumulatedDepreciation = model.SubGlAccumulatedDepreciation;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.RegisterId > 0 ? DateTime.Today : DateTime.Today;

                await _repo.UpdateDisposalAsync(domainObj);
                return new RegisterRegRespObj
                {
                    RegisterId = domainObj.RegisterId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Register.DOWNLOAD_REGISTER)]
        public async Task<ActionResult<RegisterRespObj>> GenerateExportRegister()
        {
            var response = _repo.GenerateExportRegister();

            return new RegisterRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.Register.UPLOAD_REGISTER)]
        public async Task<ActionResult<RegisterRegRespObj>> UploadRegisterAsync()
        {
            try
            {
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }

                }

                var user = await _identityService.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = await _repo.UploadRegisterAsync(byteList, createdBy);
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Register.REASSESSMENT_STAFF_APPROVAL)]
        public async Task<IActionResult> ReassessmentStaffApproval([FromBody]StaffApprovalObj request)
        {
            try
            {
                if (request.TargetId < 1 || request.ApprovalStatus < 1 || string.IsNullOrEmpty(request.ApprovalComment))
                {
                    return BadRequest(new StaffApprovalRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "All Fields are required for this approval"
                            }
                        }
                    });
                }
                var res = await _repo.ReassessmentStaffApprovals(request);
                if (!res.Status.IsSuccessful) return BadRequest(res);
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet(ApiRoutes.Register.REASSESSMENT_STAFF_APPROVAL_AWAITNG)]
        public async Task<IActionResult> GetCurrentStaffReassessmentAwaittingAprovals()
        {

            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    var data1 = await result.Content.ReadAsStringAsync();
                    var res1 = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data1);
                    return BadRequest(new WorkflowTaskRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}"
                            }
                        }
                    });
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res == null)
                {
                    return BadRequest(new WorkflowTaskRespObj
                    {
                        Status = res.Status
                    });
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return Ok(new WorkflowTaskRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Pending Approval"
                            }
                        }
                    });
                }
                var register = await _repo.GetReassessmentAwaitingApprovals(res.workflowTasks.Select(x =>
                 x.TargetId).ToList(), res.workflowTasks.Select(s =>
                 s.WorkflowToken).ToList());

                var classificationName = _dataContext.ppe_assetclassification.Where(a => a.Deleted == false).ToList();
                return Ok(new RegisterRespObj
                {
                    Registers = register.Select(d => new RegisterObj
                    {
                        RegisterId = d.RegisterId,
                        Active = d.Active,
                        AssetClassificationId = d.AssetClassificationId,
                        ClassificationName = classificationName.FirstOrDefault(a => a.AsetClassificationId == d.AssetClassificationId)?.ClassificationName ?? null,
                        AssetNumber = d.AssetNumber,
                        AdditionFormId = d.AdditionFormId,
                        Cost = d.Cost,
                        DateOfPurchaase = d.DateOfPurchaase,
                        DepreciationStartDate = d.DepreciationStartDate,
                        Description = d.Description,
                        Quantity = d.Quantity,
                        Location = d.Location,
                        LpoNumber = d.LpoNumber,
                        ResidualValue = d.ResidualValue,
                        RemainingUsefulLife = d.RemainingUsefulLife,
                        UsefulLife = d.UsefulLife,
                        ProposedResidualValue = d.ProposedResidualValue,
                        ProposedUsefulLife = d.ProposedUsefulLife,
                        DepreciationForThePeriod = d.DepreciationForThePeriod,
                        AccumulatedDepreciation = d.AccumulatedDepreciation,
                        NetBookValue = d.NetBookValue
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = register.Count() < 1 ? "No Register  awaiting approvals" : null
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost(ApiRoutes.Register.DISPOSAL_STAFF_APPROVAL)]
        public async Task<IActionResult> DisposalStaffApproval([FromBody]StaffApprovalObj request)
        {
            try
            {
                if (request.TargetId < 1 || request.ApprovalStatus < 1 || string.IsNullOrEmpty(request.ApprovalComment))
                {
                    return BadRequest(new StaffApprovalRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "All Fields are required for this approval"
                            }
                        }
                    });
                }
                var res = await _repo.DisposalStaffApprovals(request);
                if (!res.Status.IsSuccessful) return BadRequest(res);
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet(ApiRoutes.Register.DISPOSAL_STAFF_APPROVAL_AWAITNG)]
        public async Task<IActionResult> GetCurrentStaffDisposalAwaittingAprovals()
        {

            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    var data1 = await result.Content.ReadAsStringAsync();
                    var res1 = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data1);
                    return BadRequest(new WorkflowTaskRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}"
                            }
                        }
                    });
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res == null)
                {
                    return BadRequest(new WorkflowTaskRespObj
                    {
                        Status = res.Status
                    });
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return Ok(new WorkflowTaskRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Pending Approval"
                            }
                        }
                    });
                }
                var disposals = await _repo.GetDisposalAwaitingApprovals(res.workflowTasks.Select(x =>
                 x.TargetId).ToList(), res.workflowTasks.Select(s =>
                 s.WorkflowToken).ToList());


                return Ok(new RegisterRespObj
                {
                    Registers = disposals.Select(d => new RegisterObj
                    {
                        RegisterId = d.RegisterId,
                        Active = d.Active,
                        AssetClassificationId = d.AssetClassificationId,
                        Cost = d.Cost,
                        DateOfPurchaase = d.DateOfPurchaase,
                        DepreciationStartDate = d.DepreciationStartDate,
                        Description = d.Description,
                        Quantity = d.Quantity,
                        Location = d.Location,
                        AssetNumber = d.AssetNumber,
                        ResidualValue = d.ResidualValue,
                        DepreciationForThePeriod = d.DepreciationForThePeriod,
                        UsefulLife = d.UsefulLife,
                        AccumulatedDepreciation = d.AccumulatedDepreciation,
                        NetBookValue = d.NetBookValue,
                        SubGlAccumulatedDepreciation = d.SubGlAccumulatedDepreciation,
                        SubGlDepreciation = d.SubGlDepreciation,
                        SubGlDisposal = d.SubGlDisposal

                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = disposals.Count() < 1 ? "No Disposal  awaiting approvals" : null
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       [HttpPost(ApiRoutes.Register.GET_END_OF_MONTH_DEPRECIATION)]
        public async Task<ActionResult<FinTransacRegRespObj>> GetEntries([FromBody] EndOfDayRequestObj model)
        {
            try
            {
                var response = _repo.GetEndOfMonthDepreciation(model.RequestDate);
                return new FinTransacRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful"} }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new FinTransacRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}
