using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class logType
    {
        [Key]
        public int typeID { get; set; }

        [Required, MaxLength(10)]
        public string typeName { get; set; }

    }
}