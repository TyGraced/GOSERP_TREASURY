using AutoMapper;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Queries; 
using GODPAPIs.Contracts.RequestResponse.Supplier;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GOSLibraries.GOS_API_Response;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class GetAllSupplierTopSupplierQueryHandler : IRequestHandler<GetAllSupplierTopSupplierQuery, SupplierTopSupplierRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper;
        public GetAllSupplierTopSupplierQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _mapper = mapper;
            _supRepo = supplierRepository;
        }
        public async Task<SupplierTopSupplierRespObj> Handle(GetAllSupplierTopSupplierQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supRepo.GetAllSupplierTopSupplierAsync();
            return new SupplierTopSupplierRespObj
            {
                TopSuppliers = _mapper.Map<List<SupplierTopSupplierObj>>(supplier),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = supplier == null ? "Search Complete!! No Record Found" : null
                    }
                }
            };
        }
    }
}
