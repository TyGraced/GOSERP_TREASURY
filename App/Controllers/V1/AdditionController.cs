using AutoMapper;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public class AdditionController : Controller
    {
        private readonly IAdditionService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        public AdditionController(
            IAdditionService additionService,
            IMapper mapper,
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor,
            IIdentityServerRequest serverRequest,
            ILoggerService logger)
        {
            _mapper = mapper;
            _repo = additionService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
        }

        [HttpGet(ApiRoutes.Addition.GET_ALL_ADDITION)]

        public async Task<ActionResult<AdditionFormRespObj>> GetAllAdditionAsync()
        {
            try
            {
                var response = await _repo.GetAllAdditionAsync();
                return new AdditionFormRespObj
                {
                    AdditionForms = _mapper.Map<List<AdditionFormObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AdditionFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Addition.GET_ADDITION_BY_ID)]
        public async Task<ActionResult<AdditionFormRespObj>> GetAdditionByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new AdditionFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "AdditionForm Id is required" } }
                };
            }

            var response = await _repo.GetAdditionByIdAsync(search.SearchId);
            var resplist = new List<ppe_additionform> { response };
            return new AdditionFormRespObj
            {
                AdditionForms = _mapper.Map<List<AdditionFormObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.Addition.ADD_UPDATE_ADDITION)]
        public async Task<ActionResult<AdditionFormRegRespObj>> AddUpDateAddition([FromBody] AddUpdateAdditionFormObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                ppe_additionform item = null;
                if (model.AdditionFormId > 0)
                {
                    item = await _repo.GetAdditionByIdAsync(model.AdditionFormId);
                    if (item == null)
                        return new AdditionFormRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new ppe_additionform();
                domainObj.AdditionFormId = model.AdditionFormId > 0 ? model.AdditionFormId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.LpoNumber = model.LpoNumber;
                domainObj.DateOfPurchase = model.DateOfPurchase;
                domainObj.Description = model.Description;
                domainObj.Quantity = model.Quantity;
                domainObj.Cost = model.Cost;
                domainObj.SubGlAddition = model.SubGlAddition;
                domainObj.Location = model.Location;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.AdditionFormId > 0 ? DateTime.Today : DateTime.Today;

                var res = await _repo.AddUpdateAdditionAsync(domainObj);
                return new AdditionFormRegRespObj
                {
                    AdditionFormId = domainObj.AdditionFormId,
                    Status = res.Status,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AdditionFormRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Addition.DELETE_ADDITION)]
        public async Task<IActionResult> DeleteAddition([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteAdditionAsync(id);
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

        [HttpGet(ApiRoutes.Addition.DOWNLOAD_ADDITION)]
        public async Task<ActionResult<AdditionFormRespObj>> GenerateExportAdditionForm()
        {
            var response = _repo.GenerateExportAddition();

            return new AdditionFormRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.Addition.UPLOAD_ADDITION)]
        public async Task<ActionResult<AdditionFormRegRespObj>> UploadAdditionAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityService.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadAdditionAsync(image, createdBy);
                return new AdditionFormRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AdditionFormRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Addition.ADDITION_STAFF_APPROVAL)]
        public async Task<IActionResult> AdditionStaffApproval([FromBody]StaffApprovalObj request)
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
                var res = await _repo.AdditionStaffApprovals(request);
                if (!res.Status.IsSuccessful) return BadRequest(res);
                return Ok(res);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet(ApiRoutes.Addition.ADDITION_STAFF_APPROVAL_AWAITNG)]
        public async Task<IActionResult> GetCurrentStaffAdditionawaittingAprovals()
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
                var additions = await _repo.GetAdditionAwaitingApprovals(res.workflowTasks.Select(x =>
                 x.TargetId).ToList(), res.workflowTasks.Select(s =>
                 s.WorkflowToken).ToList());


                return Ok(new AdditionFormRespObj
                {
                    AdditionForms = additions.Select(d => new AdditionFormObj
                    {
                        AdditionFormId = d.AdditionFormId,
                        Active = d.Active,
                        AssetClassificationId = d.AssetClassificationId,
                        Cost = d.Cost,
                        DateOfPurchase = d.DateOfPurchase,
                        DepreciationStartDate = d.DepreciationStartDate,
                        Description = d.Description,
                        Quantity = d.Quantity,
                        Location = d.Location,
                        LpoNumber = d.LpoNumber,
                        ResidualValue = d.ResidualValue,
                        SubGlAddition = d.SubGlAddition,
                        UsefulLife = d.UsefulLife
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = additions.Count() < 1 ? "No AdditionForm  awaiting approvals" : null
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        [HttpGet(ApiRoutes.Addition.GET_AWAITING_APPROVAL_LIST)]
        public async Task<ActionResult<AdditionFormRespObj>> GetAdditionForAppraisalAsync()
        {
            try
            {
                return await _repo.GetAdditionForAppraisalAsync();

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AdditionFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}


