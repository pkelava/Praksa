using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Model;
using Trgovina.Model.Common;

namespace Trgovina.Repository.Common
{
    public interface ITrgovinaRepository
    {
        List<string> DohvatiSve();
        List<DomainKupac> SviKupci();
        List<string> SveKupovineKupca(string kupac);
        List<string> KupacPoPotrosnji(int potrosnja);
        void NovaKupovina(DomainKupac kupac, int proizvodid);
        void PromijeniCijenu(int proizvodid, int novacijena);
        void UkloniKupca(int kupacid);
    }
}
