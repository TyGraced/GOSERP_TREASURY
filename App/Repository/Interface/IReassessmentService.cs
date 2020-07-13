using PPE.Contracts.Response;
using PPE.DomainObjects.PPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Interface
{
    public interface IReassessmentService
    {
        Task<bool> AddUpdateReassessmentAsync(ppe_reassessment model);

        Task<bool> DeleteReassessmentAsync(int id);

        Task<ppe_reassessment> GetReassessmentByIdAsync(int id);

        Task<IEnumerable<ppe_reassessment>> GetAllReassessmentAsync();

        Task<bool> UploadReassessmentAsync(byte[] record, string createdBy);

        byte[] GenerateExportReassessment();

        Task<StaffApprovalRegRespObj> ReassessmentStaffApprovals(StaffApprovalObj request);

        Task<IEnumerable<ppe_reassessment>> GetReassessmentAwaitingApprovals(List<int> reassessmentIds, List<string> tokens);
    }
}
