using MovieSample.SharedLibrary.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieSample.UI.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _client;

        private string _baseUrl = "https://localhost:5001/api/Movie/";

        //_client.BaseAddress->Beispiel-> https://github.com/CodeMazeBlog/httpclient-aspnetcore/blob/post-put-delete-with-httpclient/CompanyEmployees.Client/CompanyEmployees.Client/Services/HttpClientCrudService.cs


        // Seperation of Concern IHttpClientFactory wird beim erstellen des Controllers verwendet 
        public MovieService(HttpClient client)  
        {
            _client = client;
        }

        


        //Achtung ich verwende die Allgemeine Methode SendAsync
        public async Task<List<Movie>> GetAll()
        {
            

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _baseUrl);

            // HttpResponseMessage response = _client.GetAsync(_baseUrl);
            HttpResponseMessage response = await _client.SendAsync(request);
            string jsonText = await response.Content.ReadAsStringAsync();

            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(jsonText);
            return movies;
        }

        // Get -> https://localhost:5001/api/Movie/1
        public async Task<Movie> GetById(int id)
        {
            string extendetURL = _baseUrl + id.ToString();
            HttpResponseMessage response = await _client.GetAsync(extendetURL);
            string jsonText = await response.Content.ReadAsStringAsync();

            Movie movies = JsonConvert.DeserializeObject<Movie>(jsonText);
            return movies;
        }

        // POST -> https://localhost:5001/api/Movie/
        public async Task InsertMovie(Movie movie)
        {
            //Conventieren in JSON-String
            string jsonText = JsonConvert.SerializeObject(movie);
            //Geben das Json in HTTP Body und MediaType ist application/json 
            StringContent body = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_baseUrl, body);
        }

        //PUT https://localhost:5001/api/Movie/1
        public async Task UpdateMovie(Movie movie)
        {
            //extendet UR aufbauen
            string extendetURL = _baseUrl + movie.Id.ToString();
            string jsonText = JsonConvert.SerializeObject(movie);
            StringContent body = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(extendetURL, body);
        }

        // Delete -> https://localhost:5001/api/Movie/1
        public async Task DeleteMovie(int id)
        {
            string url = _baseUrl + id.ToString();
            HttpResponseMessage response = await _client.DeleteAsync(url);
        }
    }
}
