using Microsoft.AspNetCore.Mvc;
using PPE.Contracts.Response;
using PPE.DomainObjects.PPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Interface
{
    public interface IAdditionService
    {
        Task<AdditionFormRegRespObj> AddUpdateAdditionAsync(ppe_additionform model);

        Task<bool> DeleteAdditionAsync(int id);

        Task<ppe_additionform> GetAdditionByIdAsync(int id);

        Task<IEnumerable<ppe_additionform>> GetAllAdditionAsync();

        Task<bool> UploadAdditionAsync(byte[] record, string createdBy);

        byte[] GenerateExportAddition();
        Task<StaffApprovalRegRespObj> AdditionStaffApprovals(StaffApprovalObj request);
        Task<IEnumerable<ppe_additionform>> GetAdditionAwaitingApprovals(List<int> additonIds, List<string> tokens);
        Task<ActionResult<AdditionFormRespObj>> GetAdditionForAppraisalAsync();
    }
}
