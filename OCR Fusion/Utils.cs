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
    }
}
