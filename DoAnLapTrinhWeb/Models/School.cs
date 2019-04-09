using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class School
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Candidate> Candidates { get; set; }

        public School() { }
    }
}