using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class Specialization
    {
        [Key]
        public int Id_Specialization { get; set; }
        public string Name { get; set; }

        public ICollection<Candidate> Candidates { get; set; }
        public ICollection<Post> Posts { get; set; }

        public Specialization() { }
    }
}