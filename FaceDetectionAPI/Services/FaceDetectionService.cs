using OpenCvSharp;

namespace FaceDetectionAPI.Services
{
    public class FaceDetectionService
    {
        private readonly CascadeClassifier _faceCascade;

        public FaceDetectionService()
        {
            _faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
        }

        public List<Rect> DetectFaces(string imagePath)
        {
            using var image = Cv2.ImRead(imagePath);
            var grayImage = new Mat();
            Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

            var faces = _faceCascade.DetectMultiScale(grayImage, 1.1, 4);
            return faces.ToList();
        }
    }
}
