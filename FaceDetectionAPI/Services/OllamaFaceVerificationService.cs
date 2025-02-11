using System.Text.Json;
using System.Text;
using FaceDetectionAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Azure;

namespace FaceDetectionAPI.Services
{
    public class OllamaFaceVerificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OllamaFaceVerificationService> _logger;
        private readonly string _modelName;

        public OllamaFaceVerificationService(HttpClient httpClient, IOptions<OllamaConfig> config, ILogger<OllamaFaceVerificationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _modelName = config.Value.Model; // Fetch model from config
        }


        public async Task<string> VerifyFaceAsync(string faceData)
        {
            var request = new
            {
                model = _modelName, // Use bound model name dynamically
                prompt = $"Verify the face identity based on the following data: {faceData}"
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("/api/generate", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ollama API Error: {response.StatusCode} - {errorMessage}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            StringBuilder fullResponse = new StringBuilder();

            using (var reader = new StringReader(jsonResponse))
            {                
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    var obj = JsonSerializer.Deserialize<OllamaResponse>(line);
                    if (obj != null)
                    {
                        fullResponse.Append(obj.response);
                    }
                }

                //Console.WriteLine(fullResponse.ToString().Trim());
            }
                        


            return fullResponse.ToString().Trim();
        }

    }


}
