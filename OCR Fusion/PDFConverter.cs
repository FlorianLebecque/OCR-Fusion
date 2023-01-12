using Aspose.Pdf;
using Aspose.Pdf.Devices;
using System.Drawing;
using System.Drawing.Imaging;
using Point = System.Drawing.Point;

namespace OCR_Fusion {

    public class PDFConverter {
    
        public string Convert(IFormFile doc) {

            string pdf_path = Utils.uploadPath + "temps/" + doc.FileName;
            string img_path = Utils.GetImagePath(doc.FileName+".png");

            using (var fileStream = new FileStream(pdf_path, FileMode.Create)) {
                doc.CopyTo(fileStream);
            }

            List<string> temp_imgs = PDFToBMPS(doc,pdf_path);
            Bitmap bmp = MergePDFBMPS(temp_imgs);

            bmp.Save(img_path, ImageFormat.Png);
            bmp.Dispose();

            File.Delete(pdf_path);
            foreach (string img in temp_imgs) {
                File.Delete(img);
            }

            return doc.FileName + ".png";
        }




        private List<string> PDFToBMPS(IFormFile doc,string pdf_path) {
            List<string> temps_img = new();
            Document pdfDocument = new Document(pdf_path);
            foreach (var page in pdfDocument.Pages) {

                PageSize ps = new((float)page.PageInfo.Width, (float)page.PageInfo.Height);

                // Create PNG device with specified attributes
                // Width, Height, Resolution
                BmpDevice BmpDevice = new BmpDevice((int)page.PageInfo.Width,(int)page.PageInfo.Height);

                // Convert a particular page and save the image to stream
                string file_name = Utils.uploadPath + "temps/" + doc.FileName + "_" + page.Number + "_out" + ".bmp";
                temps_img.Add(file_name);
                BmpDevice.Process(pdfDocument.Pages[page.Number], file_name);
            }

            pdfDocument.Dispose();

            return temps_img;
        }

        private Bitmap MergePDFBMPS(List<string> temps_bmps) {

            Bitmap temp = new Bitmap(temps_bmps[0]);

            for (int i = 1; i < temps_bmps.Count; i++) {

                Bitmap a = new Bitmap(temps_bmps[i]);
                Bitmap c = MergedBitmaps(temp, a);

                temp.Dispose();
                a.Dispose();

                temp = c;
            }

            return temp;
        }

        private Bitmap MergedBitmaps(Bitmap bmp1, Bitmap bmp2) {
            Bitmap result = new Bitmap(Math.Max(bmp1.Width, bmp2.Width), bmp1.Height + bmp2.Height);
            using (Graphics g = Graphics.FromImage(result)) {
                g.DrawImage(bmp1, Point.Empty);

                g.DrawImage(bmp2,0,bmp1.Height);
            }
            return result;
        }


    }
}
