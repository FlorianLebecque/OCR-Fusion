using Google.Cloud.Vision.V1;
using System.Drawing;
using OCR_Fusion.API_Object;
using System.Text.Json.Nodes;

namespace OCR_Fusion {
    public class Utils {

        private static IDBCrud db;

        public static void SetDatabaseInterface(IDBCrud db_) { 
            db = db_;
        }

        public static string GetExtention(string filename) {
            return filename.Split(".")[filename.Split(".").Length - 1];
        }

        public static string[] allowedExtention {
            get {
                string[] ae = { "png", "jpeg", "jpg" };
                return ae;
            }
        }

        public const string uploadPath = "Uploads/";

        public static bool CheckImage(string filename) {
            if (!File.Exists(uploadPath + filename)) {
                throw new Exception("File not found");
            }
            return true;
        }

        public static void CheckUploadDir() {
            if (!Directory.Exists(uploadPath)) {
                Directory.CreateDirectory(uploadPath);
            }

            if (!Directory.Exists(uploadPath+"temps/")) {
                Directory.CreateDirectory(uploadPath+"temps/");
            }
        } 

        public static string GetImagePath(string imageName) {
            return Path.Combine(uploadPath, imageName);
        }

        public static void SaveImage(IFormFile image) {

            using (var fileStream = new FileStream(GetImagePath(image.FileName), FileMode.Create)) {
                image.CopyTo(fileStream);
            }
        }

        public static void Insert<T>(string table, T value) {
            db.Insert<T>(table,value);
        }

        public static void Update<T>(string table, Guid id ,T value) {
            db.Update<T>(table, id, value);
        }

        public static List<T> Gets<T>(string table, string session) {
            return db.Gets<T>(table, session);
        }

        public static void Delete<T>(string table, string session) {
            db.Delete<T>(table, session);
        }
        public static Byte[] CropImageBytes(string filepath, Vector[] cropArea)
        {
            Bitmap bmpImage = new Bitmap(filepath);
            System.Drawing.Image img = System.Drawing.Image.FromFile(filepath);
            int x1 = (int)(cropArea[0].x * img.Width);
            int y1 = (int)(cropArea[0].y * img.Height);
            int x2 = (int)(cropArea[1].x * img.Width);
            int y2 = (int)(cropArea[1].y * img.Height);
            int inputWidth = x2 - x1;
            int inputHeight = y2 - y1;
            Rectangle cropRectangle = new Rectangle(x1, y1, inputWidth, inputHeight);
            var bitmap = bmpImage.Clone(cropRectangle, bmpImage.PixelFormat);

            img.Dispose();
            bmpImage.Dispose();

            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
        }

        public static Stream CropOrNotImageStream(string filepath, InputDefinition input)
        {

            if (input.regions.Count != 0)
            {
                Byte[] cropImage = Utils.CropImageBytes(filepath, input.regions[0]);
                MemoryStream imageStream = new MemoryStream(cropImage);
                return imageStream;
            }
            else
            {
                Stream imageStream = ToStream(filepath);
                return imageStream;
            } 
        }
        public static Stream ToStream(string imagePath)
        {
            Stream stream = new FileStream(imagePath, FileMode.Open);
            return stream;
        }

    }
}
