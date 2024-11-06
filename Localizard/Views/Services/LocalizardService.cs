using Localizard.Models;
using System.Text.Json;

namespace Localizard.Views.Services
{
    public class LocalizardService
    {
        private readonly HttpClient _httpClient;

        public LocalizardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7118");
        }

        public async Task<Project>GetProjectData(string Name)
        {
            var response = await _httpClient.GetAsync($"/MyEntities?Name={Name}");
            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Project>(json);
            }

            return null;

        }
    }
}
