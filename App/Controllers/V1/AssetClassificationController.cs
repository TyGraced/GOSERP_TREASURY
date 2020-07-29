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
    public class AssetClassificationController : Controller
    {
        private readonly IAssetClassificationService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        public AssetClassificationController(IAssetClassificationService assetClassificationService, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _mapper = mapper;
            _repo = assetClassificationService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.AssetClassification.GET_ALL_ASSETCLASSIFICATION)]
        public async Task<ActionResult<AssetClassificationRespObj>> GetAllAssetClassificationAsync()
        {
            try
            {
                var response = await _repo.GetAllAssetClassificationAsync();
                return new AssetClassificationRespObj
                {
                    AssetClassifications = _mapper.Map<List<AssetClassificationObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AssetClassificationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.AssetClassification.GET_ASSETCLASSIFICATION_BY_ID)]
        public async Task<ActionResult<AssetClassificationRespObj>> GetAssetClassificationByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new AssetClassificationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "AssetClassification Id is required" } }
                };
            }

            var response = await _repo.GetAssetClassificationByIdAsync(search.SearchId);
            var resplist = new List<ppe_assetclassification> { response };
            return new AssetClassificationRespObj
            {
                AssetClassifications = _mapper.Map<List<AssetClassificationObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.AssetClassification.ADD_UPDATE_ASSETCLASSIFICATION)]
        public async Task<ActionResult<AssetClassificationRegRespObj>> AddUpDateAssetClassification([FromBody] AddUpdateAssetClassificationObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                ppe_assetclassification item = null;
                if (model.AsetClassificationId > 0)
                {
                    item = await _repo.GetAssetClassificationByIdAsync(model.AsetClassificationId);
                    if (item == null)
                        return new AssetClassificationRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }
                var domainObj = new ppe_assetclassification();
                domainObj.AsetClassificationId = model.AsetClassificationId > 0 ? model.AsetClassificationId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.ClassificationName = model.ClassificationName;
                domainObj.UsefulLifeMin = model.UsefulLifeMin;
                domainObj.UsefulLifeMax = model.UsefulLifeMax;
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.Depreciable = model.Depreciable;
                domainObj.DepreciationMethod = model.DepreciationMethod;
                domainObj.SubGlAddition = model.SubGlAddition;
                domainObj.SubGlAccumulatedDepreciation = model.SubGlAccumulatedDepreciation;
                domainObj.SubGlDepreciation = model.SubGlDepreciation;
                domainObj.SubGlDisposal = model.SubGlDisposal;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.AsetClassificationId > 0 ? DateTime.Today : DateTime.Today;

                 await _repo.AddUpdateAssetClassificationAsync(domainObj);
                return new AssetClassificationRegRespObj
                {
                    AsetClassificationId = domainObj.AsetClassificationId,
                    Status = new APIResponseStatus { IsSuccessful = true    , Message = new APIResponseMessage { FriendlyMessage =  "successful"  } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AssetClassificationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.AssetClassification.DELETE_ASSETCLASSIFICATION)]
        public async Task<IActionResult> DeleteAssetClassification([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteAssetClassificationAsync(id);
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

        [HttpGet(ApiRoutes.AssetClassification.DOWNLOAD_ASSETCLASSIFICATION)]
        public async Task<ActionResult<AssetClassificationRespObj>> GenerateExportAssetClassification()
        {
            var response = _repo.GenerateExportAssetClassification();

            return new AssetClassificationRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.AssetClassification.UPLOAD_ASSETCLASSIFICATION)]
        public async Task<ActionResult<AssetClassificationRegRespObj>> UploadAssetClassificationAsync()
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

                var isDone = await _repo.UploadAssetClassificationAsync(byteList, createdBy);
                return new AssetClassificationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AssetClassificationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}
