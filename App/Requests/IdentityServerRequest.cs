using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using PPE.Contracts.Response;
using PPE.Contracts.V1;
using Puchase_and_payables.Requests;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PPE.Requests
{
    public class IdentityServerRequest : IIdentityServerRequest
    {
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int MaxRetries = 100;
        private readonly IHttpClientFactory _httpClientFactory; 
        private HttpResponseMessage result = new HttpResponseMessage();
        private readonly IHttpContextAccessor _accessor;
        private static HttpClient Client;
        private AuthenticationResult _authResponse = null;
        private readonly ILoggerService _logger;

        public IdentityServerRequest(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILoggerService loggerService)
        {
            _accessor = httpContextAccessor;
            _logger = loggerService;
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy.Handle<Exception>()
              .WaitAndRetryAsync(MaxRetries, times =>
              TimeSpan.FromMilliseconds(times * 100));
        }

        public async Task<AuthenticationResult> IdentityServerLoginAsync(string userName, string password)
        {
            try
            {

                var loginRquest = new UserLoginReqObj
                {
                    UserName = userName,
                    Password = password,
                };
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");

                var jsonContent = JsonConvert.SerializeObject(loginRquest);
                var buffer = Encoding.UTF8.GetBytes(jsonContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await gosGatewayClient.PostAsync(ApiRoutes.Identity.LOGIN.Trim(), byteContent);

                var accountInfo = await result.Content.ReadAsStringAsync();
                _authResponse = JsonConvert.DeserializeObject<AuthenticationResult>(accountInfo);

                if (_authResponse == null)
                {

                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }

                if (_authResponse.Token != null)
                {

                    return new AuthenticationResult
                    {
                        Token = _authResponse.Token,
                        RefreshToken = _authResponse.RefreshToken
                    };
                }

                return new AuthenticationResult
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = _authResponse.Status.IsSuccessful,
                        Message = new APIResponseMessage
                        {
                            TechnicalMessage = _authResponse.Status?.Message?.TechnicalMessage,
                            FriendlyMessage = _authResponse.Status?.Message?.FriendlyMessage
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<UserDataResponseObj> UserDataAsync()
        {
            try
            {
                var currentUserId = _accessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                string authorization = _accessor.HttpContext.Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authorization))
                {
                    return new UserDataResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Error Occurred ! Please Contact Systems Administrator"
                            }
                        }
                    };
                }
                gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
                var result = await gosGatewayClient.GetAsync(ApiRoutes.Identity.FETCH_USERDETAILS);
                if (!result.IsSuccessStatusCode)
                {
                    var accountInfo1 = await result.Content.ReadAsStringAsync();
                    var dgf = JsonConvert.DeserializeObject<UserDataResponseObj>(accountInfo1);

                    return new UserDataResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "Some thing went Wrong!", TechnicalMessage = $"{result.ReasonPhrase}  {(int)result.StatusCode}  {result.Content}" }
                        }
                    };
                }
                var accountInfo = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDataResponseObj>(accountInfo);
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new UserDataResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<HttpResponseMessage> StaffApprovalRequestAsync(IndentityServerApprovalCommand request)
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(request);

                    var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    result = await gosGatewayClient.PostAsync(ApiRoutes.Workflow.STAFF_APPROVAL_REQUEST, data);

                    if (!result.IsSuccessStatusCode)
                    {
                        new StaffApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }

        public async Task<HttpResponseMessage> GetAllStaff()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.Workflow.GET_ALL_STAFF);
                    if (!result.IsSuccessStatusCode)
                    {
                        new StaffApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }

        public async Task<HttpResponseMessage> GotForApprovalAsync(GoForApprovalRequest request)
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);


            var jsonContent = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.PostAsync(ApiRoutes.Workflow.GO_FOR_APPROVAL, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        new StaffApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }

        public async Task<HttpResponseMessage> GetAnApproverItemsFromIdentityServer()
        {

            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.Workflow.GET_ALL_STAFF_AWAITING_APPROVALS);
                    if (!result.IsSuccessStatusCode)
                    {
                        new StaffApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }
    }
}
