using MovieApp.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace MovieApp.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Bitte warten Sie, bis der Service vorhanden ist");
            Console.ReadKey();

            #region Anzeigen Aller Movies
            IList<Movie> movies = await GetAllMovies();


            foreach (Movie movie in movies)
            {
                Console.WriteLine($"{movie.Id} {movie.Title} {movie.Description} {movie.Price}");
            }
            Console.WriteLine("Bitte weiter zu Beispiel 'Hinzufügen eines Movies'");
            Console.ReadKey();
            Console.Clear();



            #endregion


            #region Erstellen eines Movies

            InsertMovie(new Movie { Title = "Zurück in die Zukunft", Description = "Film durch Raum und Zeit", Genre = GenreType.Action, Price = 12 });

            #endregion

            #region Updaten eines Movies
            #endregion

            #region Löschen eines Movies
            #endregion
        }

        public static async Task<List<Movie>> GetAllMovies()
        {
            string url = "https://localhost:7032/api/Movie";

            HttpClient client = new HttpClient();
            
            //Client möchte eine Anfrage an den RestFul-Service senden
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            //Client sendet Anfrage ab und bekommt ein HttpResponseMessage Objekt als Antwort 
            HttpResponseMessage response = await client.SendAsync(request);

            //Lese das Ergebnis der Antwort aus
            string jsonResult = await response.Content.ReadAsStringAsync();

            List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(jsonResult);

            if (movies == null)
                throw new Exception();

            return movies;
        }


        //Todo: Schauen, woran der Fehler beim Übertragen liegt. 
        // - Email an Kunden schreiben, wo Bug liegt. 
        public static async Task InsertMovie(Movie movie)
        {
            string url = "https://localhost:7032/api/Movie";
        
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(movie);
            string json1 = System.Text.Json.JsonSerializer.Serialize<Movie>(movie);

            StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, body);
            }
            catch (Exception ex)
            {
            }
        }
    }
}