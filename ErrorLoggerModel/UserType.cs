using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class UserType
    {
        [Key]
        public int userTypeID { get; set; }

        [Required, MaxLength(10)]
        public string userType { get; set; }

    }
}