using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using Microsoft.SqlServer.Server;
using Trgovina.Service;
using Trgovina.Model;
using AutoMapper;

namespace Trgovina.netapi.Controllers
{
    public class KupovinaController : ApiController
    {


        [HttpGet]
        [Route("api/DohvatiSve")]
        public HttpResponseMessage DohvatiSve()
        {
            List<string> ToPrint = TrgovinaService.DohvatiSve();
            if(ToPrint.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Baza Podataka je prazna!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, ToPrint);
            }
            
        }


        [HttpGet]
        [Route("api/SviKupci")]
        public HttpResponseMessage SviKupci()
        {
            List<DomainKupac> DomainSviKupci = TrgovinaService.SviKupci();
            List<RestKupac> RestSviKupci = new List<RestKupac>();

            if(DomainSviKupci.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nema kupaca");
            }
            else
            {
                var config = new MapperConfiguration(mc => mc.CreateMap<DomainKupac, RestKupac>());
                var domainToRestKupac = config.CreateMapper();

                foreach(var kupnja in DomainSviKupci)
                {
                    RestKupac restKupac = domainToRestKupac.Map<DomainKupac, RestKupac>(kupnja);
                    RestSviKupci.Add(restKupac);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, RestSviKupci);
        }

        [HttpGet]
        [Route("api/SveKupovineKupca")]
        public HttpResponseMessage SveKupovineKupca([FromBody] string kupac)
        {
            List<string> ListaSvihKupovina = TrgovinaService.SveKupovineKupca(kupac);

            if(ListaSvihKupovina.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvog kupca!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, ListaSvihKupovina);
            }         
        }
      

        [HttpGet]
        [Route("api/KupacPoPotrosnji/{potrosnja}")]
        public HttpResponseMessage KupacPoPotrosnji(int potrosnja)
        {
            List<string> Kupci = TrgovinaService.KupacPoPotrosnji(potrosnja);
            if (Kupci.Count != 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Kupci);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvih kupaca");
            }

            
        }


        [HttpPost]
        [Route("api/NovaKupovina/{proizvodid}")]
        public HttpResponseMessage NovaKupovina(int proizvodid, [FromBody]RestKupac kupnja)
        {

            //Mappiranje kupca
            var config = new MapperConfiguration(mc => mc.CreateMap<RestKupac, DomainKupac>());
            var restToDomainKupac = config.CreateMapper();
            DomainKupac domainKupac = restToDomainKupac.Map<RestKupac, DomainKupac>(kupnja);

            if (TrgovinaService.NovaKupovina(domainKupac, proizvodid))
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Sve je insertano");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvog proizvoda!");
            }

            
        }

        [HttpPut]
        [Route("api/PromijeniCijenu/{proizvodid}")]
        public HttpResponseMessage PromijeniCijenu(int proizvodid, [FromBody]int novacijena)
        {

            if (TrgovinaService.PromijeniCijenu(proizvodid, novacijena))
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Cijena promjenjena");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvog proizvoda");
            }
         
        }

        //U tablicama na stranim ključevima je primjenjen "ON DELETE CASCADE"
        [HttpDelete]
        [Route("api/UkloniKupca/{kupacid}")]
        public HttpResponseMessage UkloniKupca(int kupacid)
        {

            if (TrgovinaService.ServiceUkloniKupca(kupacid))
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Kupac je uklonjen.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvog kupca!");
            }
        }
    }

    public class RestKupac
    {
        public string ime { get; set; }
        public string prezime { get; set; }
        public string nacinplacanja { get; set; }
    }
}
