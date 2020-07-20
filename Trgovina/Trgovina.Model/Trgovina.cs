using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Model.Common;


namespace Trgovina.Model
{
    public class DomainProizvod : IDomainProizvod
    {
        static public List<int> IDProizvoda { get; set; } = new List<int>();

        public int ProizvodID { get; set; }
        public string NazivProizvoda { get; set; }
        public int CijenaProizvoda { get; set; }
    }

    public class DomainKupovina : IDomainKupovina
    {
        public DateTime DatumKupovine { get; set; }
    }

    public class DomainKupac : IDomainKupac
    {
        static public List<int> IDKupaca { get; set; } = new List<int>();

        public int KupacID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string NacinPlacanja { get; set; }

    }


}
