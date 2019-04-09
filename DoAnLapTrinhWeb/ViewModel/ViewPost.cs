using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.ViewModel
{
    public class ViewPost
    {
        public int Id_Post { get; set; }
        public DateTime DatePost { get; set; }
        public string Title { get; set; }
        public string Name_Specialization { get; set; }
        public ViewPost() { }
    }
}