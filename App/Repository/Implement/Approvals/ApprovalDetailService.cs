using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TREASURY.Contracts.Response;
using TREASURY.Data;
using TREASURY.DomainObjects.Approval;
using Puchase_and_payables.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TREASURY.Repository.Implement.Approvals
{
    public interface IApprovalDetailService
    {
        Task<ApprovalDetailsRespObj> GetApprovalDetailsAsync(int targetId, string workflowToken);
    }
    public class ApprovalDetailService : IApprovalDetailService
    {
        private readonly IIdentityServerRequest _serverRequest;
        private readonly DataContext _dataContext;
        public ApprovalDetailService(IIdentityServerRequest  serverRequest, DataContext dataContext)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;
        }

        public async Task<ApprovalDetailsRespObj> GetApprovalDetailsAsync(int targetId, string workflowToken)
        {
            var list = await _dataContext.cor_approvaldetail.Where(a => a.TargetId == targetId && a.WorkflowToken == workflowToken).ToListAsync(); 
            var StaffResponse = await _serverRequest.GetAllStaff();

            var response = new ApprovalDetailsRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            if (!StaffResponse.IsSuccessStatusCode)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = StaffResponse.ReasonPhrase;
                return response;
            }

            var stringData = await StaffResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApprovalDetailsRespObj>(stringData);

            if (!result.Status.IsSuccessful)
            {
                response.Status = result.Status;
                return response;
            }
            var temp = list;
            var previousStaff = temp.GroupBy(d => d.StaffId).Select(d => d.First()).Where(d => d.StatusId == (int)ApprovalStatus.Approved && d.TargetId == targetId && d.WorkflowToken == workflowToken).ToArray();

            response.AprovalDetails = list.Select(x => new AprovalDetailsObj()
            {
                ApprovalDetailId = x.ApprovalDetailId,
                Comment = x.Comment,
                Date = x.Date,
                StaffId = x.StaffId,
                FirstName = result.staff.FirstOrDefault(d => d.StaffId == x.StaffId)?.FirstName,
                LastName = result.staff.FirstOrDefault(d => d.StaffId == x.StaffId)?.LastName,
                StatusId = x.StatusId,
                StatusName = Convert.ToString((ApprovalStatus)x.StatusId), 
                TargetId = x.TargetId, 
                WorkflowToken = x.WorkflowToken,
            }).ToList();
            response.PreviousStaff = previousStaff.Select(p => new PreviousStaff
            {
                StaffId = p.StaffId,
                Name = $"{result.staff.FirstOrDefault(d => d.StaffId == p.StaffId)?.FirstName} {result.staff.FirstOrDefault(d => d.StaffId == p.StaffId)?.LastName}",
            }).ToList();
            return response;
        }
    }
}
