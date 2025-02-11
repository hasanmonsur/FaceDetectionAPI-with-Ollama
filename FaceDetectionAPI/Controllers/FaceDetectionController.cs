using FaceDetectionAPI.Helpers;
using FaceDetectionAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaceDetectionAPI.Controllers
{
    [ApiController]
    [Route("api/face")]
    public class FaceDetectionController : ControllerBase
    {
        private readonly FaceDetectionService _faceDetectionService;
        private readonly OllamaFaceVerificationService _ollamaService;

        public FaceDetectionController(FaceDetectionService faceDetectionService, OllamaFaceVerificationService ollamaService)
        {
            _faceDetectionService = faceDetectionService;
            _ollamaService = ollamaService;
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectFaces(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var faces = _faceDetectionService.DetectFaces(filePath);

            if (faces.Count == 0)
                return NotFound("No faces detected.");


            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            byte[] fileBytes = await FileHelper.ConvertToByteArrayAsync(file);

            if (fileBytes == null || fileBytes.Length == 0)
                return BadRequest(new { Message = "Invalid image data." });

            //var verificationResult = await _ollamaService.VerifyAsync(fileBytes);


            var verificationResult = await _ollamaService.VerifyFaceAsync($"Detected {faces.Count} face(s).");


            return Ok(new { FacesDetected = faces.Count, AIResponse = verificationResult });
        }

       
    }
}
