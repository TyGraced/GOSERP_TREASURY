﻿using Microsoft.AspNetCore.Mvc;
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
        Task<bool> UploadAdditionAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportAddition();
        Task<StaffApprovalRegRespObj> AdditionStaffApprovals(StaffApprovalObj request);
        Task<IEnumerable<ppe_additionform>> GetAdditionAwaitingApprovals(List<int> additonIds, List<string> tokens);
        Task<bool> AddUpdateLpoNumber(ppe_lpo model);
        Task<IEnumerable<ppe_lpo>> GetAllLpoAsync();
        IEnumerable<ApprovalDetailsObj> GetApprovalTrail(int targetId, string workflowToken);
    }
}
