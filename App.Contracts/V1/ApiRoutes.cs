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

            public const string GO_FOR_APPROVAL = "/workflow/goThroughApprovalFromCode";
            public const string GET_ALL_STAFF_AWAITING_APPROVALS = "/workflow/get/all/staffAwaitingApprovalsFromCode";
            public const string STAFF_APPROVAL_REQUEST = "/workflow/staff/approvaltask";
            public const string GET_ALL_STAFF = "/admin/get/all/staff";
        }

        public class Addition
        {
            public const string ADD_UPDATE_ADDITION = Base + "/addition/add/update/addition";
            public const string GET_ALL_ADDITION = Base + "/addition/get/all/addition";
            public const string GET_ADDITION_BY_ID = Base + "/addition/get/additionbyid";
            public const string DELETE_ADDITION = Base + "/addition/delete/addition";
            public const string DOWNLOAD_ADDITION = Base + "/addition/download/addition";
            public const string UPLOAD_ADDITION = Base + "/addition/upload/addition";

            public const string ADDITION_STAFF_APPROVAL = Base + "/addition/staff/approval/request";
            public const string ADDITION_STAFF_APPROVAL_AWAITNG = Base + "/addition/get/all/staff/awaiting/approvals";
            public const string GET_AWAITING_APPROVAL_LIST = Base + "/addition/get/awaiting/approval/list";
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

        public class Reassessment
        {
            public const string ADD_UPDATE_REASSESSMENT = Base + "/reassessment/add/update/reassessment";
            public const string GET_ALL_REASSESSMENT = Base + "/reassessment/get/all/reassessment";
            public const string GET_REASSESSMENT_BY_ID = Base + "/reassessment/get/reassessmentbyid";
            public const string DELETE_REASSESSMENT = Base + "/reassessment/delete/reassessment";
            public const string DOWNLOAD_REASSESSMENT = Base + "/reassessment/download/reassessment";
            public const string UPLOAD_REASSESSMENT = Base + "/reassessment/upload/reassessment";
        }

        public class Register
        {
            public const string ADD_UPDATE_REGISTER = Base + "/register/add/update/register";
            public const string GET_ALL_REGISTER = Base + "/register/get/all/register";
            public const string GET_REGISTER_BY_ID = Base + "/register/get/registerbyid";
            public const string DELETE_REGISTER = Base + "/register/delete/register";
            public const string DOWNLOAD_REGISTER = Base + "/register/download/register";
            public const string UPLOAD_REGISTER = Base + "/register/upload/register";

            public const string REGISTER_STAFF_APPROVAL = Base + "/register/staff/approval/request";
            public const string REGISTER_STAFF_APPROVAL_AWAITNG = Base + "/register/get/all/staff/awaiting/approvals";
        }


    }
}
