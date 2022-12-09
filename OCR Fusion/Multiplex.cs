using OCR_Fusion.OCR.Typography.Iron;
using OCR_Fusion.OCR.Typography.Placeholder;


namespace OCR_Fusion {
    public class Multiplex {

        private static Dictionary<string, OCRAlgorythm> algos = new();
        public static void Register(Type T, RegisterAttribute attribute) {

            algos.Add(attribute.id, new OCRAlgorythm { algo = T, Id = attribute.id, Name = attribute.name, Description = attribute.description });

        }
        public static IOCRManager GetOCR(InputDefinition input,IConfiguration config) {

            if (config.GetValue<string>("mode") == "test") {
                return new PlaceholderOCR();
            }

            if (!algos.ContainsKey(input.algo)) {
                throw new NotImplementedException();
            }

            return (IOCRManager)Activator.CreateInstance(algos[input.algo].algo);

        }
    
    }

    class OCRAlgorythm {
        public Type algo { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
