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
using PPE.Data;
using PPE.DomainObjects.PPE;
using PPE.Repository.Implement.Addition;
using PPE.Repository.Interface;
using PPE.Requests;
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
        private readonly DataContext _dataContext;
        private readonly IAdditionService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IFinanceServerRequest _financeRequest;
        public AdditionController(
            DataContext dataContext,
            IAdditionService additionService,
            IMapper mapper,
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor,
            IIdentityServerRequest serverRequest,
            IFinanceServerRequest financeRequest,
            ILoggerService logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _repo = additionService;
            _identityService = identityService;
            _financeRequest = financeRequest;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
        }

        [HttpGet(ApiRoutes.Addition.GET_ALL_ADDITION)]

        public async Task<ActionResult<AdditionFormRespObj>> GetAllAdditionAsync( )
        {
            try
            {
                var response = await _repo.GetAllAdditionAsync();
                var resObj = new List<AdditionFormObj>();
                var subGlResponse = await _financeRequest.GetAllSubGlAsync();

                if (response.Count() > 0){
                    
                    resObj = _mapper.Map<List<AdditionFormObj>>(response);
                    foreach (var res in resObj)
                    {
                        res.SubGlAdditionName = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAddition)?.SubGLName;

                    }
                }


                return new AdditionFormRespObj
                {
                    AdditionForms = resObj,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Search complete, no record found."} }
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

                // List of additions
                var adds = await _repo.GetAllAdditionAsync();
                if (model.AdditionFormId == 0)
                {
                    if (adds.Select(e => e.LpoNumber.Trim().ToLower()).Contains(model.LpoNumber.Trim().ToLower()))
                    
                    {
                        return new AdditionFormRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "You can't have the same LpoNumber on this table" } }
                        };

                    }
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
                domainObj.SubGlDisposal = model.SubGlDisposal;
                domainObj.SubGlDepreciation = model.SubGlDepreciation;
                domainObj.SubGlAccumulatedDepreciation = model.SubGlAccumulatedDepreciation;
                domainObj.Location = model.Location;
                domainObj.AssetClassificationId = model.AssetClassificationId;
                //domainObj.DepreciationStartDate = model.DepreciationStartDate;
                domainObj.UsefulLife = model.UsefulLife;
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.AdditionFormId > 0 ? DateTime.Today : DateTime.Today;

                 await _repo.AddUpdateAdditionAsync(domainObj);
                return new AdditionFormRegRespObj
                {
                    AdditionFormId = domainObj.AdditionFormId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Gone for approval" } }
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
            var response = true;
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
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                new DeleteRespObjt
                {
                    Deleted = true,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
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

                var isDone = await _repo.UploadAdditionAsync(byteList, createdBy);
                return new AdditionFormRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
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

                var subGlResponse = await _financeRequest.GetAllSubGlAsync();
                var classificationName = _dataContext.ppe_assetclassification.Where(a => a.Deleted == false).ToList();
                return Ok(new AdditionFormRespObj
                {

                    AdditionForms = additions.Select(d => new AdditionFormObj

                    {
                        AdditionFormId = d.AdditionFormId,
                        Active = true,
                        AssetClassificationId = d.AssetClassificationId,
                        ClassificationName = classificationName.FirstOrDefault(a => a.AsetClassificationId == d.AssetClassificationId)?.ClassificationName ?? null,
                        Cost = d.Cost,
                        DateOfPurchase = d.DateOfPurchase,
                        //DepreciationStartDate = d.DepreciationStartDate,
                        Description = d.Description,
                        Quantity = d.Quantity,
                        Location = d.Location,
                        LpoNumber = d.LpoNumber,
                        ResidualValue = d.ResidualValue,
                        SubGlAddition = d.SubGlAddition,
                        SubGlAdditionName = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlAddition)?.SubGLName,
                        SubGlAdditionCode = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlAddition)?.SubGLCode,
                        SubGlAccumulatedDepreciation = d.SubGlAccumulatedDepreciation,
                        SubGlAccumulatedDepreciationName = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlAccumulatedDepreciation)?.SubGLName,
                        SubGlAccumulatedDepreciationCode = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlAccumulatedDepreciation)?.SubGLCode,
                        SubGlDepreciation = d.SubGlDepreciation,
                        SubGlDepreciationName = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlDepreciation)?.SubGLName,
                        SubGlDepreciationCode = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlDepreciation)?.SubGLCode,
                        SubGlDisposal = d.SubGlDisposal,
                        SubGlDisposalName = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlDisposal)?.SubGLName,
                        SubGlDisposalCode = subGlResponse.subGls.FirstOrDefault(a => a.SubGLId == d.SubGlDisposal)?.SubGLCode,
                        UsefulLife = d.UsefulLife,
                        CreatedBy = d.CreatedBy,
                        CreatedOn = d.CreatedOn,
                        UpdatedBy = d.UpdatedBy,
                        UpdatedOn = d.UpdatedOn
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

        [HttpPost(ApiRoutes.Addition.UPDATE_LPONUMBER)]
        public async Task<ActionResult<LpoRegRespObj>> AddUpdateLpoNumber([FromBody] LpoObj model)
        {
            try
            {
                var lpo = await _repo.GetAllLpoAsync();
                if (model.IsUsed == false)
                {

                    {
                        return new LpoRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "You can't have the same LpoNumber on this table" } }
                        };

                    }
                }
                ppe_lpo item = new ppe_lpo();
                
               
                var domainObj = new ppe_lpo();
                domainObj.LPOId = model.LPOId > 0 ? model.LPOId : 0;
                domainObj.Active = true;
                
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.LpoNumber = model.LpoNumber;
                domainObj.DateOfPurchase = model.DateOfPurchase;
                domainObj.Description = model.Description;
                domainObj.Quantity = model.Quantity;
                domainObj.Cost = model.Cost;
                domainObj.Location = model.Location;
                domainObj.IsUsed = false;
               

                await _repo.AddUpdateLpoNumber(domainObj);
                return new LpoRegRespObj
                {
                    LPOId = domainObj.LPOId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LpoRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Addition.GET_LPONUMBER_BY_ID)]
        public async Task<ActionResult<LpoRespObj>> GetLpoByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new LpoRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "LPO Id is required" } }
                };
            }

            var response = await _repo.GetLpoByIdAsync(search.SearchId);
            var resplist = new List<ppe_lpo> { response };
            return new LpoRespObj
            {
                lpos = _mapper.Map<List<LpoObj>>(resplist),
            };
        }


        [HttpGet(ApiRoutes.Addition.GET_ADDITION_APPROVAL_COMMENTS)]
        public async Task<ActionResult<ApprovalDetailsRespObj>> GetApprovalTrail([FromQuery] ApprovalDetailSearchObj model)
        {
            try
            {
                return new ApprovalDetailsRespObj
                {
                    Details = _repo.GetApprovalTrail(model.TargetId, model.WorkflowToken)
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalDetailsRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

    }
}


 