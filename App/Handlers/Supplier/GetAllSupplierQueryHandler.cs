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
    public class GetAllSupplierQueryHandler : IRequestHandler<GetAllSupplierQuery, SupplierRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper;
        public GetAllSupplierQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _mapper = mapper;
            _supRepo = supplierRepository;
        }
        public async Task<SupplierRespObj> Handle(GetAllSupplierQuery request, CancellationToken cancellationToken)
        {

            var supplierList = await _supRepo.GetAllSupplierAsync();
            return new SupplierRespObj
            {
                Suppliers = _mapper.Map<List<SupplierObj>>(supplierList),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = supplierList.Count() == 0 ? "Search Complete!! No Record Found": null
                    }
                }
            };
        }
    }
}
