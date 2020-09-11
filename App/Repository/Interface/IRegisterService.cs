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
        Task<RegisterRegRespObj> UpdateReassessmentAsync(ppe_register model);
        Task<DisposalsRespObj> UpdateDisposalAsync(ppe_derecognition model);
        Task<RegisterRegRespObj> UpdateReEvaluationAsync(ppe_register model);
        IEnumerable<RegisterObj> GetRegisterByIdAsync(int id);
        IEnumerable<RegisterObj> GetAllRegister();
        Task<bool> UploadRegisterAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportRegister();
        Task<StaffApprovalRegRespObj> ReassessmentStaffApprovals(StaffApprovalObj request);
        Task<StaffApprovalRegRespObj> DisposalStaffApprovals(StaffApprovalObj request);
        Task<StaffApprovalRegRespObj> ReEvaluationStaffApprovals(StaffApprovalObj request);
        Task<IEnumerable<ppe_derecognition>> GetDisposalAwaitingApprovals(List<int> disposalIds, List<string> tokens);
        Task<IEnumerable<ppe_register>> GetReassessmentAwaitingApprovals(List<int> registerIds, List<string> tokens);
        Task<IEnumerable<ppe_register>> GetReEvaluationAwaitingApprovals(List<int> reevaluationIds, List<string> tokens);
        IEnumerable<TransactionObj> GetEndOfMonthDepreciation(DateTime date);
    }
}
