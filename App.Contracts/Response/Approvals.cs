using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPE.Contracts.Response
{
    public class GoForApprovalRequest
    {
        public int StaffId { get; set; }
        public int CompanyId { get; set; }
        public int StatusId { get; set; }
        public int TargetId { get; set; }
        public string Comment { get; set; }
        public int OperationId { get; set; }
        public bool DeferredExecution { get; set; }
        public int WorkflowId { get; set; }
        public bool ExternalInitialization { get; set; }
        public bool EmailNotification { get; set; }
        public int WorkflowTaskId { get; set; }
        public int ApprovalStatus { get; set; }
    }

    public class GoForApprovalRespObj
    {
        public int TargetId { get; set; }
        public bool HasWorkflowAccess { get; set; }
        public bool EnableWorkflow { get; set; }
        public bool ApprovalProcessStarted { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class IdentityServerApprovalCommand
    {
        public int ApprovalStatus { get; set; }
        public string ApprovalComment { get; set; }
        public int TargetId { get; set; }
        public string WorkflowToken { get; set; }
    }


    public class StaffApprovalObj
    {
        public int ApprovalStatus { get; set; }
        public string ApprovalComment { get; set; }
        public int TargetId { get; set; }
        public int ProposedUsefulLife { get; set; }

    }
    public class StaffApprovalRegRespObj
    {
        public int ResponseId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowTaskRespObj
    {
        public List<WorkflowTaskObj> workflowTasks { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class WorkflowTaskObj
    {
        public int TargetId { get; set; }
        public int OperationId { get; set; }
        public int WorkflowId { get; set; }
        public string StaffEmail { get; set; }
        public string WorkflowToken { get; set; }
    }


}
