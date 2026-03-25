using System.Data;
using System.Reflection.Metadata;
using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        var query = from s in DaneUczelni.Studenci  
                                        where s.Miasto.Equals("Warsaw")
                                        select $" {s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}, {s.Miasto}";
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        var query = DaneUczelni.Studenci
            .Select(stud => $"{stud.Email}");
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var query = DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"{s.Id},{s.Imie},{s.Nazwisko}");
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var query = DaneUczelni.Przedmioty
            .Where(p => p.Kategoria.Equals("Analytics"))
            .Select(p => $"{p.Nazwa},{p.DataStartu}")
            .FirstOrDefault();
        return query != null ? new List<string> { query } : new List<string>{"Brak przedmiotów z kategorii Analytics"};
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var query = DaneUczelni.Zapisy
            .Any(z => z.CzyAktywny == false);
        return new List<string>
        {
            query ? "tak" : "nie"
        };
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var query = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrEmpty(p.Katedra));
        return new List<string>
        {
            query ? "tak" : "nie"
        }; 
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var query = DaneUczelni.Zapisy
            .Count(z => z.CzyAktywny == true);
        return new List<string>
        {
            query.ToString()
        };

    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var query = DaneUczelni.Studenci
            .Distinct()
            .OrderBy(s => s.Miasto)
            .Select(s => $"{s.Miasto}");
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        var query = DaneUczelni.Zapisy
            .OrderBy(z => z.DataZapisu)
            .Select(z => $"{z.DataZapisu},{z.Id},{z.PrzedmiotId}")
            .Take(3);
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var query = DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa},{p.Kategoria}");
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var query = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => $"{s.Imie} {s.Nazwisko} - {z.DataZapisu}"
            );
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        var query = DaneUczelni.Zapisy
            .Join(DaneUczelni.Studenci, z => z.StudentId, s => s.Id, (z, s) => new { s, z })
            .Join(DaneUczelni.Przedmioty, zs => zs.z.PrzedmiotId, p => p.Id,
                (zs, p) => $"{zs.s.Imie},{zs.s.Nazwisko},{p.Nazwa}");
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var query = DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty, z => z.PrzedmiotId, p => p.Id, (z, p) => new { z, p })
            .GroupBy(pz => pz.p.Nazwa)
            .Select(pz => $"{pz.Key},{pz.Count()}");
        return query;

    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var query = DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty, z => z.PrzedmiotId, p => p.Id, (z, p) => new { z, p })
            .Where(zp => zp.z.OcenaKoncowa != null)
            .GroupBy(pz => pz.p.Nazwa)
            .Select(pz => $"{pz.Key},{pz.Average(x => x.z.OcenaKoncowa)}");
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var query = DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, przedmioty) => $"{pr.Imie} {pr.Nazwisko} - {przedmioty.Count()}"
            );
        return query;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var query = DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy, s=> s.Id,z=>z.StudentId,(s,z)=>new { s, z })
            .Where(zp => zp.z.OcenaKoncowa != null)
            .GroupBy(zp=> new {zp.s.Imie,zp.s.Nazwisko}).
            Select(zp => $"{zp.Key},{zp.Max(x=> x.z.OcenaKoncowa)}");
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var query = DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy,s=>s.Id,z=>z.StudentId,(s,z)=>new { s, z })
            .Where(zp => zp.z.CzyAktywny)
            .GroupBy(zp => new {zp.s.Imie,zp.s.Nazwisko})
            .Where(g=>g.Count()>1)
            .Select(g=>$"{g.Key.Imie},{g.Key.Nazwisko}:{g.Count()}");
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var query = DaneUczelni.Przedmioty
            .Join(DaneUczelni.Zapisy,p=>p.Id,z=>z.PrzedmiotId,(p,z)=>new { p, z })
            .Where(pz=>pz.p.DataStartu.Year == 2026 && pz.p.DataStartu.Month == 4)
            .GroupBy(pz=>pz.p.Nazwa)
            .Where(g => g.Sum(x => x.z.OcenaKoncowa != null ? 1 : 0) == 0)
            .Select(pz=> pz.Key);
        return query;
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var query = DaneUczelni.Prowadzacy
            .GroupJoin(DaneUczelni.Przedmioty, pro => pro.Id, prze => prze.ProwadzacyId,
                (pro, prze) => new { pro, prze })
            .SelectMany(
                x => x.prze.DefaultIfEmpty(), 
                (x, p) => new { x.pro, p })
            .GroupJoin(DaneUczelni.Zapisy, xp=> xp.p != null ? xp.p.Id : 0, z=> z.PrzedmiotId, (xp,z)=> new { xp.pro,xp.p, z })
            .Select(xp=>new {xp.pro.Imie, xp.pro.Nazwisko,Avg = xp.z.Where(z=> z.OcenaKoncowa !=null).Select(z=>z.OcenaKoncowa.Value).DefaultIfEmpty().Average()})
            .Select(r=> $"{r.Imie},{r.Nazwisko},{r.Avg}");
        return query;

    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var query = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s.Miasto, z.CzyAktywny }
            )
            .Where(x => x.CzyAktywny) 
            .GroupBy(x => x.Miasto)    
            .Select(g => new { Miasto = g.Key, LiczbaAktywnych = g.Count() })
            .OrderByDescending(x => x.LiczbaAktywnych) 
            .Select(x => $"{x.Miasto} - {x.LiczbaAktywnych}");

        return query;
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
