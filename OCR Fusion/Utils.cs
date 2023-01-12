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

        public static void Update<T>(string table, T value) {
            db.Update<T>(table, value);
        }

        public static List<T> Gets<T>(string table, string session) {
            return db.Gets<T>(table, session);
        }

        public static void Delete<T>(string table, string session) {
            db.Delete<T>(table, session);
        }
        public static Byte[] CropImage(string filepath, Vector[] cropArea)
        {
            Bitmap bmpImage = new Bitmap(filepath);
            Vector xy1 = cropArea[0];
            Vector xy2 = cropArea[1];
            float inputWidth = xy2.x - xy1.x;
            float inputHeight = xy2.y - xy1.y;
            Rectangle cropRectangle = new Rectangle((int)xy1.x, (int)xy1.y, (int)inputWidth, (int)inputHeight);
            var bytemap = bmpImage.Clone(cropRectangle, bmpImage.PixelFormat);
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bytemap, typeof(byte[]));
        }

    }
}
