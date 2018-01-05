using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// A 2017.10.hó szakmai (ágazati) informatika érettségi vizsga 
// programozás feladata C#-ban.

namespace Egyszámjáték_201710
{	// egyetlen játékos adatai
	class Játékos
	{
		public string név;
		public List<int> tippek = new List<int>();
	}
	// az egész játékmenet adatai
	class Számjáték
	{
		public List<Játékos> játékosok = new List<Játékos>();
		public Számjáték(string fájlnév)
		{
			var adatok = File.ReadAllLines(fájlnév);
			foreach (var sor in adatok)
			{
				var adatsor = sor.Split(' ');
				var j = new Játékos();
				j.név = adatsor[0];
				var tippsor = adatsor.ToList(); // mert tömbből nem lehet törölni
				tippsor.RemoveAt(0);	// ha a nevet kitöröljük, a maradék...
				j.tippek = tippsor.ConvertAll(int.Parse); // ...a tippsor, de...
				// át kell alakítani 'int'-re. Itt két megoldás is szerepel.
				//	j.tippek = tippsor.Select(int.Parse).ToList();
				játékosok.Add(j);
			}
		}
		public int fordulók_száma
		{
			get { return játékosok[0].tippek.Count; }
		}
		public int játékosok_száma
		{
			get { return játékosok.Count; }
		}
		public bool volt_e_tipp(int forduló, int tipp)
		{
			//	return játékosok.FindAll (j => j.tippek [forduló-1] == tipp).Count == 0 ? false : true;
			// ...de a "régi" módszerrel is lehet:
			return játékosok.Where(j => j.tippek[forduló - 1] == tipp).ToList().Count() == 0 ? false : true;
		}
		public int legnagyobbtipp
		{
			get { return játékosok.Select(j => j.tippek.Max()).Max(); }
		}
		public List<int> forduló_tippjei(int forduló)
		{
			return játékosok.Select(t => t.tippek[forduló - 1]).ToList();
		}
		public string nyertes_tipp(int forduló)
		{
			var nyertes = játékosok.GroupBy(f => f.tippek[forduló - 1])
							 .Where(t => t.Count() == 1)
							 .OrderBy(t => t.Key)
							 .FirstOrDefault();
			var ny_tipp = nyertes != null ? nyertes.Key.ToString() : "";
			var ny_név = nyertes != null ?
					játékosok.Where(j => j.tippek[forduló - 1] == nyertes.Key).First().név : "";
			var elválasztó = nyertes != null ? ";" : "";
			return ny_tipp + elválasztó + ny_név;
		}
		public void listázás()
		{
			foreach (var j in játékosok)
			{
				Console.Write(j.név + ": ");
				foreach (var tipp in j.tippek)
				{
					Console.Write("{0} ", tipp);
				}
				Console.WriteLine();
			}
		}
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Egyszámjáték (2017.10.hó szakmai OKJ)");
			//2. feladat
			var játék = new Számjáték(@"/home/misterx/CSharp/Egyszámjáték_201710/egyszamjatek.txt");
			//játék.listázás();
			Console.WriteLine("3. feladat: Játékosok száma: {0}", játék.játékosok_száma);
			Console.WriteLine("4. feladat: Fordulók száma: {0}", játék.fordulók_száma);
			Console.WriteLine("5. feladat: Az első fordulóban {0}volt egyes tipp!",
				játék.volt_e_tipp(1, 1) ? "" : "nem ");
			Console.WriteLine("6. feladat: A legnagyobb tipp a fordulók során: {0}", játék.legnagyobbtipp);
			Console.Write("7. feladat: Kérem egy forduló számát [1-{0}]: ", játék.fordulók_száma);
			var forduló = int.Parse(Console.ReadLine());
			Console.WriteLine("\tA kért forduló tippjei: {0}", játék.forduló_tippjei(forduló).
				Aggregate("", (s, t) => s += t.ToString() + " "));
			var ny_tipp = játék.nyertes_tipp(forduló);
			Console.WriteLine("8. feladat: {0}", ny_tipp.Length > 0 ?
				"A nyertes tipp a megadott fordulóban: " + ny_tipp.Split(';')[0] :
				"A fordulóban nem volt nyertes tipp.");
			Console.WriteLine("9. feladat: {0}", ny_tipp.Length > 0 ?
				"A megadott forduló nyertese: " + ny_tipp.Split(';')[1] :
				"Nem volt nyertes a megadott fordulóban.");
			// 10. feladat
			if (ny_tipp.Length > 0)
			{
				var szöveg = "Forduló sorszáma: " + forduló +
							 "\nNyertes tipp: " + ny_tipp.Split(';')[0] +
							 "\nNyertes játékos: " + ny_tipp.Split(';')[1];
				File.WriteAllText("nyertes.txt", szöveg);
			}
			//	Console.ReadKey (); // csak VS-ben kell...
		}
	}
}
