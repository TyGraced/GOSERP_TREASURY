using AutoMapper;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.RequestResponse.Supplier;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Puchase_and_payables.Contracts.Queries.Supplier;
using Puchase_and_payables.Contracts.Response.ApprovalRes;
using Puchase_and_payables.Contracts.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Approvals
{
    public class GetAllSupplierDataAwaitingApprovalQueryHandler : IRequestHandler<GetAllSupplierDataAwaitingApprovalQuery, SupplierRespObj>
    {
        private readonly ISupplierRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _accesor;
        private readonly AsyncRetryPolicy<SupplierRespObj> _retryPolicy;
        private const int maxRetryTimes = 4;

        public GetAllSupplierDataAwaitingApprovalQueryHandler(ISupplierRepository supplierRepository, IMapper mapper, 
            IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = supplierRepository;
            _factory = httpClientFactory;
            _accesor = httpContextAccessor;
            _retryPolicy = Policy<SupplierRespObj>.Handle<HttpRequestException>()

                .WaitAndRetryAsync(maxRetryTimes, times =>

                TimeSpan.FromSeconds(times * 2));
        }
        public async Task<SupplierRespObj> Handle(GetAllSupplierDataAwaitingApprovalQuery request, CancellationToken cancellationToken)
        {



            var gosGatewayClient = _factory.CreateClient("GOSDEFAULTGATEWAY");


            string authorization = _accesor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            HttpResponseMessage result = new HttpResponseMessage();
            return await _retryPolicy.ExecuteAsync(async () =>
            {

            try
            {



                result = await gosGatewayClient.GetAsync(ApiRoutes.Workflow.GET_ALL_STAFF_AWAITING_APPROVALS);
                if (!result.IsSuccessStatusCode)
                {
                    new SupplierRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}" }
                        }
                    };
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res.workflowTasks.FirstOrDefault().TargetId < 1)
                {
                    return new SupplierRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Pending Approval"
                            }
                        }
                    };
                }
                var supplier = await _repo.GetSupplierDataAwaitingApprovalAsync(res.workflowTasks.Select(x => x.TargetId).ToList());

                return new SupplierRespObj
                {
                    Suppliers = _mapper.Map<List<SupplierObj>>(supplier),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = supplier == null ? "No supplier detail awaiting approvals" : null
                        }
                    }
                };
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            });
        

          
        }
    }
}
