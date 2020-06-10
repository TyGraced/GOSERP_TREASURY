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
    public class UpdateSupplierDocumentCommandHandler : IRequestHandler<UpdateSupplierDocumentCommand, SupplierDocumentRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        public UpdateSupplierDocumentCommandHandler(ILoggerService loggerService, IIdentityService identityService, IMapper mapper, ISupplierRepository supplierRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _logger = loggerService;
            _supRepo = supplierRepository;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SupplierDocumentRegRespObj> Handle(UpdateSupplierDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.UserDataAsync();

                cor_supplierdocument supDoc = _mapper.Map<cor_supplierdocument>(request);

                supDoc.Deleted = false;
                supDoc.CreatedOn = request.SupplierDocumentId > 0 ? (DateTime?)null : DateTime.Now;
                supDoc.CreatedBy = user.UserName;
                supDoc.UpdatedBy = user.UserName;

                await _supRepo.UpdateSupplierDocumentAsync(supDoc);

                return new SupplierDocumentRegRespObj
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
                _logger.Error($"ErrorID : SupplierDocumentCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new SupplierDocumentRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to delete item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : SupplierDocumentCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }
    }
}
