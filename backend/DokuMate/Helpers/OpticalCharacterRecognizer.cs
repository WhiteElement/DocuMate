
using Tesseract;

namespace DokuMate.Helpers;

public class OpticalCharacterRecognizer
{
   public string Image { get; set; }
   private readonly string _tessdataFolder;

   public OpticalCharacterRecognizer()
   {
         _tessdataFolder = @".\tessdata";
      
   }
   public async Task<string> DoOcr()
   {
      string ocrContent =  String.Empty;
      try
      {
         using var engine = new TesseractEngine(_tessdataFolder, "deu");
         engine.SetVariable("user_defined_dpi", "70");
         using var img = Pix.LoadFromFile(Image);
         using var page = engine.Process(img);
         ocrContent = page.GetText();
      }
      catch (Exception e)
      {
         Console.WriteLine($"Exception doing OCR: {e}");
      }
      return ocrContent;
   }
}