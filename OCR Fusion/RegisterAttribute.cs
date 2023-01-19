using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace OCR_Fusion {
    public class RegisterAttribute : Attribute {

        public string id; 
        public string name;
        public string description;
        public RegisterAttribute(string id,string name,string description) {
            this.id = id;
            this.name = name;
            this.description = description;
        }

    }
}