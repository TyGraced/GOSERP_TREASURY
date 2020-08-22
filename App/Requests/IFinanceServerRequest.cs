using PPE.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Requests
{
    public interface IFinanceServerRequest
    {
        Task<SubGlRespObj> GetAllSubGlAsync();
        Task<FinTransacRegRespObj> PassEntryToFinance(TransactionObj request);
    }
}
