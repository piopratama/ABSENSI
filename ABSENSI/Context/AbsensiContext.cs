using ABSENSI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ABSENSI.Context
{
  public class AbsensiContext : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<UserSchedule> UserSchedules { get; set; }
  }
}