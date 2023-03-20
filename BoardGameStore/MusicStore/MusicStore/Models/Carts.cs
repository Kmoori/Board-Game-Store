using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models
{
    public class Carts
    {
        public int CartId { get; set; }
        public int TermekId { get; set; }
        public int Mennyiseg { get; set; }
    }
}