using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Films
{
   public class Film
    {
        public Film()
        {
        }

        public string FName { get; set; }
        public string FDesc { get; set; }
        public decimal? FImdb { get; set; }
        public int FAge { get; set; }
        public int FId { get; set; }
        public int FPrice { get; set; }

    }
}
