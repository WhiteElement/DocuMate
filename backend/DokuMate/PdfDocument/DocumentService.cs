using DokuMate.Database;
using MongoDB.Driver;

namespace DokuMate.PdfDocument;

public class DocumentService
{
    private readonly IMongoCollection<PdfDocument> documentCollection;
    private const string CollectionName = "Documents";

     public DocumentService(MongoDatabase mongoDataBase)
     {
         documentCollection = mongoDataBase.Db.GetCollection<PdfDocument>(CollectionName);
     }

     public async Task<List<PdfDocumentDTO>> GetAll()
     {
         List<PdfDocument> documents = await documentCollection
             .Find(_ => true)
             .ToListAsync();

         return documents
             .Select(d => new PdfDocumentDTO( d.Id, d.Name, d.Info, d.Tags, d.Created ))
             .ToList();
     }

     public async Task<PdfDocument?> GetOne(string id)
     {
         return await documentCollection
             .Find(x => x.Id == id)
             .FirstOrDefaultAsync();
     }

     public async Task<PdfDocument> CreateOne(PdfDocument document)
     {
         // TODO: OCR

         // document.OcrContent = ocrContent;
         await documentCollection.InsertOneAsync(document);

         return document;
     }

     public async Task EditOne(PdfDocument document)
     {
         await documentCollection.ReplaceOneAsync(x => x.Id == document.Id, document);
     }

     public async Task<bool> DeleteOne(string id)
     {
         var result = await documentCollection.DeleteOneAsync(x => x.Id == id);
         return result.DeletedCount > 0;
     }

     public async Task DeleteAll()
     {
         await documentCollection.DeleteManyAsync(_ => true);
     }
      
}