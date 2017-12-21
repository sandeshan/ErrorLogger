using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;
    public class ErrorLog
    {
        [Key]
        [Display(Name = "Log ID")]
        public int logID { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Log File Name")]
        public string fileName { get; set; }

        [Required]
        [Display(Name = "Log File Type")]
        public virtual logType logType { get; set; }
        //public string logType { get; set; }
        
        [Display(Name = "Timestamp")]
        public DateTime timeStamp { get; set; }

        /// <summary>
        /// !!!!! If you do not make this virtual, navigational properties will NOT work
        /// </summary>
        [Display(Name = "Source Application")]
        public virtual Application Application { get; set; }

    }
}
