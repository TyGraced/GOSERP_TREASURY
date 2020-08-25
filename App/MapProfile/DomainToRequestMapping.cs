using AutoMapper;
using PPE.Contracts.Response;
using PPE.DomainObjects.PPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.MapProfile
{
    public class DomainToRequestMapping : Profile
    {
        public DomainToRequestMapping()
        {
            CreateMap<ppe_additionform, AdditionFormObj>();
            CreateMap<ppe_assetclassification, AssetClassificationObj>();
            CreateMap<ppe_reassessment, ReassessmentObj>();
            CreateMap<ppe_register, RegisterObj>();
            CreateMap<ppe_disposal, DisposalObj>();
            //CreateMap<,>();
        }
    }
}
