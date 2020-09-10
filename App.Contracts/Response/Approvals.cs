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
        public List<int> TargetId { get; set; }
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
        public string WorkflowToken { get; set; }


    }
    public class StaffApprovalRegRespObj
    {
        public int ResponseId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class StaffObj
    {
        public int staffId { get; set; }
        public string staffCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public int jobTitle { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string gender { get; set; }
    }

    public class StaffRespObj
    {
        public List<StaffObj> staff { get; set; }
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

    public class ApprovalDetailsObj
    {
        public int ApprovalDetailId { get; set; }
        public int StatusId { get; set; }
        public int StaffId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int TargetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SupplierName { get; set; }
        public string StatusName { get; set; }
        public string WorkflowToken { get; set; }
    }
 
    public class ApprovalDetailSearchObj
    {
        public int TargetId { get; set; }
        public string WorkflowToken { get; set; }
    }

    public class AprovalDetailsObj
    {
        public int ApprovalDetailId { get; set; }
        public int StatusId { get; set; }
        public int StaffId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int TargetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SupplierName { get; set; }
        public string StatusName { get; set; }
        public string WorkflowToken { get; set; }
        public DateTime ArrivalDate { get; set; }
    }

    public class PreviousStaff
    {
        public int StaffId { get; set; }
        public string Name { get; set; }
    }
    public class ApprovalDetailsRespObj
    {
        public List<AprovalDetailsObj> AprovalDetails { get; set; }
        public List<PreviousStaff> PreviousStaff { get; set; }
        public List<IndentityServerStaffObj> staff { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class IndentityServerStaffObj
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int JobTitle { get; set; }
        public string Email { get; set; }
    }
}
