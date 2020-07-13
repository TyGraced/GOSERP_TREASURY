using PPE.Contracts.Response;
using PPE.DomainObjects.PPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Interface
{
    public interface IDisposalService
    {
         Task<bool> AddUpdateDisposalAsync(ppe_disposal model);

         Task<bool> DeleteDisposalAsync(int id);

         Task<ppe_disposal> GetDisposalByIdAsync(int id);

         Task<IEnumerable<ppe_disposal>> GetAllDisposalAsync();

         byte[] GenerateExportDisposal();

         Task<StaffApprovalRegRespObj> DisposalStaffApprovals(StaffApprovalObj request);

         Task<IEnumerable<ppe_disposal>> GetDisposalAwaitingApprovals(List<int> additonIds, List<string> tokens);
    }
}
