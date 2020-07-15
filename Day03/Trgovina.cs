using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using Microsoft.SqlServer.Server;

namespace Trgovina.netapi.Controllers
{
    public class KupovinaController : ApiController
    {
        public SqlConnection MyConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True");


        [HttpGet]
        [Route("api/DohvatiSve")]
        public HttpResponseMessage DohvatiSve()
        {
            List<string> ToPrint = new List<string>();

            SqlConnection NewConnection = MyConnection;

            using (NewConnection)
            {
                SqlCommand command = new SqlCommand(
                    "Select kupovina.kupac_id, ime, prezime, nacin_placanja, datum_kupovine, proizvod.proizvod_id, naziv_proizvoda, cijena_proizvoda " +
                    "FROM kupac INNER JOIN kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                    "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id;",
                    NewConnection
                    );
                NewConnection.Open();

                SqlDataReader Sve = command.ExecuteReader();

                if (Sve.HasRows)
                {
                    while (Sve.Read())
                    {
                        ToPrint.Add(
                            Sve.GetInt32(0) + " " + Sve.GetString(1) + " " + Sve.GetString(2) + " " +
                            Sve.GetString(3) + " " + Sve.GetDateTime(4) + " " + Sve.GetInt32(5) + " " +
                            Sve.GetString(6) + " " + Sve.GetInt32(7)
                        );
                    }
                    Sve.Close();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Nema nista u tablicama");
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, ToPrint);
        }


        [HttpGet]
        [Route("api/SviKupci")]
        public HttpResponseMessage SviKupci()
        {
            List<string> ListaSvihKupaca = new List<string>();

            SqlConnection NewConnection = MyConnection;
           using (NewConnection)
            {
                SqlCommand command = new SqlCommand("SELECT kupac_id, ime, prezime FROM kupac;", NewConnection);
                NewConnection.Open();
                SqlDataReader SviKupci = command.ExecuteReader();

                if (SviKupci.HasRows)
                {
                    
                    while (SviKupci.Read())
                    {
                        ListaSvihKupaca.Add(SviKupci.GetInt32(0) + " " + SviKupci.GetString(1) + " " + SviKupci.GetString(2));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "There are no users!");
                }
                SviKupci.Close();

            }
            return Request.CreateResponse(HttpStatusCode.OK, ListaSvihKupaca);
        }

        [HttpGet]
        [Route("api/SveKupovineKupca")]
        public HttpResponseMessage SveKupovineKupca([FromBody] string kupac)
        {
            string [] ime_i_prezime = kupac.Split(' ');
            List<string> ListaSvihKupovina = new List<string>();

            SqlConnection NewConnection = MyConnection;
            using (NewConnection)
            {
                string formatiranje_naredbe = String.Format(
                    "SELECT ime, prezime, naziv_proizvoda, cijena_proizvoda " +
                    "FROM Kupac INNER JOIN Kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                    "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id " +
                    "WHERE ime = '{0}' AND prezime = '{1}';",
                    ime_i_prezime[0], ime_i_prezime[1]
                );
                
                SqlCommand command = new SqlCommand(formatiranje_naredbe, NewConnection);
                NewConnection.Open();

                SqlDataReader SveKupovine = command.ExecuteReader();

                if (SveKupovine.HasRows)
                {
                    while (SveKupovine.Read())
                    {
                        ListaSvihKupovina.Add(
                            SveKupovine.GetString(0) + " " + SveKupovine.GetString(1) + " " +
                            SveKupovine.GetString(2) + " " + SveKupovine.GetInt32(3)
                            );
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvih kupovina!");
                }
                SveKupovine.Close();
            }
            return Request.CreateResponse(HttpStatusCode.OK, ListaSvihKupovina);
        }


        [HttpGet]
        [Route("api/KupacPoPotrosnji/{potrosnja}")]
        public HttpResponseMessage KupacPoPotrosnji(int potrosnja)
        {
            SqlConnection NewConnection = MyConnection;

            List<string> Kupci = new List<string>();

            string formatiranje_naredbe = String.Format(
                "SELECT ime, prezime, SUM(cijena_proizvoda) " +
                "FROM kupac INNER JOIN kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id " +
                "GROUP BY ime, prezime " +
                "HAVING SUM(cijena_proizvoda) > {0}",
                potrosnja
                );
            
            using (NewConnection)
            {
                SqlCommand command = new SqlCommand(formatiranje_naredbe, NewConnection);
                NewConnection.Open();
                SqlDataReader KupciIznadPraga = command.ExecuteReader();

                if (KupciIznadPraga.HasRows)
                {
                    while (KupciIznadPraga.Read())
                    {
                        Kupci.Add(
                                KupciIznadPraga.GetString(0) + " " + 
                                KupciIznadPraga.GetString(1) + " " + 
                                KupciIznadPraga.GetInt32(2)
                            );
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Nema takvih kupaca!");
                }
                KupciIznadPraga.Close();
            }

            return Request.CreateResponse(HttpStatusCode.OK, Kupci);
        }


        [HttpPost]
        [Route("api/NovaKupovina")]
        public HttpResponseMessage NovaKupovina([FromBody]KupacProizvod kupnja)
        {
            SqlConnection NewConnection = MyConnection;
            NewConnection.Open();

            string formatiranjenaredbe = String.Format(
                "INSERT INTO kupac (kupac_id, ime, prezime)" +
                "VALUES ( {0}, '{1}', '{2}'); ",
                kupnja.kupac_id, kupnja.ime, kupnja.prezime
                );

            SqlCommand command = new SqlCommand(formatiranjenaredbe, NewConnection);
            command.ExecuteNonQuery();


            formatiranjenaredbe = String.Format(
                "INSERT INTO proizvod ( proizvod_id, naziv_proizvoda, cijena_proizvoda) " +
                "VALUES ( {0}, '{1}', {2})",
                kupnja.proizvod_id, kupnja.naziv_proizvoda, kupnja.cijena_proizvoda
            );
            command = new SqlCommand(formatiranjenaredbe, NewConnection);
            command.ExecuteNonQuery();



            formatiranjenaredbe = String.Format(
               "INSERT INTO kupovina (kupac_id, proizvod_id, nacin_placanja, datum_kupovine) " +
               "VALUES ( {0}, {1}, '{2}', GETDATE()); ",
               kupnja.kupac_id, kupnja.proizvod_id, kupnja.nacin_placanja
            );
            command = new SqlCommand(formatiranjenaredbe, NewConnection);
            command.ExecuteNonQuery();



            return Request.CreateResponse(HttpStatusCode.Accepted, "Sve je insertano");
        }

        [HttpPut]
        [Route("api/PromijeniCijenu/{proizvod_id}")]
        public HttpResponseMessage PromijeniCijenu(int proizvod_id, [FromBody]int novacijena)
        {
            SqlConnection NewConnection = MyConnection;

            string formatiranjenaredbe = String.Format(
                "UPDATE proizvod SET cijena_proizvoda = {1} WHERE proizvod_id = {0}",
                proizvod_id, novacijena
                );

            using (NewConnection)
            {
                NewConnection.Open();
                SqlCommand command = new SqlCommand(formatiranjenaredbe, NewConnection);
                command.ExecuteNonQuery();
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, "Cijena promjenjena");
        }

        //U tablicama na stranim ključevima je primjenjen "ON DELETE CASCADE"
        [HttpDelete]
        [Route("api/UkloniKupca/{kupac_id}")]
        public HttpResponseMessage UkloniKupca(int kupac_id)
        {
            SqlConnection NewConnection = MyConnection;

            string formatiranjenaredbe = String.Format(
                "DELETE FROM kupac WHERE kupac_id = {0}",
                kupac_id
            );

            using (NewConnection)
            {
                NewConnection.Open();
                SqlCommand command = new SqlCommand(formatiranjenaredbe, NewConnection);
                command.ExecuteNonQuery();
            }


            return Request.CreateResponse(HttpStatusCode.Accepted, "Kupac je uklonjen.");
        }
    }

    public class KupacProizvod
    {
        public int kupac_id { get; set; }
        public int proizvod_id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string nacin_placanja { get; set; }
        public string naziv_proizvoda { get; set; }
        public int cijena_proizvoda { get; set; }
    }
}

