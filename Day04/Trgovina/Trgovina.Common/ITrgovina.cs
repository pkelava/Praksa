using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trgovina.Common
{
    public interface IRestAllClasses
    {

    }

    public interface IRestKupac : IRestAllClasses
    {
        string Ime { get; set; }
        string Prezime { get; set; }
        string NacinPlacanja { get; set; }
    }

    public interface IRestProizvod : IRestAllClasses
    {
        string NazivProizvoda { get; set; }
        int CijenaProizvoda { get; set; }
    }

    public interface IRestKupovina : IRestAllClasses
    {
        DateTime DatumKupovine { get; set; }
    }
}
