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
    public class RegisterController : Controller
    {
        private readonly IRegisterService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        public RegisterController(IRegisterService registerService, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _mapper = mapper;
            _repo = registerService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.Register.GET_ALL_REGISTER)]

        public async Task<ActionResult<RegisterRespObj>> GetAllRegisterAsync()
        {
            try
            {
                var response = await _repo.GetAllRegisterAsync();
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
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Register Id is required" } }
                };
            }

            var response = await _repo.GetRegisterByIdAsync(search.SearchId);
            var resplist = new List<ppe_register> { response };
            return new RegisterRespObj
            {
                Registers = _mapper.Map<List<RegisterObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.Register.ADD_UPDATE_REGISTER)]
        public async Task<ActionResult<RegisterRegRespObj>> AddUpDateRegister([FromBody] AddUpdateRegisterObj model)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                ppe_register item = null;
                if (model.RegisterId > 0)
                {
                    item = await _repo.GetRegisterByIdAsync(model.RegisterId);
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
                domainObj.ResidualValue = model.ResidualValue;
                domainObj.Location = model.Location;
                domainObj.DepreciationForThePeriod = model.DepreciationForThePeriod;
                domainObj.AccumulatedDepreciation = model.AccumulatedDepreciation;
                domainObj.NetBookValue = model.NetBookValue;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.RegisterId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateRegisterAsync(domainObj);
                return new RegisterRegRespObj
                {
                    RegisterId = domainObj.RegisterId,
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

        [HttpPost(ApiRoutes.Register.DELETE_REGISTER)]
        public async Task<IActionResult> DeleteRegister([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteRegisterAsync(id);
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
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityService.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadRegisterAsync(image, createdBy);
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new RegisterRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}
