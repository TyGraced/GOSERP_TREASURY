using AutoMaTREASURYr;
using TREASURY.Contracts.Response;
using TREASURY.DomainObjects.TREASURY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TREASURY.MapProfile
{
    public class DomainToRequestMapping : Profile
    {
        public DomainToRequestMapping()
        {
            CreateMap<TREASURY_additionform, AdditionFormObj>();
            CreateMap<TREASURY_assetclassification, AssetClassificationObj>();
            CreateMap<TREASURY_reassessment, ReassessmentObj>();
            CreateMap<TREASURY_register, RegisterObj>();
            CreateMap<TREASURY_disposal, DisposalObj>();
        }
    }
}
