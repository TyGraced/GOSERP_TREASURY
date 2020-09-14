using TREASURY.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TREASURY.Requests
{
    public interface IFinanceServerRequest
    {
        Task<SubGlRespObj> GetAllSubGlAsync();
        Task<FinTransacRegRespObj> PassEntryToFinance(TransactionObj request);
    }
}
