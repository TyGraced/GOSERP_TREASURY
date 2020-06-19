using AutoMapper;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPE.AuthHandler;
using PPE.Contracts.Response;
using PPE.Contracts.V1;
using PPE.DomainObjects.PPE;
using PPE.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Controllers.V1
{
    [Authorize]
    public class ReassessmentController : Controller
    {
        private readonly IReassessmentService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        public ReassessmentController(IReassessmentService reassessmentService, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _mapper = mapper;
            _repo = reassessmentService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.Reassessment.GET_ALL_REASSESSMENT)]

        public async Task<ActionResult<ReassessmentRespObj>> GetAllReassessmentAsync()
        {
            try
            {
                var response = await _repo.GetAllReassessmentAsync();
                return new ReassessmentRespObj
                {
                    Reassessments = _mapper.Map<List<ReassessmentObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new ReassessmentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Reassessment.GET_REASSESSMENT_BY_ID)]
        public async Task<ActionResult<ReassessmentRespObj>> GetReassessmentByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new ReassessmentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Reassessment Id is required" } }
                };
            }

            var response = await _repo.GetReassessmentByIdAsync(search.SearchId);
            var resplist = new List<ppe_reassessment> { response };
            return new ReassessmentRespObj
            {
                Reassessments = _mapper.Map<List<ReassessmentObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.Reassessment.ADD_UPDATE_REASSESSMENT)]
        public async Task<ActionResult<ReassessmentRegRespObj>> AddUpDateReassessment([FromBody] AddUpdateReassessmentObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                ppe_reassessment item = null;
                if (model.ReassessmentId > 0)
                {
                    item = await _repo.GetReassessmentByIdAsync(model.ReassessmentId);
                    if (item == null)
                        return new ReassessmentRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new ppe_reassessment();
                domainObj.ReassessmentId = model.ReassessmentId > 0 ? model.ReassessmentId : 0;
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
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.Location = model.Location;
                domainObj.DepreciationForThePeriod = model.DepreciationForThePeriod;
                domainObj.AccumulatedDepreciation = model.AccumulatedDepreciation;
                domainObj.NetBookValue = model.NetBookValue;
                domainObj.RemainingUsefulLife = model.RemainingUsefulLife;
                domainObj.ProposedUsefulLife = model.ProposedUsefulLife;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.ReassessmentId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateReassessmentAsync(domainObj);
                return new ReassessmentRegRespObj
                {
                    ReassessmentId = domainObj.ReassessmentId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReassessmentRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Reassessment.DELETE_REASSESSMENT)]
        public async Task<IActionResult> DeleteReassessment([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteReassessmentAsync(id);
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

        [HttpGet(ApiRoutes.Reassessment.DOWNLOAD_REASSESSMENT)]
        public async Task<ActionResult<ReassessmentRespObj>> GenerateExportReassessment()
        {
            var response = _repo.GenerateExportReassessment();

            return new ReassessmentRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.Reassessment.UPLOAD_REASSESSMENT)]
        public async Task<ActionResult<ReassessmentRegRespObj>> UploadReassessmentAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityService.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadReassessmentAsync(image, createdBy);
                return new ReassessmentRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new ReassessmentRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}
