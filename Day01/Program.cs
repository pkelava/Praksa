using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ForumLibrary;

namespace main
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IPosts> Clanci_i_Komentari = new List<IPosts>();

            User korisnik1 = new User("petar", "123");
            Console.WriteLine("Korisnik1: " + korisnik1.UserName);
            korisnik1.Log_in("Petar", "123");
            korisnik1.Log_in("petar", "123");
            korisnik1.Log_in("petar", "123");
            korisnik1.Logg_Out();

            Admin moder = new Admin("Igor", "321");
            Console.WriteLine("Admin: " + moder.UserName());
            User korisnik4 = new User("petar", "123");

            Threads clanak = new Threads(korisnik1, "ovo je zanimljivi clanak 1", "*funfact*");
            Threads clanak1 = new Threads(korisnik1, "ovo je kontroverzni clanak 2", "*politika*");
            Threads clanak2 = new Threads(korisnik1, "ovo je smijesni clanak 3", "*šala*");
            Threads clanak3 = new Threads(korisnik1, "Ovo su vijesti", "*nove covid vijesti*");
            Clanci_i_Komentari.Add(clanak);
            Clanci_i_Komentari.Add(clanak1);
            Clanci_i_Komentari.Add(clanak2);
            Clanci_i_Komentari.Add(clanak3);


            Threads.PrintThreads();

            korisnik4 = new User("goran", "456");
            clanak.AddComment(korisnik4, "Prikazivanje");
            clanak.AddComment(korisnik4, "Moći");
            clanak.AddComment(korisnik4, "Interfacea");
            IPosts TempComm = new Comments(korisnik1, "Dodatan primjer");
            Clanci_i_Komentari.Add(TempComm);

            foreach(IPosts temp in Clanci_i_Komentari)
            {
                if(temp is IThreadsPosts clanci)
                {

                    Console.WriteLine("\nČlanci + Komentari: ");
                    Console.WriteLine(clanci.Context);
                    Console.WriteLine(clanci.TimeOfCreation);
                    Console.WriteLine(clanci.PostCreator);
                    clanci.PrintComments();
                    
                }
                else {
                    Console.WriteLine("\nKomentar: ");
                    Console.WriteLine(temp.Context);
                    Console.WriteLine(temp.TimeOfCreation);
                    Console.WriteLine(temp.PostCreator);
                }
            }





            Console.ReadLine();
        }
    }
}
