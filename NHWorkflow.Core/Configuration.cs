using System;
using System.Collections.Generic;
using System.Text;

namespace NHWorkflow.Core
{
    public partial class NHConfig
    {
        public bool DisplayFullErrorStack { get; set; }
        public string StaticFilesCacheControl { get; set; }
        public bool UseResponseCompression { get; set; }


    }
        
}
