using System.ComponentModel.DataAnnotations;

namespace dotnetapi.Models
{
    public class Employee 
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string? empname { get; set; }
        [Required]
        public string? empdepartment { get; set; }

        [Range(0,1)]
        public int? isactive { get; set; }

    }
}
