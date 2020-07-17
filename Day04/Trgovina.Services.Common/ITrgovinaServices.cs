using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Model;
using Trgovina.Model.Common;

namespace Trgovina.Service.Common
{
    public interface ITrgovinaServices
    {
        List<string> DohvatiSve();
        List<DomainKupac> SviKupci();
        List<string> SveKupovineKupca(string kupac);
        List<string> KupacPoPotrosnji(int potrosnja);
        bool NovaKupovina(DomainKupac kupac, int proizvodid);
        bool PromijeniCijenu(int proizvodid, int novacijena);
        bool UkloniKupca(int kupacid);
    }
}
