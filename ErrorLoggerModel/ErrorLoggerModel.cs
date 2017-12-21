using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;
    public class ErrorLogsModel
    {
        [Display(Name = "Log File Name")]
        [Required(ErrorMessage = "FileName is a required field")]
        public string fileName { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime timeStamp { get; set; }

        [Display(Name = "Source Application")]
        public string srcApplication { get; set; }

        [Display(Name = "Log File Type")]
        public string logFileType { get; set; }
    }
}
