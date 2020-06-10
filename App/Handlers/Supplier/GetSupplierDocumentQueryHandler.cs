using AutoMapper;
using GODP.APIsContinuation.Repository.Interface;
using GODPAPIs.Contracts.Queries;
using GODPAPIs.Contracts.RequestResponse.Supplier;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using System.Collections.Generic; 
using System.Threading;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Handlers.Supplier
{
    public class GetSupplierDocumentQueryHandler : IRequestHandler<GetSupplierDocumentQuery, SupplierDocumentRespObj>
    {
        private readonly ISupplierRepository _supRepo;
        private readonly IMapper _mapper;
        public GetSupplierDocumentQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _mapper = mapper;
            _supRepo = supplierRepository;
        }
        public async Task<SupplierDocumentRespObj> Handle(GetSupplierDocumentQuery request, CancellationToken cancellationToken)
        {

            var supplier = await _supRepo.GetSupplierDocumentAsync(request.SupplierDocumentId);
            return new SupplierDocumentRespObj
            {
                SupplierDocument = _mapper.Map<List<SupplierDocumentObj>>(supplier),
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
