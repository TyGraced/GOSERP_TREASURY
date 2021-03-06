﻿using System;
using System.Collections.Generic;

namespace TREASURY.Contracts.V1
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

        public static class Finance
        {
            public const string PASS_TO_ENTRY = "/financialtransaction/pass/to/entry";
        }

        public static class SubGl
        {
            public const string GET_ALL_SUBGL = "/subgl/get/all";
            public const string GET_SUBGL_BY_ID = "/subgl/get/single/subgl/id";
        }

        public class Addition
        {
            public const string ADD_UPDATE_ADDITION = Base + "/addition/add/update/addition";
            public const string UPDATE_LPONUMBER = Base + "/addition/update/lponumber";
            public const string GET_ALL_ADDITION = Base + "/addition/get/all/addition";
            public const string GET_ADDITION_BY_ID = Base + "/addition/get/additionbyid";
            public const string GET_ALL_LPONUMBER = Base + "/addition/get/all/lponumbers";
            public const string DELETE_ADDITION = Base + "/addition/delete/addition";
            public const string DOWNLOAD_ADDITION = Base + "/addition/download/addition";
            public const string UPLOAD_ADDITION = Base + "/addition/upload/addition";

            public const string ADDITION_STAFF_APPROVAL = Base + "/addition/staff/approval/request";
            public const string MULTI_ADDITION_STAFF_APPROVAL = Base + "/addition/staff/multiapproval/request";
            public const string ADDITION_STAFF_APPROVAL_AWAITNG = Base + "/addition/get/all/staff/awaiting/approvals";
            public const string GET_ADDITION_APPROVAL_COMMENTS = Base + "/addition/get/approval/comments";
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

        public class Disposal
        {
            public const string ADD_UPDATE_DISPOSAL = Base + "/disposal/add/update/disposal";
            public const string GET_ALL_DISPOSAL = Base + "/disposal/get/all/disposal";
            public const string GET_DISPOSAL_BY_ID = Base + "/disposal/get/disposalbyid";
            public const string DELETE_DISPOSAL = Base + "/disposal/delete/disposal";
            public const string DOWNLOAD_DISPOSAL = Base + "/disposal/download/disposal";

            public const string DISPOSAL_STAFF_APPROVAL = Base + "/disposal/staff/approval/request";
            public const string DISPOSAL_STAFF_APPROVAL_AWAITNG = Base + "/disposal/get/all/staff/awaiting/approvals";
        }
        
        public class Reassessment
        {
            public const string ADD_UPDATE_REASSESSMENT = Base + "/reassessment/add/update/reassessment";
            public const string UPDATE_MULTIPLE_USEFULLIFE = Base + "/reassessment/update/multiple/proposedusefullife";
            public const string UPDATE_MULTIPLE_RESIDUALVALUE = Base + "/reassessment/update/multiple/proposedresidualvalue";
            public const string GET_ALL_REASSESSMENT = Base + "/reassessment/get/all/reassessment";
            public const string GET_REASSESSMENT_BY_ID = Base + "/reassessment/get/reassessmentbyid";
            public const string DELETE_REASSESSMENT = Base + "/reassessment/delete/reassessment";
            public const string DOWNLOAD_REASSESSMENT = Base + "/reassessment/download/reassessment";

            public const string REASSESSMENT_STAFF_APPROVAL = Base + "/reassessment/staff/approval/request";
            public const string REASSESSMENT_STAFF_APPROVAL_AWAITNG = Base + "/reassessment/get/all/staff/awaiting/approvals";
        }

        public class Register
        {
            public const string ADD_UPDATE_REGISTER = Base + "/register/add/update/register";
            public const string UPDATE_REASSESSMENT = Base + "/register/update/reassessment";
            public const string UPDATE_REEVALUATION = Base + "/register/update/reevaluation";
            public const string UPDATE_DISPOSAL = Base + "/register/update/disposal";
            public const string GET_END_OF_MONTH_DEPRECIATION = Base + "/register/get/end/of/month/depreciation";
            public const string UPDATE_MULTIPLE_USEFULLIFE = Base + "/register/update/multiple/proposedusefullife";
            public const string UPDATE_MULTIPLE_RESIDUALVALUE = Base + "/register/update/multiple/proposedresidualvalue";
            public const string UPDATE_MULTIPLE_REEVALUATION = Base + "/register/update/multiple/reevaluation";
            public const string GET_ALL_REGISTER = Base + "/register/get/all/register";
            public const string GET_REGISTER_BY_ID = Base + "/register/get/registerbyid";
            public const string DELETE_REGISTER = Base + "/register/delete/register";
            public const string DOWNLOAD_REGISTER = Base + "/register/download/register";
            public const string UPLOAD_REGISTER = Base + "/register/upload/register";

            public const string REASSESSMENT_STAFF_APPROVAL = Base + "/register/staff/reassessment/approval/request";
            public const string MULTI_REASSESSMENT_STAFF_APPROVAL = Base + "/register/staff/reassessment/multi/approval/request";
            public const string DISPOSAL_STAFF_APPROVAL = Base + "/register/staff/disposal/approval/request";
            public const string MULTI_DISPOSAL_STAFF_APPROVAL = Base + "/register/staff/disposal/multi/approval/request";
            public const string REEVALUATION_STAFF_APPROVAL = Base + "/register/staff/reevaluation/approval/request";
            public const string REASSESSMENT_STAFF_APPROVAL_AWAITNG = Base + "/register/get/all/staff/reassessment/awaiting/approvals";
            public const string DISPOSAL_STAFF_APPROVAL_AWAITNG = Base + "/register/get/all/staff/disposal/awaiting/approvals";
            public const string REEVALUATION_STAFF_APPROVAL_AWAITNG = Base + "/register/get/all/staff/reevaluation/awaiting/approvals";
        }


    }
}
