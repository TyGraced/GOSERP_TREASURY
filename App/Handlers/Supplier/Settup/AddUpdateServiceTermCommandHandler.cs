using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Puchase_and_payables.AuthHandler;
using Puchase_and_payables.Contracts.Commands.Supplier.setup;
using Puchase_and_payables.Contracts.Response.Supplier;
using Puchase_and_payables.DomainObjects.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{ 

    public class AddUpdateServiceTermCommandHandler : IRequestHandler<AddUpdateServiceTermCommand, ServiceTermRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ISupplierRepository _supRepo;
        private readonly IIdentityService _identityService;
        public AddUpdateServiceTermCommandHandler(ILoggerService loggerService, ISupplierRepository supplierRepository, IIdentityService identityService)
        {
            _logger = loggerService;
            _supRepo = supplierRepository;
            _identityService = identityService;
        }
        public async Task<ServiceTermRegRespObj> Handle(AddUpdateServiceTermCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                if(user != null)
                {
                    return new ServiceTermRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Unable to ",
                            }
                        }
                    };
                }
                cor_serviceterms sup = new cor_serviceterms();
                sup.Deleted = true;
                sup.CreatedOn = request.ServiceTermsId > 0 ? (DateTime?)null : DateTime.Now;
                sup.CreatedBy = user.UserName;
                sup.UpdatedBy = user.UserName;
                sup.ServiceTermsId = request.ServiceTermsId > 0 ? request.ServiceTermsId : 0;
                sup.Header = request.Header;
                sup.Content = request.Content;
                sup.Deleted = false;
                await _supRepo.AddUpdateSeviceTermAsync(sup);
                var actionTaken = request.ServiceTermsId < 1 ? "created" : "updated";
                return new ServiceTermRegRespObj
                {
                    ServiceTermId = sup.ServiceTermsId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Successfully  {actionTaken}",
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ServiceTermRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to delete item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
