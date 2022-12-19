using OCR_Fusion.OCR.Typography.Iron;
using OCR_Fusion.OCR.Typography.Placeholder;

namespace OCR_Fusion {
    public class Multiplex {
    
    
        public static IOCRManager GetOCR(InputDefinition input,IConfiguration config) {

            if (config.GetValue<string>("mode") == "test") {
                return new PlaceholderOCR();
            }


            if (!input.IsHandWritten) {
                return new IronOCR();
            }

            throw new NotImplementedException();

        }
    
    }
}
