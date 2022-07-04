using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABSENSI.Models
{
    public class UserSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_schedule_id { get; set; }
        public DateTime schedule_start { get; set; }
        public DateTime schedule_end { get; set; }
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User user { get; set; }
        public bool attendance { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public DateTime? inserted_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }
    }
}