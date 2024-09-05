using ImageMagick;
using System;
using System.IO;

public class PdfToImages
{
    // Method to convert PDF from byte array to images
    public void ConvertPdfBytesToImages(byte[] pdfBytes, string outputDirectory, string fileName, int? startPage = null, int? endPage = null)
    {
        // Ensure the output directory exists
        Directory.CreateDirectory(outputDirectory);

        // Extract the file name without extension
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        // Load the PDF from the byte array
        using (var stream = new MemoryStream(pdfBytes))
        {
            using (var images = new MagickImageCollection())
            {
                // Check if specific pages need to be read
                if (startPage.HasValue && endPage.HasValue)
                {
                    // Configure settings to read a specific range of pages
                    var settings = new MagickReadSettings
                    {
                        FrameIndex = startPage.Value - 1, // Convert to zero-based index
                        FrameCount = endPage.Value - startPage.Value + 1
                    };
                    images.Read(stream, settings);
                }
                else
                {
                    // Read the entire document without restricting pages
                    images.Read(stream);
                }

                // Save each page as an image with the specified naming format
                for (int i = 0; i < images.Count; i++)
                {
                    string imageFileName = $"{fileNameWithoutExtension}-Page-{i + 1}.png";
                    images[i].Write(Path.Combine(outputDirectory, imageFileName));
                    Console.WriteLine($"Saved {imageFileName}");
                }
            }
        }
    }
}
