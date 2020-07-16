using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Trgovina.Model;

namespace Trgovina.Repository
{
    public class TrgovinaRepository
    {
        static public SqlConnection MyConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True");
        

        static public List<string> DohvatiSve()
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

                SqlDataReader SviPodaci = command.ExecuteReader();


                while (SviPodaci.Read())
                {
                    ToPrint.Add(
                        SviPodaci.GetInt32(0) + " " + SviPodaci.GetString(1) + " " + SviPodaci.GetString(2) + " " +
                        SviPodaci.GetString(3) + " " + SviPodaci.GetDateTime(4) + " " + SviPodaci.GetInt32(5) + " " +
                        SviPodaci.GetString(6) + " " + SviPodaci.GetInt32(7)
                    );
                }
                SviPodaci.Close();
            }
            NewConnection.Close();
            return ToPrint;
        }


        static public List<DomainKupac> SviKupci()
        {
            List<DomainKupac> ListaSvihKupaca = new List<DomainKupac>();

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
                        DomainKupac noviKupac = new DomainKupac();
                        noviKupac.kupacid = SviKupci.GetInt32(0);
                        noviKupac.ime = SviKupci.GetString(1);
                        noviKupac.prezime = SviKupci.GetString(2);

                        ListaSvihKupaca.Add(noviKupac);
                    }
                }
                else
                {
                    return null;
                }
                SviKupci.Close();
            }
            NewConnection.Close();
            return ListaSvihKupaca;
        }


        static public List<string> SveKupovineKupca(string kupac)
        {
            string[] imeprezime = kupac.Split(' ');
            List<string> ListaSvihKupovina = new List<string>();

            SqlConnection NewConnection = MyConnection;
            using (NewConnection)
            {
                string formatiranje_naredbe = String.Format(
                    "SELECT ime, prezime, naziv_proizvoda, cijena_proizvoda " +
                    "FROM Kupac INNER JOIN Kupovina ON kupac.kupac_id = kupovina.kupac_id " +
                    "INNER JOIN proizvod ON kupovina.proizvod_id = proizvod.proizvod_id " +
                    "WHERE ime = '{0}' AND prezime = '{1}';",
                    imeprezime[0], imeprezime[1]
                );

                SqlCommand command = new SqlCommand(formatiranje_naredbe, NewConnection);
                NewConnection.Open();

                SqlDataReader SveKupovine = command.ExecuteReader();

                while (SveKupovine.Read())
                {
                    ListaSvihKupovina.Add(
                        SveKupovine.GetString(0) + " " + SveKupovine.GetString(1) + " " +
                        SveKupovine.GetString(2) + " " + SveKupovine.GetInt32(3)
                        );
                }

                SveKupovine.Close();
            }
            NewConnection.Close();
            return ListaSvihKupovina;
        }

        static public List<string> KupacPoPotrosnji(int potrosnja)
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


                while (KupciIznadPraga.Read())
                    {
                        Kupci.Add(
                                KupciIznadPraga.GetString(0) + " " +
                                KupciIznadPraga.GetString(1) + " " +
                                KupciIznadPraga.GetInt32(2)
                            );
                    }

                KupciIznadPraga.Close();
            }
            NewConnection.Close();
            return Kupci;
        }


        static public void NovaKupovina(DomainKupac kupac, int proizvodid)
        {
            SqlConnection NewConnection = MyConnection;
            NewConnection.Open();

            string formatiranjenaredbe = String.Format(
                "INSERT INTO kupac (kupac_id, ime, prezime, nacin_placanja)" +
                "VALUES ( {0}, '{1}', '{2}', '{3}'); ",
                kupac.kupacid, kupac.ime, kupac.prezime, kupac.nacinplacanja
                );

            SqlCommand command = new SqlCommand(formatiranjenaredbe, NewConnection);
            command.ExecuteNonQuery();

            formatiranjenaredbe = String.Format(
               "INSERT INTO kupovina (kupac_id, proizvod_id, datum_kupovine) " +
               "VALUES ( {0}, {1}, GETDATE()); ",
               kupac.kupacid, proizvodid
            );
            command = new SqlCommand(formatiranjenaredbe, NewConnection);
            command.ExecuteNonQuery();
            NewConnection.Close();
        }


        static public void PromijeniCijenu(int proizvodid, int novacijena)
        {
            SqlConnection NewConnection = MyConnection;

            string formatiranjenaredbe = String.Format(
                "UPDATE proizvod SET cijena_proizvoda = {1} WHERE proizvod_id = {0}",
                proizvodid, novacijena
                );

            using (NewConnection)
            {
                NewConnection.Open();
                SqlCommand command = new SqlCommand(formatiranjenaredbe, NewConnection);
                command.ExecuteNonQuery();
            }
            NewConnection.Close();
        }

        static public void UkloniKupca(int kupacid)
        {
            SqlConnection NewConnection = MyConnection;

            string formatiranjenaredbe = String.Format(
                "DELETE FROM kupac WHERE kupac_id = {0}",
                kupacid
            );

            using (NewConnection)
            {
                NewConnection.Open();
                SqlCommand command = new SqlCommand(formatiranjenaredbe, NewConnection);
                command.ExecuteNonQuery();
            }
            NewConnection.Close();
        }
    }
}
