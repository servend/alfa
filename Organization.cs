using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alfa
{
    internal class Organization
    {
        public string INN {  get; set; }
        public string Number { get; set; }
        
        public Organization(string inn,string number) {
            INN= inn;
            Number= number;
           
        }

    }
   
}
