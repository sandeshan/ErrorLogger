using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class User
    {
        [Key]
        [Display(Name = "User ID")]
        public int userID { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "E-Mail ID")]
        public string emailID { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime lastLoginDate { get; set; }

        [Display(Name = "Active Status")]
        public string activeStatus { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public virtual UserType userType { get; set; }

        [Display(Name = "Applications")]
        public virtual ICollection<Application> Applications { get; set; }

    }
}
