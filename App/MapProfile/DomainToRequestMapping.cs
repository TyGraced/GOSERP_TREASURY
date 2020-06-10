using AutoMapper;
using GODP.APIsContinuation.DomainObjects.Supplier;
using GODPAPIs.Contracts.RequestResponse.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puchase_and_payables.MapProfile
{
    public class DomainToRequestMapping : Profile
    {
        public DomainToRequestMapping()
        {
            CreateMap<cor_supplier, SupplierObj>();
        }
    }
}
