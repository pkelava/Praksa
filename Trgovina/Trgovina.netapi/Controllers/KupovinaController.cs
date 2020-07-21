using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trgovina.Service;
using Trgovina.Model;
using AutoMapper;
using Trgovina.Model.Common;
using System.Threading.Tasks;
using Trgovina.Common;
using Trgovina.Service.Common;
using Autofac;
using System.Reflection.Emit;
using System.Web.UI.WebControls;

namespace Trgovina.netapi.Controllers
{
    public class KupovinaController : ApiController
    {
        private ITrgovinaServices TrgovinaService { get; set; }

        public KupovinaController(ITrgovinaServices trgService)
        {
            this.TrgovinaService = trgService;
        }

        #region methods

        [HttpGet]
        [Route("api/DohvatiSve")]
        public async Task<HttpResponseMessage> DohvatiSve()
        {
            List<string> toPrint = await TrgovinaService.DohvatiSve();
            
            bool uvijet = toPrint.Count != 0;  
            return Response(uvijet, toPrint, "Baza Podataka je prazna.");
        }


        [HttpGet]
        [Route("api/SviKupci")]
        public async Task<HttpResponseMessage> SviKupci()
        {
            List<DomainKupac> domainSviKupci = await TrgovinaService.SviKupci();
            List<RestKupac> restSviKupci = new List<RestKupac>();      

            var config = new MapperConfiguration(mc => mc.CreateMap<DomainKupac, RestKupac>());
            var domainToRestKupac = config.CreateMapper();


            foreach (var kupnja in domainSviKupci)
            {
                RestKupac restKupac = domainToRestKupac.Map<DomainKupac, RestKupac>(kupnja);
                restSviKupci.Add(restKupac);
            }

            bool uvijet = restSviKupci.Count != 0;
            return Response(uvijet, restSviKupci, "Nema kupaca.");

        }

        [HttpGet]   
        [Route("api/SveKupovineKupca")]
        public async Task<HttpResponseMessage> SveKupovineKupca([FromBody] string kupac)
        {
            List<string> listaSvihKupovina = await TrgovinaService.SveKupovineKupca(kupac);

            bool uvijet = listaSvihKupovina.Count != 0;      
            return Response(uvijet, listaSvihKupovina, "Nema takvog kupca.");

        }


        [HttpGet]
        [Route("api/KupacPoPotrosnji/{potrosnja}")]
        public async Task<HttpResponseMessage> KupacPoPotrosnji(int potrosnja)
        {
            List<string> kupci = await TrgovinaService.KupacPoPotrosnji(potrosnja);

            bool uvijet = kupci.Count != 0;
            return Response(uvijet, kupci, "Nema takvih kupaca.");

        }


        [HttpPost]
        [Route("api/NovaKupovina/{proizvodid}")]
        public async Task<HttpResponseMessage> NovaKupovina(int proizvodID, [FromBody] RestKupac kupnja)
        {
            //Mappiranje kupca
            var config = new MapperConfiguration(mc => mc.CreateMap<RestKupac, DomainKupac>());
            var restToDomainKupac = config.CreateMapper();
            DomainKupac domainKupac = restToDomainKupac.Map<RestKupac, DomainKupac>(kupnja);

            bool uvijet = await TrgovinaService.NovaKupovina(domainKupac, proizvodID);
            
            return Response(uvijet, "Sve je insertano.", "Nema takvog proizvoda.");
        }

        [HttpPut]
        [Route("api/PromijeniCijenu/{proizvodid}")]
        public async Task<HttpResponseMessage> PromijeniCijenu(int proizvodID, [FromBody] int novaCijena)
        {
            bool uvijet = await TrgovinaService.PromijeniCijenu(proizvodID, novaCijena);
            
            return Response(uvijet, "Cijena je promijenjena.", "Nema takvog proizvoda.");
        }

        //U tablicama na stranim ključevima je primjenjen "ON DELETE CASCADE"
        [HttpDelete]
        [Route("api/UkloniKupca/{kupacid}")]
        public async Task<HttpResponseMessage> UkloniKupca(int kupacID)
        {
            bool uvijet = await TrgovinaService.UkloniKupca(kupacID);
            
            return Response(uvijet, "Kupac je uklonjen.", "Nema takvog kupca.");
        }
        #endregion methods
        #region DRY

        public HttpResponseMessage Response(bool uvijet, object goodResponse, string badResponse)
        {
            if (uvijet)
            {
                return Request.CreateResponse(HttpStatusCode.OK, goodResponse);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, badResponse);
            }
        }
        #endregion DRY
    }

    public class RestKupac
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string NacinPlacanja { get; set; }
    }


    public class RestKupovina
    {
        public DateTime DatumKupovine { get; set; }
    }

    public class RestProizvod 
    {
        public string NazivProizvoda { get; set; }
        public int CijenaProizvoda { get; set; }
    }



}
