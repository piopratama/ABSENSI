using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSENSI.Class
{
    public class CalendarEvent
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool attendance { get; set; }
        public bool is_past { get; set; }
        public string constraint { get; set; }
    }
}