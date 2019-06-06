using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Films
{
   public class Client
    {
        public Client()
        {
          
        }

        public int CId { get; set; }
        public string CFio { get; set; }
        public string CCardNumber { get; set; }
        public int? CCvv { get; set; }
        public string CLogin { get; set; }
        public string CPassword { get; set; }
        public string CDate { get; set; }
        
    }
}
