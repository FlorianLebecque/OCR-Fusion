using OCR_Fusion.OCR.Iron;
using OCR_Fusion.OCR.Placeholder;
using OCR_Fusion.OCR.VisionAPI;

namespace OCR_Fusion {
    public class Multiplex {
    
    
        public static IOCRManager GetOCR(InputDefinition input,IConfiguration config) {

            if (config.GetValue<string>("mode") == "test") {
                return new PlaceholderOCR();
            }


            if (!input.IsHandWritten) {
                return new IronOCR();
            }
            else
            {
                return new VisionOCR();
            }

            throw new NotImplementedException();

        }
    
    }
}
