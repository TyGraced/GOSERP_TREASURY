using AutoMapper;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PPE.AuthHandler;
using PPE.Contracts.Response;
using PPE.Contracts.V1;
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
    public class DisposalController : Controller
    {
        private readonly IDisposalService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        public DisposalController(
            IDisposalService disposalService, 
            IMapper mapper, 
            IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerService logger,
            IIdentityServerRequest serverRequest)
        {
            _mapper = mapper;
            _repo = disposalService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
        }

        [HttpGet(ApiRoutes.Disposal.GET_ALL_DISPOSAL)]

        public async Task<ActionResult<DisposalRespObj>> GetAllDisposalAsync()
        {
            try
            {
                var response = await _repo.GetAllDisposalAsync();
                return new DisposalRespObj
                {
                    Disposals = _mapper.Map<List<DisposalObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new DisposalRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Disposal.GET_DISPOSAL_BY_ID)]
        public async Task<ActionResult<DisposalRespObj>> GetDisposalByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new DisposalRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Disposal Id is required" } }
                };
            }

            var response = await _repo.GetDisposalByIdAsync(search.SearchId);
            var resplist = new List<ppe_disposal> { response };
            return new DisposalRespObj
            {
                Disposals = _mapper.Map<List<DisposalObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.Disposal.ADD_UPDATE_DISPOSAL)]
        public async Task<ActionResult<DisposalRegRespObj>> AddUpDateDisposalAsync([FromBody] AddUpdateDisposalObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                ppe_disposal item = null;
                if (model.DisposalId > 0)
                {
                    item = await _repo.GetDisposalByIdAsync(model.DisposalId);
                    if (item == null)
                        return new DisposalRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new ppe_disposal();
                domainObj.DisposalId = model.DisposalId > 0 ? model.DisposalId : 0;
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
                domainObj.ProceedFromDisposal = model.ProceedFromDisposal;
                domainObj.ProposedDisposalDate = model.ProposedDisposalDate;
                domainObj.ReasonForDisposal = model.ReasonForDisposal;
                domainObj.RequestDate = model.RequestDate;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.DisposalId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateDisposalAsync(domainObj);
                return new DisposalRegRespObj
                {
                    DisposalId = domainObj.DisposalId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DisposalRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Disposal.DELETE_DISPOSAL)]
        public async Task<IActionResult> DeleteDisposal([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteDisposalAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                new DeleteRespObjt
                {
                    Deleted = true,
                    Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                });

        }

        [HttpGet(ApiRoutes.Disposal.DOWNLOAD_DISPOSAL)]
        public async Task<ActionResult<DisposalRespObj>> GenerateExportDisposal()
        {
            var response = _repo.GenerateExportDisposal();

            return new DisposalRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.Disposal.DISPOSAL_STAFF_APPROVAL)]
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

        [HttpGet(ApiRoutes.Disposal.DISPOSAL_STAFF_APPROVAL_AWAITNG)]
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


                return Ok(new DisposalRespObj
                {
                    Disposals = disposals.Select(d => new DisposalObj
                    {
                        DisposalId = d.DisposalId,
                        Active = d.Active,
                        AssetClassificationId = d.AssetClassificationId,
                        Cost = d.Cost,
                        //DateOfPurchase = d.DateOfPurchase,
                        DepreciationStartDate = d.DepreciationStartDate,
                        Description = d.Description,
                        Quantity = d.Quantity,
                        Location = d.Location,
                        //LpoNumber = d.LpoNumber,
                        ResidualValue = d.ResidualValue,
                        //SubGlAddition = d.SubGlAddition,
                        UsefulLife = d.UsefulLife
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

    }
}
