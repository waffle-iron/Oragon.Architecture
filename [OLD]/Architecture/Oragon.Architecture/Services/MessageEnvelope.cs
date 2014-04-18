using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Services
{
    public class MessageEnvelope
    {		
        public Dictionary<string, object> Arguments { get; set; }
        public object ReturnValue { get; set; }
        public Exception Exception { get; set; }
    }
}
