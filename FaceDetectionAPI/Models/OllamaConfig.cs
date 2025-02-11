namespace FaceDetectionAPI.Models
{
    public class OllamaConfig
    {
        public string BaseUrl { get; set; } = "http://localhost:11434";
        public string Model { get; set; } = "llama3.2:latest"; // Default model
    }
}
