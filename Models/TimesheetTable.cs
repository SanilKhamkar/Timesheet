using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace emptime.Models
{
    public class TimesheetTable
    {        
        public string Username { get; set; }        
        public string Punchin { get; set; }
        public string Punchout { get; set; }
    }
}