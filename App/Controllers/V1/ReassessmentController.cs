//using AutoMapper;
//using GOSLibraries.GOS_API_Response;
//using GOSLibraries.GOS_Error_logger.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using PPE.AuthHandler;
//using PPE.Contracts.Response;
//using PPE.Contracts.V1;
//using PPE.DomainObjects.PPE;
//using PPE.Repository.Interface;
//using Puchase_and_payables.Requests;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PPE.Controllers.V1
//{
//    [Authorize]
//    public class ReassessmentController : Controller
//    {
//        private readonly IReassessmentService _repo;
//        private readonly IMapper _mapper;
//        private readonly IIdentityService _identityService;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly ILoggerService _logger;
//        private readonly IIdentityServerRequest _serverRequest;

//        public ReassessmentController(
//            IReassessmentService reassessmentService, 
//            IMapper mapper, 
//            IIdentityService identityService, 
//            IHttpContextAccessor httpContextAccessor, 
//            ILoggerService logger, 
//            IIdentityServerRequest serverRequest)
//        {
//            _mapper = mapper;
//            _repo = reassessmentService;
//            _identityService = identityService;
//            _httpContextAccessor = httpContextAccessor;
//            _logger = logger;
//            _serverRequest = serverRequest;
//        }

//        [HttpGet(ApiRoutes.Reassessment.GET_ALL_REASSESSMENT)]
//        public async Task<ActionResult<ReassessmentRespObj>> GetAllReassessmentAsync()
//        {
//            try
//            {
//                var response = await _repo.GetAllReassessmentAsync();
//                return new ReassessmentRespObj
//                {
//                    Reassessments = _mapper.Map<List<ReassessmentObj>>(response),
//                };
//            }
//            catch (Exception ex)
//            {
//                var errorCode = ErrorID.Generate(5);
//                return new ReassessmentRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
//                };
//            }
//        }

//        [HttpGet(ApiRoutes.Reassessment.GET_REASSESSMENT_BY_ID)]
//        public async Task<ActionResult<ReassessmentRespObj>> GetReassessmentByIdAsync([FromQuery] SearchObj search)
//        {
//            if (search.SearchId < 1)
//            {
//                return new ReassessmentRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Reassessment Id is required" } }
//                };
//            }

//            var response = await _repo.GetReassessmentByIdAsync(search.SearchId);
//            var resplist = new List<ppe_reassessment> { response };
//            return new ReassessmentRespObj
//            {
//                Reassessments = _mapper.Map<List<ReassessmentObj>>(resplist),
//            };
//        }

//        [HttpPost(ApiRoutes.Reassessment.ADD_UPDATE_REASSESSMENT)]
//        public async Task<ActionResult<ReassessmentRegRespObj>> AddUpDateReassessment([FromBody] AddUpdateReassessmentObj model)
//        {
//            try
//            {
//                var user = await _identityService.UserDataAsync();
//                ppe_reassessment item = null;
//                if (model.ReassessmentId > 0)
//                {
//                    item = await _repo.GetReassessmentByIdAsync(model.ReassessmentId);
//                    if (item == null)
//                        return new ReassessmentRegRespObj
//                        {
//                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
//                        };
//                }

