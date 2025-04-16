using Azure;
using Azure.AI.Vision.ImageAnalysis;

namespace SubtitulosDensos
{
    internal class Program
    {

        //Documentacion de Microsoft para el SDK de Azure Vision
        //https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/call-analyze-image-40?pivots=programming-language-csharp

        private static readonly string endpoint = "URL";
        private static readonly string apiKey = "KEY";

        static async Task Main(string[] args)
        {

            var imagePath = "RUTA DEL ARCHIVO";

            using FileStream stream = new FileStream(imagePath, FileMode.Open);
            BinaryData imageData = BinaryData.FromStream(stream);

            ImageAnalysisClient client = new ImageAnalysisClient(
            new Uri(endpoint),
            new AzureKeyCredential(apiKey));

            VisualFeatures visualFeatures =
            VisualFeatures.Caption |
            VisualFeatures.DenseCaptions |
            VisualFeatures.Objects |
            VisualFeatures.Read |
            VisualFeatures.Tags |
            VisualFeatures.People |
            VisualFeatures.SmartCrops;

            ImageAnalysisOptions options = new ImageAnalysisOptions
            {
                GenderNeutralCaption = true,
                Language = "en",
                SmartCropsAspectRatios = new float[] { 0.9F, 1.33F }
            };

            ImageAnalysisResult result = client.Analyze(
            imageData,
            visualFeatures,
            options);

            Console.WriteLine("****************************************************");
            Console.WriteLine("Caption:");
            Console.WriteLine("Descripción de la imagen:");
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            Console.WriteLine($"Texto: '{result.Caption.Text}'");
            Console.WriteLine($"Confianza: {result.Caption.Confidence:P2}");
            Console.WriteLine();

            Console.WriteLine("****************************************************");
            Console.WriteLine("Dense Captions:");
            Console.WriteLine("Descripciones detalladas de la imagen:");
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            foreach (var denseCaption in result.DenseCaptions.Values)
            {
                Console.WriteLine($"Texto: '{denseCaption.Text}'");
                Console.WriteLine($"Confianza: {denseCaption.Confidence:P2}");
                Console.WriteLine($"Caja de delimitación: {denseCaption.BoundingBox}");
                Console.WriteLine();

            }
            Console.WriteLine();

            Console.WriteLine("****************************************************");
            Console.WriteLine("Objects:");
            Console.WriteLine("Detección de objetos y etiquetas:");
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            foreach (DetectedObject detectedObject in result.Objects.Values)
            {
                Console.WriteLine($"Etiqueta: {detectedObject.Tags.First().Name}");
                Console.WriteLine($"Cuadro delimitador {detectedObject.BoundingBox.ToString()}");
                Console.WriteLine();
            }
            Console.WriteLine();


            Console.WriteLine("****************************************************");
            Console.WriteLine("Read: (OCR)");
            Console.WriteLine("Detectar texto en imagen");
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            foreach (DetectedTextBlock block in result.Read.Blocks)
                foreach (DetectedTextLine line in block.Lines)
                {
                   
                    foreach (DetectedTextWord word in line.Words)
                    {
                        Console.WriteLine($"{word.Text}  => Confianza: {word.Confidence:P2}");               
                    }
                }
            Console.WriteLine();

            Console.WriteLine("****************************************************");
            Console.WriteLine("Tags:");
            Console.WriteLine("Etiquetas:");
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            foreach (DetectedTag tag in result.Tags.Values)
            {
                Console.WriteLine($"Etiqueta: {tag.Name}");
                Console.WriteLine($"Confianza: {tag.Confidence:P2}");
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("****************************************************");
            Console.WriteLine("People:");
            Console.WriteLine("Detección de personas:");
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            foreach (DetectedPerson person in result.People.Values)
            {
                Console.WriteLine($"Cuadro delimitador: {person.BoundingBox.ToString()}");
                Console.WriteLine($"Confianza: {person.Confidence:P2}");
                Console.WriteLine();
            }
        }
    }
}