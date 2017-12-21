using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class Application
    {
        [Key]
        [Display(Name = "Application ID")]
        public int appId { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Application Name")]
        public string appName { get; set; }

        [Display(Name = "Application Type")]
        public string appType { get; set; }

        [Display(Name = "Application Debug Level")]
        public virtual DebugLevel debugLevel { get; set; }

        [Display(Name = "Application Status")]
        public string appStatus { get; set; }

        [Display(Name = "Application Users")]
        public virtual ICollection<User> Users { get; set; }

        [Display(Name = "Related Error Logs")]
        public virtual ICollection<ErrorLog> ErrorLogs { get; set; }
    }
}
