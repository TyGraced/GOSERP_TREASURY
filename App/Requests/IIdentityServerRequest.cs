using GOSLibraries.GOS_Financial_Identity;
using PPE.Contracts.Response; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Puchase_and_payables.Requests
{
    public interface IIdentityServerRequest
    {
        Task<AuthenticationResult> IdentityServerLoginAsync(string userName, string password);
        Task<UserDataResponseObj> UserDataAsync();
        Task<HttpResponseMessage> StaffApprovalRequestAsync(IdentityServerApprovalCommand request); 
        Task<HttpResponseMessage> GetAllStaff();
        Task<StaffRespObj> GetAllStaffAsync();
        Task<HttpResponseMessage> GotForApprovalAsync(GoForApprovalRequest  request);
        Task<HttpResponseMessage> GetAnApproverItemsFromIdentityServer();
    }
}
