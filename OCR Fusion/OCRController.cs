namespace OCR_Fusion {
    public class OCRController {

        private IOCRManager ocrManager;
        public OCRController(IOCRManager oCRManager){ 
            this.ocrManager = oCRManager;
        }


        public OutputDefinition GetText(InputDefinition input) {

            OutputDefinition output = ocrManager.GetText(input);

            return output;
        }

    }
}
