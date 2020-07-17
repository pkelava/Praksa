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
using Trgovina.Model.Common;
using System.Threading.Tasks;
using Trgovina.Common;

namespace Trgovina.netapi.Controllers
{
    public class KupovinaController : ApiController
    {

        Trgovina.Service.TrgovinaService trgovinaService = new Trgovina.Service.TrgovinaService();

        
        [HttpGet]
        [Route("api/DohvatiSve")]
        public async Task<HttpResponseMessage> DohvatiSve()
        {
            List<string> toPrint = await trgovinaService.DohvatiSve();

            bool uvijet = toPrint.Count != 0;
            return  Response(uvijet, toPrint, "Baza Podataka je prazna.");

        }


        [HttpGet]
        [Route("api/SviKupci")]
        public async Task<HttpResponseMessage> SviKupci()
        {
            List<DomainKupac> domainSviKupci = await trgovinaService.SviKupci();
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
            List<string> listaSvihKupovina = await trgovinaService.SveKupovineKupca(kupac);

            bool uvijet = listaSvihKupovina.Count != 0;
            return Response(uvijet, listaSvihKupovina, "Nema takvog kupca.");

        }


        [HttpGet]
        [Route("api/KupacPoPotrosnji/{potrosnja}")]
        public async Task<HttpResponseMessage> KupacPoPotrosnji(int potrosnja)
        {
            List<string> kupci = await trgovinaService.KupacPoPotrosnji(potrosnja);

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

            bool uvijet = await trgovinaService.NovaKupovina(domainKupac, proizvodID);
            return Response(uvijet, "Sve je insertano.", "Nema takvog proizvoda.");

        }

        [HttpPut]
        [Route("api/PromijeniCijenu/{proizvodid}")]
        public HttpResponseMessage PromijeniCijenu(int proizvodID, [FromBody] int novaCijena)
        {
            bool uvijet = trgovinaService.PromijeniCijenu(proizvodID,novaCijena);
            return Response(uvijet, "Cijena je promijenjena.", "Nema takvog proizvoda.");

        }

        //U tablicama na stranim ključevima je primjenjen "ON DELETE CASCADE"
        [HttpDelete]
        [Route("api/UkloniKupca/{kupacid}")]
        public HttpResponseMessage UkloniKupca(int kupacID)
        {
            bool uvijet = trgovinaService.UkloniKupca(kupacID);
            return Response(uvijet, "Kupac je uklonjen.", "Nema takvog kupca.");
        }

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
