using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class Candidate
    {
        [Key]
        public int Id_Candidate { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Phone, Required]
        public string PhoneNumber { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, StringLength(16)]
        public string UserName { get; set; }
        public bool Gender { get; set; }
        [Required, StringLength(16)]
        public string Password { get; set; }
        public string CVLink { get; set; }
        public int YearBorn { get; set; }

        public int Id_School { get; set; }
        public int Id_Specialization { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }
        public ICollection<Following> Followings { get; set; }

        public Candidate() { }
    }
}