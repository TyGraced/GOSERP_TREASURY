using System;
using System.Collections.Generic;
using System.IO;

namespace Puchase_and_payables.MailHandler
{
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public MemoryStream Attachment { get; set; }
    }
}
