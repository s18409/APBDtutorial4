using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutorial4.Models
{
    public class Student
    {
        [Required(ErrorMessage ="Please Enter A Valid Name!")]
        [MaxLength(20)]  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string studies { get; set; }
        public int semester { get; set; }
    }
}