//                var domainObj = new ppe_reassessment();
//                domainObj.ReassessmentId = model.ReassessmentId > 0 ? model.ReassessmentId : 0;
//                domainObj.Active = true;
//                domainObj.CreatedBy = user.UserName;
//                domainObj.CreatedOn = DateTime.Today;
//                domainObj.Deleted = false;
//                domainObj.AssetNumber = model.AssetNumber;
//                domainObj.LpoNumber = model.LpoNumber;
//                domainObj.AssetClassificationId = model.AssetClassificationId;
//                domainObj.Description = model.Description;
//                domainObj.Cost = model.Cost;
//                domainObj.DateOfPurchaase = model.DateOfPurchaase;
//                domainObj.Quantity = model.Quantity;
//                domainObj.DepreciationStartDate = model.DepreciationStartDate;
//                domainObj.UsefulLife = model.UsefulLife;
//                //domainObj.UsefulLife = model.ProposedUsefulLife;
//                domainObj.ProposedResidualValue = model.ProposedResidualValue;
//                domainObj.ResidualValue = model.ResidualValue;
//                domainObj.Location = model.Location;
//                domainObj.DepreciationForThePeriod = model.DepreciationForThePeriod;
//                domainObj.AccumulatedDepreciation = model.AccumulatedDepreciation;
//                domainObj.NetBookValue = model.NetBookValue;
//                domainObj.RemainingUsefulLife = model.RemainingUsefulLife;
//                domainObj.ProposedUsefulLife = model.ProposedUsefulLife;
//                domainObj.UpdatedBy = user.UserName;
//                domainObj.UpdatedOn = model.ReassessmentId > 0 ? DateTime.Today : DateTime.Today;

//                await _repo.AddUpdateReassessmentAsync(domainObj);
//                return new ReassessmentRegRespObj
//                {
//                    ReassessmentId = domainObj.ReassessmentId,
//                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
//                };
//            }
//            catch (Exception ex)
//            {
//                var errorCode = ErrorID.Generate(5);
//                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
//                return new ReassessmentRegRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
//                };
//            }
//        }

//        /*[HttpPost(ApiRoutes.Reassessment.DELETE_REASSESSMENT)]
//        public async Task<IActionResult> DeleteReassessment([FromBody] DeleteRequest item)
//        {
//            var response = true;
//            var Ids = item.ItemIds;
//            foreach (var id in Ids)
//            {
//                response = await _repo.DeleteReassessmentAsync(id);
//            }
//            if (!response)
//                return BadRequest(
//                    new DeleteRespObjt
//                    {
//                        Deleted = false,
//                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
//                    });
//            return Ok(
//                new DeleteRespObjt
//                {
//                    Deleted = true,
//                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
//                });

//        }

//        [HttpGet(ApiRoutes.Reassessment.DOWNLOAD_REASSESSMENT)]
//        public async Task<ActionResult<ReassessmentRespObj>> GenerateExportReassessment()
//        {
//            var response = _repo.GenerateExportReassessment();

//            return new ReassessmentRespObj
//            {
//                export = response,
//            };
//        }*/

//        [HttpPost(ApiRoutes.Reassessment.REASSESSMENT_STAFF_APPROVAL)]
//        public async Task<IActionResult> ReassessmentStaffApproval([FromBody]StaffApprovalObj request)
//        {
//            try
//            {
//                if (request.TargetId < 1 || request.ApprovalStatus < 1 || string.IsNullOrEmpty(request.ApprovalComment))
//                {
//                    return BadRequest(new StaffApprovalRegRespObj
//                    {
//                        Status = new APIResponseStatus
//                        {
//                            IsSuccessful = false,
//                            Message = new APIResponseMessage
//                            {
//                                FriendlyMessage = "All Fields are required for this approval"
//                            }
//                        }
//                    });
//                }
//                var res = await _repo.ReassessmentStaffApprovals(request);
//                if (!res.Status.IsSuccessful) return BadRequest(res);
//                return Ok(res);
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }

//        }

//        [HttpGet(ApiRoutes.Reassessment.REASSESSMENT_STAFF_APPROVAL_AWAITNG)]
//        public async Task<IActionResult> GetCurrentStaffReassessmentAwaittingAprovals()
//        {

//            try
//            {
//                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
//                if (!result.IsSuccessStatusCode)
//                {
//                    var data1 = await result.Content.ReadAsStringAsync();
//                    var res1 = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data1);
//                    return BadRequest(new WorkflowTaskRespObj
//                    {
//                        Status = new APIResponseStatus
//                        {
//                            IsSuccessful = false,
//                            Message = new APIResponseMessage
//                            {
//                                FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}"
//                            }
//                        }
//                    });
//                }

