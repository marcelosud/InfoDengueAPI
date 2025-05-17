using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfoDengueAPI.Application.Services
{
    public class InfodengueService : IInfodengueService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public InfodengueService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = "https://info.dengue.mat.br/api/alertcity";
        }

        public async Task<JArray> GetEpidemiologicalData(int codigoIBGE, int ewStart, int ewEnd, int eyStart, int eyEnd, string disease)
        {
            var queryString = $"?geocode={codigoIBGE}&disease={disease}&format=json&ew_start={ewStart}&ew_end={ewEnd}&ey_start={eyStart}&ey_end={eyEnd}";

            var url = _baseUrl + queryString;

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao consultar a API INFODENGUE: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JArray>(content);
        }
    }
}
