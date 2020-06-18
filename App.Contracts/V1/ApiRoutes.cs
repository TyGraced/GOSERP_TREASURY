using System;
using System.Collections.Generic;

namespace PPE.Contracts.V1
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

        public class Addition
        {
            public const string ADD_UPDATE_ADDITION = Base + "/addition/add/update/addition";
            public const string GET_ALL_ADDITION = Base + "/addition/get/all/addition";
            public const string GET_ADDITION_BY_ID = Base + "/addition/get/additionbyid";
            public const string DELETE_ADDITION = Base + "/addition/delete/addition";
            public const string DOWNLOAD_ADDITION = Base + "/addition/download/addition";
            public const string UPLOAD_ADDITION = Base + "/addition/upload/addition";
        }

        public class AssetClassification
        {
            public const string ADD_UPDATE_ASSETCLASSIFICATION = Base + "/assetclassification/add/update/assetclassification";
            public const string GET_ALL_ASSETCLASSIFICATION = Base + "/assetclassification/get/all/assetclassification";
            public const string GET_ASSETCLASSIFICATION_BY_ID = Base + "/assetclassification/get/assetclassificationbyid";
            public const string DELETE_ASSETCLASSIFICATION = Base + "/assetclassification/delete/assetclassification";
            public const string DOWNLOAD_ASSETCLASSIFICATION = Base + "/assetclassification/download/assetclassification";
            public const string UPLOAD_ASSETCLASSIFICATION = Base + "/assetclassification/upload/assetclassification";
        }



    }
}
