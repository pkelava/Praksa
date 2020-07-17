using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Repository;
using Trgovina.Model;
using Trgovina.Service.Common;
using Trgovina.Model.Common;


namespace Trgovina.Service 
{
    public class TrgovinaService : ITrgovinaServices
    {
        private readonly Trgovina.Repository.TrgovinaRepository trgovinaRepository = new Trgovina.Repository.TrgovinaRepository();

        #region Methods

        public async Task<List<string>> DohvatiSve()
        {
            return await trgovinaRepository.DohvatiSve();
        }


        public async Task<List<DomainKupac>> SviKupci()
        {
            return await trgovinaRepository.SviKupci();
        }

        public async Task<List<string>> SveKupovineKupca(string kupac)
        {
            return await trgovinaRepository.SveKupovineKupca(kupac);
        }


        public async Task<List<string>> KupacPoPotrosnji(int potrosnja)
        {
            if(DomainKupac.IDKupaca.Count == 0)
            {
                return null;
            }
            else
            {
               return await trgovinaRepository.KupacPoPotrosnji(potrosnja);
            }
        }


        public async Task<bool> NovaKupovina(DomainKupac kupac, int proizvodID)
        {
            bool flag = DomainProizvod.IDProizvoda.Exists(p => p == proizvodID);

            if(flag)
            {
                return false;
            }
            else
            {
                int trenutnibrojkupaca = DomainKupac.IDKupaca.Count;
                //id mog trenutno kupca je za jedan veći od zadnjeg kupca koji je došao ili ako je prvi onda 1
                if (trenutnibrojkupaca > 0)
                {
                    kupac.KupacID = DomainKupac.IDKupaca[trenutnibrojkupaca - 1] + 1;
                }
                else
                {
                    kupac.KupacID = 1;
                }
                DomainKupac.IDKupaca.Add(kupac.KupacID);
                await trgovinaRepository.NovaKupovina(kupac, proizvodID);
                return true;
            }
        }
        
        public async Task<bool> PromijeniCijenu(int proizvodID, int novaCijena)
        {
            bool flag = DomainProizvod.IDProizvoda.Exists(p => p == proizvodID);

            if(flag)
            {
                return false;
            }
            else
            {
                await trgovinaRepository.PromijeniCijenu(proizvodID, novaCijena);
                return true;
            }
        }


        public async Task<bool> UkloniKupca(int kupacID)
        {
            bool flag = DomainKupac.IDKupaca.Exists(p => p == kupacID);

            if(!flag)
            {
                return false;
            }
            else
            {
                int toremove = DomainKupac.IDKupaca.Find(p => p == kupacID);
                DomainKupac.IDKupaca.Remove(toremove);
                await trgovinaRepository.UkloniKupca(kupacID);
                return true;
            }
        }
        #endregion Methods

    }




}


