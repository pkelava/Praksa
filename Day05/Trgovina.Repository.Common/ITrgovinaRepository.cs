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
        Task<List<string>> DohvatiSve();
        Task<List<DomainKupac>> SviKupci();
        Task<List<string>> SveKupovineKupca(string kupac);
        Task<List<string>> KupacPoPotrosnji(int potrosnja);
        void NovaKupovina(DomainKupac kupac, int proizvodid);
        void PromijeniCijenu(int proizvodid, int novacijena);
        void UkloniKupca(int kupacid);
    }
}
