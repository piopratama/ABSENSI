using ABSENSI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSENSI.ViewModel
{
    public class UserViewModel
    {
        public User user { get; set; }
        public UserSchedule userSchedule { get; set; }
        public List<User> userList { get; set; }
    }
}