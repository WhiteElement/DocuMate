using IronOcr;

namespace DokuMate.Helpers;

public class OpticalCharacterRecognizer
{
    private static IronTesseract _ironTesseract = new IronTesseract();
   public string Pdf { get; set; }

   public OpticalCharacterRecognizer()
   {
       _ironTesseract.Configuration.BlackListCharacters = "~`$#^*_}{][|\\";
       _ironTesseract.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.Auto;
       _ironTesseract.Language = OcrLanguage.German;
   }
   
   public string DoOcr()
   {
         using var input = new OcrInput();
         input.LoadPdf(Pdf);
         OcrResult result = _ironTesseract.Read(input);
         
         return result.Text;
   }
}