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
            public const string LOGIN = Base + "/identity/login";
            public const string REGISTER = Base + "/identity/register";
            public const string REFRESHTOKEN = Base + "/identity/refresh";
            public const string CHANGE_PASSWORD = Base + "/identity/changePassword";
            public const string CONFIRM_EMAIL = Base + "/identity/confirmEmail";
            public const string FETCH_USERDETAILS = Base + "/identity/profile";
            public const string CONFIRM_CODE = Base + "/identity/confirmCode";
        }

        public static class SupplierEndpoints
        {
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

            public const string CHECK = Base + "/supplier/delete/topClient/check";
        }

    }
}
