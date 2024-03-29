﻿using MongoDB.Bson.Serialization.Attributes;
using OCR_Fusion.API_Object;
using System.Text.Json.Nodes;

namespace OCR_Fusion {
    public class OutputDefinition {

        public OutputDefinition() {
            Id = new();
            words = new();
            regions = new();
        }

        public OutputDefinition(Guid Id, string words) {

            this.Id = Id;
            this.words = new() { words };
        }

        [BsonId]
        public Guid Id { get; set; }
        public string session { get; set; }

        public string algorithm { get; set; }
        public string imageName { get; set; }

        public Dictionary<string,string>? parameters { get; set; }
        public List<string> words { get; set; }
        public Dictionary<string, Vector[]>? regions { get; set; }
    }
}