//                var data = await result.Content.ReadAsStringAsync();
//                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

//                if (res == null)
//                {
//                    return BadRequest(new WorkflowTaskRespObj
//                    {
//                        Status = res.Status
//                    });
//                }

//                if (res.workflowTasks.Count() < 1)
//                {
//                    return Ok(new WorkflowTaskRespObj
//                    {
//                        Status = new APIResponseStatus
//                        {
//                            IsSuccessful = true,
//                            Message = new APIResponseMessage
//                            {
//                                FriendlyMessage = "No Pending Approval"
//                            }
//                        }
//                    });
//                }
//                var reassessments = await _repo.GetReassessmentAwaitingApprovals(res.workflowTasks.Select(x =>
//                 x.TargetId).ToList(), res.workflowTasks.Select(s =>
//                 s.WorkflowToken).ToList());


//                return Ok(new ReassessmentRespObj
//                {
//                    Reassessments = reassessments.Select(d => new ReassessmentObj
//                    {
//                        ReassessmentId = d.ReassessmentId,
//                        Active = d.Active,
//                        AssetClassificationId = d.AssetClassificationId,
//                        Cost = d.Cost,
//                        DateOfPurchaase = d.DateOfPurchaase,
//                        DepreciationStartDate = d.DepreciationStartDate,
//                        DepreciationForThePeriod = d.DepreciationForThePeriod,
//                        AccumulatedDepreciation = d.AccumulatedDepreciation,
//                        NetBookValue = d.NetBookValue,
//                        Description = d.Description,
//                        Quantity = d.Quantity,
//                        Location = d.Location,
//                        LpoNumber = d.LpoNumber,
//                        ResidualValue = d.ResidualValue,
//                        RemainingUsefulLife = d.RemainingUsefulLife,
//                        AssetNumber = d.AssetNumber,
//                        UsefulLife = d.UsefulLife
//                    }).ToList(),
//                    Status = new APIResponseStatus
//                    {
//                        IsSuccessful = true,
//                        Message = new APIResponseMessage
//                        {
//                            FriendlyMessage = reassessments.Count() < 1 ? "No Reassessment  awaiting approvals" : null
//                        }
//                    }
//                });
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }



//        }

//        [HttpPost(ApiRoutes.Reassessment.UPDATE_MULTIPLE_USEFULLIFE)]
//        public async Task<ActionResult<ReassessmentRespObj>> UpdateMultipleUsefulLife([FromBody] List<ReassessmentObj> model)
//        {
//            try
//            {
//                var identity = await _identityService.UserDataAsync();
//                var user = identity.UserName;
//                foreach (var item in model)
//                {
//                    item.CreatedBy = user;
//                    item.UpdatedBy = user;
//                }

//                var isDone = _repo.UpdateMultipleUsefulLife(model);

//                return new ReassessmentRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
//                };
//            }
//            catch (Exception ex)
//            {
//                var errorCode = ErrorID.Generate(5);
//                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
//                return new ReassessmentRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
//                };
//            }
//        }

//        [HttpPost(ApiRoutes.Reassessment.UPDATE_MULTIPLE_RESIDUALVALUE)]
//        public async Task<ActionResult<ReassessmentRespObj>> UpdateMultipleResidualValue([FromBody] List<ReassessmentObj> model)
//        {
//            try
//            {
//                var identity = await _identityService.UserDataAsync();
//                var user = identity.UserName;
//                foreach (var item in model)
//                {
//                    item.CreatedBy = user;
//                    item.UpdatedBy = user;
//                }

//                var isDone = _repo.UpdateMultipleResidualValue(model);

//                return new ReassessmentRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
//                };
//            }
//            catch (Exception ex)
//            {
//                var errorCode = ErrorID.Generate(5);
//                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
//                return new ReassessmentRespObj
//                {
//                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
//                };
//            }
//        }
//    }
//}
