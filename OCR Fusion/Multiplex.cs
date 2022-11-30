using OCR_Fusion.OCR.Typography.Iron;

namespace OCR_Fusion {
    public class Multiplex {
    
    
        public static IOCRManager GetOCR(bool isHandWritter) {

            if (isHandWritter) {
                return new IronOCR();
            }

            throw new NotImplementedException();

        }
    
    }
}
