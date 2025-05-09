﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DokuMate.Document;

public class PdfDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Info { get; set; }
    public List<Tag.Tag> Tags { get; set; }
    public BsonBinaryData Binary { get; set; }
    public string? OcrContent { get; set; }
    public DateTime Created { get; set; }
}