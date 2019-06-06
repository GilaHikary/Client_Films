using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Films
{
   public class Purchase
    {
        public int BId { get; set; }
        public int FId { get; set; }
        public virtual Buy B { get; set; }
        public virtual Film F { get; set; }
    }
}
