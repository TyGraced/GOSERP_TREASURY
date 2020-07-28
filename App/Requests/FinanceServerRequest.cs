using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using PPE.Contracts.Response;
using PPE.Contracts.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PPE.Requests
{
    public class FinanceServerRequest : IFinanceServerRequest
    {
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int maxRetryTimes = 10;
        private readonly IHttpClientFactory _httpClientFactory; 
        private HttpResponseMessage result = new HttpResponseMessage();
        private readonly IHttpContextAccessor _accessor;

        public FinanceServerRequest(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy.Handle<HttpRequestException>()
            .WaitAndRetryAsync(maxRetryTimes, times =>
            TimeSpan.FromSeconds(times * 2));
        }


        public async Task<SubGlRespObj> GetAllSubGlAsync()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
            SubGlRespObj responseObj = new SubGlRespObj();
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.SubGl.GET_ALL_SUBGL);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<SubGlRespObj>(data1);
                        new SubGlRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<SubGlRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new SubGlRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new SubGlRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new SubGlRespObj
                {
                    subGls = responseObj.subGls,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            });
        }
    }
}
