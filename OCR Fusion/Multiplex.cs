using APIObject;
using OCR_Fusion.OCR.Iron;
using OCR_Fusion.OCR.Placeholder;


namespace OCR_Fusion {
    public class Multiplex {

        private static Dictionary<string, Algorithms> algos = new();
        public static void Register(Type T, RegisterAttribute attribute) {

            algos.Add(attribute.id, new(attribute.id, attribute.name, attribute.description,T));

        }

        public static List<Algorithms> GetAlgos() {
            return algos.Values.ToList();
        }

        public static IOCRManager GetOCR(InputDefinition input,IConfiguration config) {

            if (config.GetValue<string>("mode") == "test") {
                return new PlaceholderOCR();
            }

            if (!algos.ContainsKey(input.algo)) {
                throw new NotImplementedException();
            }


            return (IOCRManager)Activator.CreateInstance(algos[input.algo].GetClass());

        }
    
    }


}
