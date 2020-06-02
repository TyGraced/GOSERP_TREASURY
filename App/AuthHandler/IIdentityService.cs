using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using Puchase_and_payables.Contracts.Response;
using System;
using System.Threading.Tasks;

namespace Puchase_and_payables.AuthHandler
{
    public interface IIdentityService
    {

        Task<AuthenticationResult> LoginAsync(string userName, string password);
        Task<UserDataResponseObj> UserDataAsync();
    }
}
