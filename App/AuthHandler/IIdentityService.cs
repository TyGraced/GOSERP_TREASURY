using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using PPE.Contracts.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PPE.AuthHandler
{
    public interface IIdentityService
    {

        Task<AuthenticationResult> LoginAsync(string userName, string password);
        Task<UserDataResponseObj> UserDataAsync();

        Task<HttpResponseMessage> GotForApprovalAsync(GoForApprovalRequest request);
    }
}
