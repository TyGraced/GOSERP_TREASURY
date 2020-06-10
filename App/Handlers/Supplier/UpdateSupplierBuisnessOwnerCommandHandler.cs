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
using GOSLibraries.GOS_Error_logger.Service;
using Puchase_and_payables.AuthHandler;
using GOSLibraries.GOS_API_Response;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class UpdateSupplierBuisnessOwnerCommandHandler : IRequestHandler<UpdateSupplierBuisnessOwnerCommand, SupplierBuisnessOwnerRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        public UpdateSupplierBuisnessOwnerCommandHandler(ILoggerService loggerService, IMapper mapper, ISupplierRepository supplierRepository,
           IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _mapper = mapper;
            _logger = loggerService;
            _supRepo = supplierRepository; 
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }
        public async Task<SupplierBuisnessOwnerRegRespObj> Handle(UpdateSupplierBuisnessOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var user = await _identityService.UserDataAsync();

                cor_supplierbusinessowner supBusOwner = _mapper.Map<cor_supplierbusinessowner>(request);
                supBusOwner.Deleted = false;
                supBusOwner.CreatedOn = request.SupplierBusinessOwnerId > 0 ? (DateTime?)null : DateTime.Now;
                supBusOwner.CreatedBy = user.UserName;
                supBusOwner.UpdatedBy = user.UserName;
                await _supRepo.UpdateSupplierBusinessOwnerAsync(supBusOwner);

                return new SupplierBuisnessOwnerRegRespObj
                {
                    Status = new APIResponseStatus{ IsSuccessful = true,
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
                _logger.Error($"ErrorID : SupplierBuisnessOwnerCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new SupplierBuisnessOwnerRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to delete item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : SupplierBuisnessOwnerCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
