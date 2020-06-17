using System.Threading.Tasks; 
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity; 
using Microsoft.AspNetCore.Mvc;
using PPE.AuthHandler;
using PPE.Contracts.V1;

namespace PPE.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _loggerService;

        public IdentityController(IIdentityService identityService, ILoggerService loggerService)
        {
            _identityService = identityService;
            _loggerService = loggerService;
        } 
        [HttpPost(ApiRoutes.Identity.LOGIN)]
        public async Task<IActionResult> Login([FromBody] UserLoginReqObj request)
        {
            var authResponse = await _identityService.LoginAsync(request.UserName, request.Password);
            if (authResponse.Token == null)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = authResponse.Status.IsSuccessful,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = authResponse?.Status?.Message?.FriendlyMessage,
                            TechnicalMessage = authResponse?.Status?.Message?.TechnicalMessage
                        }
                    }
                });
            } 
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}