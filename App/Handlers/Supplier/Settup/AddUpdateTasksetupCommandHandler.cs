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
    public class AddUpdateTasksetupCommandHandler : IRequestHandler<AddUpdateTasksetupCommand, TasksetupRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly ISupplierRepository _supRepo;
        private readonly IIdentityService _identityService;
        public AddUpdateTasksetupCommandHandler(ILoggerService loggerService, ISupplierRepository supplierRepository, IIdentityService identityService)
        {
            _logger = loggerService;
            _supRepo = supplierRepository;
            _identityService = identityService;
        }
        public async Task<TasksetupRegRespObj> Handle(AddUpdateTasksetupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.UserDataAsync();
                cor_tasksetup sup = new cor_tasksetup();
                sup.Deleted = true;
                sup.CreatedOn = request.TaskSetupId > 0 ? (DateTime?)null : DateTime.Now;
                sup.CreatedBy = user.UserName;
                sup.UpdatedBy = user.UserName;
                sup.Percentage = request.Percentage;
                sup.SubGL = request.SubGL;
                sup.Deleted = false;
                sup.TaskSetupId = request.TaskSetupId > 0 ? request.TaskSetupId : 0;
                await _supRepo.AddUpdateTaskSetupAsync(sup);
                var actionTaken = request.TaskSetupId < 1 ? "created" : "updated";
                return new TasksetupRegRespObj
                {
                    TaskSetupId = sup.TaskSetupId,
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
                return new TasksetupRegRespObj
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
