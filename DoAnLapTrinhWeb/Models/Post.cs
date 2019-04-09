using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class Post
    {
        [Key]
        public int Id_Post { get; set; }
        public DateTime DatePost { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Require { get; set; }
        [Required]
        public string Benefit { get; set; }
        public long Salary { get; set; }

        public int Id_Employer { get; set; }
        public int Id_Specialization { get; set; }

        public Post() { }
    }
}