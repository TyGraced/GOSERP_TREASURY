using GODP.APIsContinuation.DomainObjects.Supplier;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Commands.Supplier;
using GODPAPIs.Contracts.RequestResponse.Supplier; 
using MediatR; 
using Polly;
using Polly.Retry;
using System; 
using System.Net.Http; 
using System.Threading;
using System.Threading.Tasks;
using Puchase_and_payables.Data;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Puchase_and_payables.Helper.Extensions;
using Puchase_and_payables.Contracts.Response.ApprovalRes;
using Puchase_and_payables.AuthHandler;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Puchase_and_payables.Contracts.V1;
using Microsoft.AspNetCore.Http;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierRegRespObj>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISupplierRepository _supplierRepo;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int MaxRetries = 3;
        private readonly IIdentityService _identityService;
        public GoForApprovalRespObj res;
        public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository, ILoggerService loggerService, IHttpContextAccessor httpContextAccessor,
            DataContext dataContext, IHttpClientFactory httpClientFactory, IIdentityService identityService)
        {
            _logger = loggerService;
            _supplierRepo = supplierRepository;
            _dataContext = dataContext;
            _identityService = identityService;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _retryPolicy = Policy.Handle<HttpRequestException>()
               .WaitAndRetryAsync(MaxRetries, times =>
               TimeSpan.FromMilliseconds(times * 100));
        }
        public async Task<SupplierRegRespObj> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                if (!user.Status.IsSuccessful)
                {
                    return new SupplierRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = user.Status.Message.FriendlyMessage, TechnicalMessage = user.Status.Message.TechnicalMessage }
                        }
                    };
                }
                if (request.SupplierId > 0)
                {
                    var existingSupplier = await BuildExistingSupplierObj(request);
                    await _supplierRepo.UpdateSupplierAsync(existingSupplier);
                }
                else
                {
                    GoForApprovalRequest wfRequest = null;
                    var newSupplier = BuildNewSupplierObj(request);
                    using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var supplierResp = await _supplierRepo.AddNewSupplierAsync(newSupplier);
                            var client = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                            string authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                            client.DefaultRequestHeaders.Add("Authorization", authorization);
                            wfRequest = new GoForApprovalRequest
                            {
                                Comment = "Supplier Registration",
                                OperationId = (int)OperationsEnum.SupplierRegistrationApproval,
                                TargetId = newSupplier.SupplierId,
                                ApprovalStatus = (int)ApprovalStatus.Processing,
                                DeferredExecution = true,
                                StaffId = user.StaffId,
                                CompanyId = 1,
                                EmailNotification = false,
                                ExternalInitialization = false,
                                StatusId = (int)ApprovalStatus.Processing,


                            };
                            var jsonContent = JsonConvert.SerializeObject(wfRequest);
                            var buffer = Encoding.UTF8.GetBytes(jsonContent);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            if (supplierResp)
                            {
                                await _retryPolicy.ExecuteAsync(async () =>
                                {
                                    var result = await client.PostAsync(ApiRoutes.Workflow.APPROVAL, byteContent);
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
                                    var accountInfo = await result.Content.ReadAsStringAsync();
                                    res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(accountInfo);
                                });
                            }
                           

                            if (res.ApprovalProcessStarted)
                            {
                              await _transaction.CommitAsync();
                                return new SupplierRegRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = res.Status.IsSuccessful,
                                        Message = res.Status.Message
                                    }
                                };
                            }

                            if (res.EnableWorkflow || !res.HasWorkflowAccess)
                            {
                                await _transaction.RollbackAsync();
                                return new SupplierRegRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = res.Status.IsSuccessful,
                                        Message = res.Status.Message
                                    }
                                };
                            }

                           
                        }
                        catch (Exception ex)
                        {
                            await _transaction.RollbackAsync();
                            #region Log error to file 
                            var errorCode = ErrorID.Generate(4);
                            _logger.Error($"ErrorID : SupplierInformationHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                            return new SupplierRegRespObj
                            {

                                Status = new APIResponseStatus
                                {
                                    Message = new APIResponseMessage
                                    {
                                        FriendlyMessage = "Error occured!! Please tyr again later",
                                        MessageId = errorCode,
                                        TechnicalMessage = $"ErrorID : SupplierInformationHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                                    }
                                }
                            };
                            #endregion 
                        }
                        finally { await _transaction.DisposeAsync(); }
                       
                        

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : SupplierInformationHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new SupplierRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : SupplierInformationHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        public async Task<cor_supplier> BuildExistingSupplierObj(UpdateSupplierCommand request)
        {
            var supplier = await _supplierRepo.GetSupplierAsync(request.SupplierId);
            if (supplier != null)
            {
                supplier.Address = request.Address;
                supplier.Name = request.Name;
                supplier.Passport = request.Passport;
                supplier.Email = request.Email;
                supplier.PhoneNo = request.PhoneNo;
                supplier.RegistrationNo = request.RegistrationNo;
                supplier.SupplierTypeId = request.SupplierTypeId;
                supplier.CountryId = request.CountryId;
                supplier.UpdatedBy = request.CreatedBy;
                supplier.UpdatedOn = DateTime.Now;
                supplier.Website = request.Website;
                supplier.PostalAddress = request.PostalAddress;
                supplier.TaxIDorVATID = request.TaxIDorVATID;
                supplier.SupplierNumber = request.SupplierNumber;
            }
            return supplier;
        }

        public cor_supplier BuildNewSupplierObj(UpdateSupplierCommand request)
        {
            return  new cor_supplier
            {
                Name = request.Name,
                Address = request.Address,
                PhoneNo = request.PhoneNo,
                Email = request.Email,
                RegistrationNo = request.RegistrationNo,
                SupplierTypeId = request.SupplierTypeId,
                Passport = request.Passport,
                CountryId = request.CountryId,
                ApprovalStatusId = (int)ApprovalStatus.Pending,
                Active = true,
                Deleted = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = request.CreatedBy,
                UpdatedOn = DateTime.Now,
                Website = request.Website,
                PostalAddress = request.PostalAddress,
                TaxIDorVATID = request.TaxIDorVATID,
                SupplierNumber = SupplierNumber.Generate(10),
                HaveWorkPrintPermit = request.HaveWorkPrintPermit == 1 ? true : false,
            };
        }
    }
}
