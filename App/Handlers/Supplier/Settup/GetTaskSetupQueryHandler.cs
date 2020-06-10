using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Puchase_and_payables.Contracts.Queries.Supplier;
using Puchase_and_payables.Contracts.Response.Supplier;
using Puchase_and_payables.DomainObjects.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{
   
    public class GetTaskSetupQueryHandler : IRequestHandler<GetTaskSetupQuery, TasksetupRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        public GetTaskSetupQueryHandler(ISupplierRepository supplierRepository)
        {
            _supRepo = supplierRepository;
        }
        public async Task<TasksetupRespObj> Handle(GetTaskSetupQuery request, CancellationToken cancellationToken)
        {
            var item = await _supRepo.GetTaskSetupAsync(request.TasksetupId);

            var respList = new List<TasksetupObj>();
            if(item != null)
            {
                var respItem = new TasksetupObj
                {
                    CompanyId = item.CompanyId,
                    Active = item.Active,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    Percentage = item.Percentage,
                    SubGL = item.SubGL,
                    TaskSetupId = item.TaskSetupId,
                    Type = item.Type,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedOn = item.UpdatedOn 
                };
                respList.Add(respItem);
            }

            return new TasksetupRespObj
            {
                TaskSetups = respList,
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = respList.Count() > 0 ? null : "Search Complete!! No Record Found"
                    }
                }
            };
        }
    }
}
