using System;
using System.Collections.Generic;

namespace Puchase_and_payables.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;



        public static class Identity
        {
            public const string LOGIN = "/identity/login";
            public const string FETCH_USERDETAILS = "/identity/profile"; //Base + "/identity/profile"; 
        }

        public static  class Workflow
        {
            public const string APPROVAL = Base + "/workflow/goThroughApproval";
            public const string GET_ALL_STAFF_AWAITING_APPROVALS = "/workflow/get/all/staffAwaitingApprovals";
        }

        public static class SupplierEndpoints
        {
            public const string ADD_UPDATE_TERMS = Base + "/supplier/add/update/serviceTerm";
            public const string ADD_UPDATE_SUPPLIER_TYPE = Base + "/supplier/add/update/supplierType";
            public const string ADD_UPDATE_TASK_SETUP = Base + "/supplier/add/update/taskSetup";
            public const string GET_ALL_TERMS = Base + "/supplier/get/all/serviceTerm";
            public const string GET_ALL_SUPPLIER_TYPE = Base + "/supplier/get/all/supplierType";
            public const string GET_ALL_TASK_SETUP = Base + "/supplier/get/all/taskSetup";
            public const string GET_TERMS = Base + "/supplier/get/single/serviceTerm/serviceTermId";
            public const string GET_SUPPLIER_TYPE = Base + "/supplier/single/all/supplierType/supplierTypeId";
            public const string GET_TASK_SETUP = Base + "/supplier/get/single/taskSetup/taskSetupId";

            public const string UPDATE_SUPPLIER = Base + "/supplier/add/update/supplier";
            public const string GET_ALL_SUPPLIERS = Base + "/supplier/get/all/supplers";
            public const string GET_SUPPLIER = Base + "/supplier/get/single/supplerId";
            public const string DELETE_SUPPLIER = Base + "/supplier/delete/supplier/targetIds ";

            public const string UPDATE_SUPPLIER_AUTHORIZATION = Base + "/supplier/add/update/authorization";
            public const string GET_ALL_SUPPLIER_AUTHORIZATIONS = Base + "/supplier/get/all/authorizations";
            public const string GET_SUPPLIER_AUTHORIZATION = Base + "/supplier/get/single/authorization/authorizationId";
            public const string DELETE_SUPPLIER_AUTHORIZATION = Base + "/supplier/delete/authorization/targetIds";

            public const string UPDATE_SUPPLIER_BUSINESS_OWNER = Base + "/supplier/add/update/businessOwner";
            public const string GET_ALL_SUPPLIER_BUSINESS_OWNER = Base + "/supplier/get/all/businessOwners";
            public const string GET_SUPPLIER_BUSINESS_OWNER = Base + "/supplier/get/single/businessOwnerId";
            public const string DELETE_SUPPLIER_BUSINESS_OWNER = Base + "/supplier/delete/businessOwner/targetIds";

            public const string UPDATE_SUPPLIER_DOCUMENT = Base + "/supplier/add/update/document";
            public const string GET_ALL_SUPPLIER_DOCUMENTS = Base + "/supplier/get/all/documents";
            public const string GET_SUPPLIER_DOCUMENT = Base + "/supplier/get/single/documentId";
            public const string DELETE_SUPPLIER_DOCUMENT = Base + "/supplier/delete/document/targetIds";

            public const string UPDATE_SUPPLIER_TOP_SUPPLIER = Base + "/supplier/update/topSupplier";
            public const string GET_ALL_TOP_SUPPLIERS = Base + "/supplier/get/all/topSuppliers";
            public const string GET_SUPPLIER_TOP_SUPPLIER = Base + "/supplier/get/single/topSupplierId";
            public const string DELETE_SUPPLIER_TOP_SUPPLIER = Base + "/supplier/delete/topSupplier/targetIds";

            public const string UPDATE_SUPPLIER_TOP_CLIENT = Base + "/supplier/update/topClient";
            public const string GET_ALL_TOP_CLIENTS = Base + "/supplier/get/all/topClients";
            public const string GET_SUPPLIER_TOP_CLIENT = Base + "/supplier/get/single/topClientId";
            public const string DELETE_SUPPLIER_TOP_CLIENT = Base + "/supplier/delete/topClient/targetIds";

            public const string GET_AWAITING_APPROVALS = Base + "/supplier/gel/all/awaitingAprovals";
        }

    }
}
