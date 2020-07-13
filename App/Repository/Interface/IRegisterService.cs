using PPE.Contracts.Response;
using PPE.DomainObjects.PPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Interface
{
    public interface IRegisterService
    {
        Task<bool> AddUpdateRegisterAsync(ppe_register model);

        Task<bool> DeleteRegisterAsync(int id);

        Task<ppe_register> GetRegisterByIdAsync(int id);

        Task<IEnumerable<ppe_register>> GetAllRegisterAsync();

        byte[] GenerateExportRegister();

        Task<StaffApprovalRegRespObj> RegisterStaffApprovals(StaffApprovalObj request);

        Task<IEnumerable<ppe_register>> GetRegisterAwaitingApprovals(List<int> registerIds, List<string> tokens);
    }
}
