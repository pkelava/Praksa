using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Model
{
        public class DomainProizvod
        {
            static public List<int> idProizvoda { get; set; } = new List<int>();

            public int proizvodID { get; set; }
            public string nazivProizvoda { get; set; }
            public string cijenaProizvoda { get; set; }
        }

        public class DomainKupovina
        {
            public DateTime datum_kupovine { get; set; }
        }

        public class DomainKupac
        {
            static public List<int> idKupaca { get; set; } = new List<int>();

            public int kupacid { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string nacinplacanja { get; set; }

    }
}
