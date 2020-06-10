using AutoMapper;
using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Puchase_and_payables.Contracts.Queries.Supplier;
using Puchase_and_payables.Contracts.Response.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puchase_and_payables.Handlers.Supplier.Settup
{
    

    public class GetAllTaskSetupQueryHandler : IRequestHandler<GetAllTaskSetupQuery, TasksetupRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        public GetAllTaskSetupQueryHandler(ISupplierRepository supplierRepository)
        {
            _supRepo = supplierRepository;
        }
        public async Task<TasksetupRespObj> Handle(GetAllTaskSetupQuery request, CancellationToken cancellationToken)
        {
            var list = await _supRepo.GetAllTaskSetupAsync();


            return new TasksetupRespObj
            {
                TaskSetups = list.Select(x => new TasksetupObj()
                {
                    UpdatedOn = x.UpdatedOn,
                    Active = x.Active, 
                    CreatedBy = x.CreatedBy ?? string.Empty,
                    CreatedOn = x.CreatedOn,
                    Deleted = x.Deleted, 
                    TaskSetupId = x.TaskSetupId,
                    UpdatedBy = x.UpdatedBy,
                    CompanyId = x.CompanyId,
                    Percentage = x.Percentage,
                    SubGL = x.SubGL,
                    Type = x.Type
                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = list.Count() > 0 ? null : "Search Complete!! No Record Found"
                    }
                }
            };
        }
    }
}
