using eVote360Pro.Core.Application.Interfaces.Infrastructure;
using Tesseract;
using System.IO;
using System.Text.RegularExpressions;

namespace eVote360Pro.Infraestructure.Shared.Services
{
    public class OcrService : IOcrService
    {
        public async Task<string?> ExtractDocumentNumberAsync(string imagePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    string dataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");
                    
                    if (!Directory.Exists(dataPath))
                    {
                        throw new DirectoryNotFoundException($"El directorio de datos de Tesseract 'tessdata' no se encontró en {dataPath}");
                    }

                    string trainedDataPath = Path.Combine(dataPath, "spa.traineddata");
                    if (!File.Exists(trainedDataPath))
                    {
                        throw new FileNotFoundException($"El archivo de idioma de Tesseract 'spa.traineddata' no se encontró en {trainedDataPath}");
                    }

                    using var engine = new TesseractEngine(dataPath, "spa", EngineMode.Default);
                    using var img = Pix.LoadFromFile(imagePath);
                    using var page = engine.Process(img);
                    string extractedText = page.GetText();

                    if (string.IsNullOrEmpty(extractedText))
                    {
                        return null;
                    }

                    var regex = new Regex(@"\b\d{3}-?\d{7}-?\d{1}\b");
                    var match = regex.Match(extractedText);
                    
                    if (match.Success)
                    {
                        return match.Value.Replace("-", "").Replace(" ", "").Trim();
                    }

                    var cleanText = Regex.Replace(extractedText, @"[^\d]", "");
                    var fallbackRegex = new Regex(@"\d{11}");
                    var fallbackMatch = fallbackRegex.Match(cleanText);

                    if (fallbackMatch.Success)
                    {
                        return fallbackMatch.Value;
                    }

                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
