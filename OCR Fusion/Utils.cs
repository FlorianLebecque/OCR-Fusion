namespace OCR_Fusion {
    public class Utils {

        public static string GetExtention(string filename) {
            return filename.Split(".")[filename.Split(".").Length - 1];
        }

    }
}
