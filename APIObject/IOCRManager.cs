using System.Text.Json.Nodes;

namespace APIObject {
    public interface IOCRManager {

        public JsonObject GetParameters();
        public OutputDefinition GetText(InputDefinition input );

    }
}
