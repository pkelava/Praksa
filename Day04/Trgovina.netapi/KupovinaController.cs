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


namespace Trgovina.netapi.Controllers
{
    public class KupovinaController : ApiController
    {


        [HttpGet]
        [Route("api/DohvatiSve")]
        public HttpResponseMessage DohvatiSve()
        {
            TrgovinaService temp = new TrgovinaService();
            List<string> toPrint = temp.DohvatiSve();

            bool uvijet = toPrint.Count != 0;
            return Response(uvijet, toPrint, "Baza Podataka je prazna.");

        }


        [HttpGet]
        [Route("api/SviKupci")]
        public HttpResponseMessage SviKupci()
        {
            TrgovinaService temp = new TrgovinaService();
            List<DomainKupac> domainSviKupci = temp.SviKupci();
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
        public HttpResponseMessage SveKupovineKupca([FromBody] string kupac)
        {
            TrgovinaService temp = new TrgovinaService();
            List<string> listaSvihKupovina = temp.SveKupovineKupca(kupac);

            bool uvijet = listaSvihKupovina.Count != 0;
            return Response(uvijet, listaSvihKupovina, "Nema takvog kupca.");

        }


        [HttpGet]
        [Route("api/KupacPoPotrosnji/{potrosnja}")]
        public HttpResponseMessage KupacPoPotrosnji(int potrosnja)
        {
            TrgovinaService temp = new TrgovinaService();
            List<string> kupci = temp.KupacPoPotrosnji(potrosnja);

            bool uvijet = kupci.Count != 0;
            return Response(uvijet, kupci, "Nema takvih kupaca.");

        }


        [HttpPost]
        [Route("api/NovaKupovina/{proizvodid}")]
        public HttpResponseMessage NovaKupovina(int proizvodID, [FromBody] RestKupac kupnja)
        {

            //Mappiranje kupca
            var config = new MapperConfiguration(mc => mc.CreateMap<RestKupac, DomainKupac>());
            var restToDomainKupac = config.CreateMapper();
            DomainKupac domainKupac = restToDomainKupac.Map<RestKupac, DomainKupac>(kupnja);

            TrgovinaService temp = new TrgovinaService();
            bool uvijet = temp.NovaKupovina(domainKupac, proizvodID);
            return Response(uvijet, "Sve je insertano.", "Nema takvog proizvoda.");

        }

        [HttpPut]
        [Route("api/PromijeniCijenu/{proizvodid}")]
        public HttpResponseMessage PromijeniCijenu(int proizvodID, [FromBody] int novaCijena)
        {
            TrgovinaService temp = new TrgovinaService();
            bool uvijet = temp.PromijeniCijenu(proizvodID, novaCijena);
            return Response(uvijet, "Cijena je promijenjena.", "Nema takvog proizvoda.");

        }

        //U tablicama na stranim ključevima je primjenjen "ON DELETE CASCADE"
        [HttpDelete]
        [Route("api/UkloniKupca/{kupacid}")]
        public HttpResponseMessage UkloniKupca(int kupacID)
        {
            TrgovinaService temp = new TrgovinaService();
            bool uvijet = temp.UkloniKupca(kupacID);
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
