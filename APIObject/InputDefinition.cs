


namespace APIObject {
    public class InputDefinition {

        public string session { get; set; }
        public string imageName { get; set; }
        public string algo { get; set; }
        public List<Vector[]> regions { get; set; }

        public Dictionary<string,string>? parameters { get; set; }

    }
}
