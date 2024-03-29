﻿using System.Text.Json.Nodes;

namespace OCR_Fusion.API_Object {
    public class Algorithms {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public JsonObject? Parameters { get; set; }

        private Type algo { get; set; }

        public Algorithms(string id, string name, string description, Type algo) {
            Id = id;
            Name = name;
            Description = description;
            this.algo = algo;

            IOCRManager ocr = (IOCRManager)Activator.CreateInstance(algo);

            this.Parameters = ocr.GetParameters();
        }

        public Type GetClass() {
            return algo;
        }
    }
}
