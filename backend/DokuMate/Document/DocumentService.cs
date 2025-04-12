using DokuMate.Database;
using DokuMate.PdfDocument;
using MongoDB.Driver;

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
             .Select(d => new PdfDocumentDTO( d.Id, d.Name, d.Info, d.Tags, d.Created ))
             .ToList();
     }

     public async Task<PdfDocument?> GetOne(string id)
     {
         return await _documentCollection
             .Find(x => x.Id == id)
             .FirstOrDefaultAsync();
     }

     public async Task<PdfDocument> CreateOne(ImageDocument imageDocument)
     {
         // Images to PDF
         // pdfBinary
         
         // TODO: OCR
         // ocrContent

         PdfDocument document = new PdfDocument()
         {
             Name = imageDocument.Name,
             Info = imageDocument.Info,
             Tags = imageDocument.Tags,
             Created = DateTime.Now,
             // Binary = pdfBinary,
             // OcrContent = ocrContent
         };

         await _documentCollection.InsertOneAsync(document);

         return document;
     }

     public async Task EditOne(PdfDocument document)
     {
         await _documentCollection.ReplaceOneAsync(x => x.Id == document.Id, document);
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
      
}