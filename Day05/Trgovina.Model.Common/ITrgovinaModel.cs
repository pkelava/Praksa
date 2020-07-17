using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Model.Common
{

    public interface IDomainAllClasses
    {

    }

    public interface IDomainKupac : IDomainAllClasses
    {
        int KupacID { get; set; }
        string Ime { get; set; }
        string Prezime { get; set; }
        string NacinPlacanja { get; set; }
    }

    public interface IDomainProizvod : IDomainAllClasses
    {
        int ProizvodID { get; set; }
        string NazivProizvoda { get; set; }
        int CijenaProizvoda { get; set; }
    }

    public interface IDomainKupovina : IDomainAllClasses 
    {
        DateTime DatumKupovine { get; set; }
    }

}
