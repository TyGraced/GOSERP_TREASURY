using AutoMapper;
using GODP.APIsContinuation.DomainObjects.Supplier; 
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Commands.Supplier; 
using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service; 
using Puchase_and_payables.AuthHandler;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class UpdateSupplierAuthorizationCommandHandler : IRequestHandler<UpdateSupplierAuthorizationCommand, SupplierAuthorizationRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        public UpdateSupplierAuthorizationCommandHandler(IIdentityService identityService, ILoggerService loggerService, IMapper mapper, ISupplierRepository supplierRepository,  IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _logger = loggerService;
            _supRepo = supplierRepository;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SupplierAuthorizationRegRespObj> Handle(UpdateSupplierAuthorizationCommand request, CancellationToken cancellationToken)
        {
			try
			{
                var user = await _identityService.UserDataAsync();

                cor_supplierauthorization supAuth = _mapper.Map<cor_supplierauthorization>(request);
                supAuth.Deleted = false;
                supAuth.CreatedOn = request.SupplierAuthorizationId > 0? (DateTime?)null : DateTime.Now;
                supAuth.CreatedBy = user.UserName;
                supAuth.UpdatedBy = user.UserName;
                await _supRepo.UpdateSupplierAuthorizationAsync(supAuth);
                return new SupplierAuthorizationRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Successfully created",
                        }
                    }
                };
            }
			catch (Exception ex)
			{
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : UpdateSupplierAuthorizationCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new SupplierAuthorizationRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to delete item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : UpdateSupplierAuthorizationCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
