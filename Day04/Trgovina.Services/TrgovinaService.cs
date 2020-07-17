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
        #region Methods

        public List<string> DohvatiSve()
        {
            TrgovinaRepository temp = new TrgovinaRepository();
            return temp.DohvatiSve();
        }


        public List<DomainKupac> SviKupci()
        {
            TrgovinaRepository temp = new TrgovinaRepository();
            return temp.SviKupci();
        }

        public List<string> SveKupovineKupca(string kupac)
        {
            TrgovinaRepository temp = new TrgovinaRepository();
            return temp.SveKupovineKupca(kupac);
        }


        public List<string> KupacPoPotrosnji(int potrosnja)
        {
            if(DomainKupac.IDKupaca.Count == 0)
            {
                return null;
            }
            else
            {
                TrgovinaRepository temp = new TrgovinaRepository();
               return temp.KupacPoPotrosnji(potrosnja);
            }
        }


        public bool NovaKupovina(DomainKupac kupac, int proizvodID)
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
                TrgovinaRepository temp = new TrgovinaRepository();
                temp.NovaKupovina(kupac, proizvodID);
                return true;
            }
        }
        
        public bool PromijeniCijenu(int proizvodID, int novaCijena)
        {
            bool flag = DomainProizvod.IDProizvoda.Exists(p => p == proizvodID);

            if(flag)
            {
                return false;
            }
            else
            {
                TrgovinaRepository temp = new TrgovinaRepository();
                temp.PromijeniCijenu(proizvodID, novaCijena);
                return true;
            }
        }


        public bool UkloniKupca(int kupacID)
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
                TrgovinaRepository temp = new TrgovinaRepository();
                temp.UkloniKupca(kupacID);
                return true;
            }
        }
        #endregion Methods

    }




}


