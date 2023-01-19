using System.Text.Json.Nodes;

namespace OCR_Fusion {
    public interface IOCRManager {

        public JsonObject GetParameters();
        public OutputDefinition GetText(InputDefinition input );


    }
}
