using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class Following
    {
        [Key, Column(Order = 0)]
        public int Id_Employer { get; set; }
        [Key, Column(Order = 1)]
        public int Id_Candidate { get; set; }

        public Following() { }
    }
}