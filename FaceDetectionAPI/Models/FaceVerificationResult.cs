namespace FaceDetectionAPI.Models
{
    public class OllamaResponse
    {
        public string model { get; set; }
        public string created_at { get; set; }
        public string response { get; set; }  // The generated response text
        public bool done { get; set; }  // Indicates if the response is complete
    }
}
