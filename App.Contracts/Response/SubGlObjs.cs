using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPE.Contracts.Response
{
    public class SubGlObj
    {
        public int SubGLId { get; set; }
        public string SubGLCode { get; set; }
        public string SubGLName { get; set; }
        

    }

    public class SubGlRespObj
    {
        public List<SubGlObj> subGls { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    

}

