using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models
{
    public class Termekek
    {
        public int Id { get; set; }
        public string nev { get; set; }
        public int ar { get; set; }
        public string tipus { get; set; }
        public string kep { get; set; }
    }
}