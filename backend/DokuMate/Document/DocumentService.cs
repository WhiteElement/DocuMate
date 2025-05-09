﻿using DokuMate.Database;
using DokuMate.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DokuMate.Document;

public class DocumentService
{
    private readonly IMongoCollection<PdfDocument> _documentCollection;
    private const string CollectionName = "Documents";

     public DocumentService(MongoDatabase mongoDataBase)
     {
         _documentCollection = mongoDataBase.Db.GetCollection<PdfDocument>(CollectionName);
     }

     public async Task<List<PdfDocumentDTO>> GetAll()
     {
         List<PdfDocument> documents = await _documentCollection
             .Find(_ => true)
             .ToListAsync();

         return documents
             .Select(d => new PdfDocumentDTO( d.Id, d.Name, d.Info, d.Tags, d.Created, null ))
             .ToList();
     }
     
     public async Task<List<PdfDocumentDTO>> GetAllFiltered(SearchRequest filterItem)
     {
         FilterDefinition<PdfDocument> filter = Builders<PdfDocument>.Filter
             .All(doc => doc.Tags, filterItem.Tags);
             
         return _documentCollection.AsQueryable()
             .Where(x => x.Name.Contains(filterItem.Name) && filter.Inject())
             .Select(d => new PdfDocumentDTO( d.Id, d.Name, d.Info, d.Tags, d.Created, null ))
             .ToList();
     }

     public async Task<PdfDocumentDTO?> GetOne(string id)
     {
         if (TryGet(id, out PdfDocument document))
         {
             return new PdfDocumentDTO(document.Id, document.Name, document.Info, document.Tags, document.Created, document.OcrContent );
         }

         return null;
     }

     private bool TryGet(string id, out PdfDocument document)
     {
         PdfDocument found = _documentCollection
             .Find(x => x.Id == id)
             .FirstOrDefault();

         if (found == null)
         {
             document = null;
             return false;
         }
         
         document = found;
         return true;
     }

     public async Task<PdfDocument> CreateOne(ImageDocument imageDocument)
     {
         if (imageDocument.Images == null || !imageDocument.Images.Any())
             throw new ArgumentException("No Images for Pdf Conversion provided");

         PdfConverter pdfConverter = new PdfConverter(imageDocument.Name, imageDocument.Images, false);
         pdfConverter.DocumentScan();
         (string pdf, string firstPage) = pdfConverter.ToPdf();
         

         PdfDocument document = new PdfDocument()
         {
             Name = $"{imageDocument.Name}.pdf",
             Info = imageDocument.Info,
             Tags = imageDocument.Tags ?? new List<Tag.Tag>(),
             Created = DateTime.Now,
             Binary = new BsonBinaryData(await File.ReadAllBytesAsync(pdf)),
         };

         await _documentCollection.InsertOneAsync(document);
         _ = Task.Run(() => AddOcrAsync(document, firstPage, pdfConverter));

         return document;
     }

     private async Task AddOcrAsync(PdfDocument document, string image, PdfConverter pdfConverter)
     {
         try {
             OpticalCharacterRecognizer ocr = new OpticalCharacterRecognizer() { Image = image };
             document.OcrContent = await ocr.DoOcr();

             await _documentCollection.ReplaceOneAsync(x => x.Id == document.Id, document);
             Console.WriteLine($"Updated {document.Id}, added ocr\n{document.OcrContent}");

             pdfConverter.CleanUp();

         }
         catch (Exception e)
         {
             Console.WriteLine($"Error doing OCR: {e}");
         }
     }

     public async Task<bool> DeleteOne(string id)
     {
         var result = await _documentCollection.DeleteOneAsync(x => x.Id == id);
         return result.DeletedCount > 0;
     }

     public async Task DeleteAll()
     {
         await _documentCollection.DeleteManyAsync(_ => true);
     }

     public PdfDocument Download(string id)
     {
         if (TryGet(id, out PdfDocument document))
         {
             return document;
         }

         return null;
     }

     public async Task<bool> UpdateOne(PdfDocumentDTO pdfDocumentDto)
     {
         if (!TryGet(pdfDocumentDto.Id, out PdfDocument document))
         {
             return false;
         }

         document.Tags = pdfDocumentDto.Tags;
         document.Name = pdfDocumentDto.Name;
         document.Info = pdfDocumentDto.Info;

         await _documentCollection.ReplaceOneAsync(x => x.Id == document.Id, document);
         return true;
     }
}