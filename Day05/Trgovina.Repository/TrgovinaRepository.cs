using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Trgovina.Model;
using Trgovina.Repository.Common;
using Trgovina.Model.Common;

namespace Trgovina.Repository
{
    public class TrgovinaRepository : ITrgovinaRepository
    {
        public SqlConnection myConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True");
        

        public async Task<List<string>> DohvatiSve()
        {
            List<string> toPrint = new List<string>();

            SqlConnection newConnection = myConnection;

            using (newConnection)
            {
                SqlCommand command = new SqlCommand(
                    "Select kupovina.kupac_id, ime, prezime, nacin_placanja, datum_kupovine, proizvod.proizvod_id, naziv_proizvoda, cijena_proizvoda " +
                    "FROM kupac INNER JOIN kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                    "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id;",
                    newConnection
                    );
                newConnection.Open();

                SqlDataReader sviPodaci = await Task.Run(() => command.ExecuteReader());


                while (sviPodaci.Read())
                {
                    toPrint.Add(
                        sviPodaci.GetInt32(0) + " " + sviPodaci.GetString(1) + " " + sviPodaci.GetString(2) + " " +
                        sviPodaci.GetString(3) + " " + sviPodaci.GetDateTime(4) + " " + sviPodaci.GetInt32(5) + " " +
                        sviPodaci.GetString(6) + " " + sviPodaci.GetInt32(7)
                    );
                }
                sviPodaci.Close();
            }
            return toPrint;
        }


        public async Task<List<DomainKupac>> SviKupci()
        {
            List<DomainKupac> listaSvihKupaca = new List<DomainKupac>();

            SqlConnection newConnection = myConnection;
            using (newConnection)
            {
                SqlCommand command = new SqlCommand("SELECT kupac_id, ime, prezime FROM kupac;", newConnection);
                newConnection.Open();
                SqlDataReader sviKupci = await Task.Run(() => command.ExecuteReader());
                DomainKupac noviKupac = new DomainKupac();
                if (sviKupci.HasRows)
                {
                    
                    
                    while (sviKupci.Read())
                    {
                        
                        noviKupac.KupacID = sviKupci.GetInt32(0);
                        noviKupac.Ime = sviKupci.GetString(1);
                        noviKupac.Prezime = sviKupci.GetString(2);

                        listaSvihKupaca.Add(noviKupac);
                    }
                }
                else
                {
                    return listaSvihKupaca;
                }
                sviKupci.Close();
            }
            return listaSvihKupaca;
        }


        public async Task<List<string>> SveKupovineKupca(string kupac)
        {
            string[] imePrezime = kupac.Split(' ');
            List<string> listaSvihKupovina = new List<string>();

            SqlConnection newConnection = myConnection;
            using (newConnection)
            {
                string formatiranje_naredbe = String.Format(
                    "SELECT ime, prezime, naziv_proizvoda, cijena_proizvoda " +
                    "FROM Kupac INNER JOIN Kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                    "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id " +
                    "WHERE ime = '{0}' AND prezime = '{1}';",
                    imePrezime[0], imePrezime[1]
                );

                SqlCommand command = new SqlCommand(formatiranje_naredbe, newConnection);
                newConnection.Open();

                SqlDataReader sveKupovine = await Task.Run(() => command.ExecuteReader());

                while (sveKupovine.Read())
                {
                    listaSvihKupovina.Add(
                        sveKupovine.GetString(0) + " " + sveKupovine.GetString(1) + " " +
                        sveKupovine.GetString(2) + " " + sveKupovine.GetInt32(3)
                        );
                }

                sveKupovine.Close();
            }
            return listaSvihKupovina;
        }

        public async Task<List<string>> KupacPoPotrosnji(int potrosnja)
        {
            SqlConnection newConnection = myConnection;

            List<string> kupci = new List<string>();

            string formatiranje_naredbe = String.Format(
                "SELECT ime, prezime, SUM(cijena_proizvoda) " +
                "FROM kupac INNER JOIN kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id " +
                "GROUP BY ime, prezime " +
                "HAVING SUM(cijena_proizvoda) > {0}",
                potrosnja
                );

            using (newConnection)
            {
                SqlCommand command = new SqlCommand(formatiranje_naredbe, newConnection);
                newConnection.Open();
                SqlDataReader KupciIznadPraga = await Task.Run(() => command.ExecuteReader());


                while (KupciIznadPraga.Read())
                    {
                        kupci.Add(
                                KupciIznadPraga.GetString(0) + " " +
                                KupciIznadPraga.GetString(1) + " " +
                                KupciIznadPraga.GetInt32(2)
                            );
                    }

                KupciIznadPraga.Close();
            }
            return kupci;
        }


        public async void NovaKupovina(DomainKupac kupac, int proizvodID)
        {
            SqlConnection newConnection = myConnection;
            newConnection.Open();

            string formatiranjenaredbe = String.Format(
                "INSERT INTO kupac (kupac_id, ime, prezime, nacin_placanja)" +
                "VALUES ( {0}, '{1}', '{2}', '{3}'); ",
                kupac.KupacID, kupac.Ime, kupac.Prezime, kupac.NacinPlacanja
                );

            SqlCommand command = new SqlCommand(formatiranjenaredbe, newConnection);
            await Task.Run(() => command.ExecuteNonQuery());

            formatiranjenaredbe = String.Format(
               "INSERT INTO kupovina (kupac_id, proizvod_id, datum_kupovine) " +
               "VALUES ( {0}, {1}, GETDATE()); ",
               kupac.KupacID, proizvodID
            );
            command = new SqlCommand(formatiranjenaredbe, newConnection);
            await Task.Run(() => command.ExecuteNonQuery());
        }


        public async void PromijeniCijenu(int proizvodID, int novaCijena)
        {
            SqlConnection newConnection = myConnection;

            string formatiranjenaredbe = String.Format(
                "UPDATE proizvod SET cijena_proizvoda = {1} WHERE proizvod_id = {0}",
                proizvodID, novaCijena
                );

            using (newConnection)
            {
                newConnection.Open();
                SqlCommand command = new SqlCommand(formatiranjenaredbe, newConnection);
                await Task.Run(() => command.ExecuteNonQuery());
            }
        }

        public async void UkloniKupca(int kupacid)
        {
            SqlConnection newConnection = myConnection;

            string formatiranjenaredbe = String.Format(
                "DELETE FROM kupac WHERE kupac_id = {0}",
                kupacid
            );

            using (newConnection)
            {
                newConnection.Open();
                SqlCommand command = new SqlCommand(formatiranjenaredbe, newConnection);
                await Task.Run(() => command.ExecuteNonQuery());
            }
        }
    }
}
