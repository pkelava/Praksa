using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Repository;
using Trgovina.Model;

namespace Trgovina.Service
{
    public class TrgovinaService
    {
        #region Methods

        static public List<string> DohvatiSve()
        {
            return TrgovinaRepository.DohvatiSve();
        }


        static public List<DomainKupac> SviKupci()
        {
            return TrgovinaRepository.SviKupci();
        }

        static public List<string> SveKupovineKupca(string kupac)
        {
            return TrgovinaRepository.SveKupovineKupca(kupac);
        }


        static public List<string> KupacPoPotrosnji(int potrosnja)
        {
            if(DomainKupac.idKupaca.Count == 0)
            {
                return null;
            }
            else
            {
               return TrgovinaRepository.KupacPoPotrosnji(potrosnja);
            }
        }


        static public bool NovaKupovina(DomainKupac kupac, int proizvodid)
        {
            bool flag = DomainProizvod.idProizvoda.Exists(p => p == proizvodid);

            if(flag)
            {
                return false;
            }
            else
            {
                int trenutnibrojkupaca = DomainKupac.idKupaca.Count;
                //id mog trenutno kupca je za jedan veći od zadnjeg kupca koji je došao ili ako je prvi onda 1
                if (trenutnibrojkupaca != 0)
                {
                    kupac.kupacid = DomainKupac.idKupaca[trenutnibrojkupaca - 1] + 1;
                }
                else
                {
                    kupac.kupacid = 1;
                }

                TrgovinaRepository.NovaKupovina(kupac, proizvodid);
                return true;
            }
        }
        
        static public bool PromijeniCijenu(int proizvodid, int novacijena)
        {
            bool flag = DomainProizvod.idProizvoda.Exists(p => p == proizvodid);

            if(flag)
            {
                return false;
            }
            else
            {
                TrgovinaRepository.PromijeniCijenu(proizvodid, novacijena);
                return true;
            }
        }


        static public bool ServiceUkloniKupca(int kupacid)
        {
            bool flag = DomainKupac.idKupaca.Exists(p => p == kupacid);

            if(flag)
            {
                return false;
            }
            else
            {
                int toremove = DomainKupac.idKupaca.Find(p => p == kupacid);
                DomainKupac.idKupaca.Remove(toremove);
                TrgovinaRepository.UkloniKupca(kupacid);
                return true;
            }
        }
        #endregion Methods

    }




}


