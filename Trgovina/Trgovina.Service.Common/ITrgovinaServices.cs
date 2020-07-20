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
        Task<List<string>> DohvatiSve();
        Task<List<DomainKupac>> SviKupci();
        Task<List<string>> SveKupovineKupca(string kupac);
        Task<List<string>> KupacPoPotrosnji(int potrosnja);
        Task<bool> NovaKupovina(DomainKupac kupac, int proizvodid);
        Task<bool> PromijeniCijenu(int proizvodid, int novacijena);
        Task<bool> UkloniKupca(int kupacid);
    }
}
