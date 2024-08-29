using ImageMagick;
using System;
using System.IO;

public class PdfToImages
{
    public void ConvertPdfToImages(string pdfFilePath, string outputDirectory, int? startPage = null, int? endPage = null)
    {
        // Ensure the output directory exists
        Directory.CreateDirectory(outputDirectory);

        // Extract the file name without extension
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pdfFilePath);

        // Load the PDF file
        using (var images = new MagickImageCollection())
        {
            images.Read(pdfFilePath);

            // If startPage or endPage is not provided, set them to cover all pages
            startPage ??= 1;
            endPage ??= images.Count;

            // Validate page range
            if (startPage < 1 || endPage > images.Count || startPage > endPage)
            {
                throw new ArgumentOutOfRangeException("Invalid page range specified.");
            }

            for (int i = startPage.Value; i <= endPage.Value; i++)
            {
                var image = images[i - 1];  // Pages are 0-indexed in MagickImageCollection

                // Set the output format (e.g., png, jpg, etc.)
                image.Format = MagickFormat.Png;

                // Create the output file path with the format {InputFileName}-Page-{pagenumber}
                string outputFilePath = Path.Combine(outputDirectory, $"{fileNameWithoutExtension}-Page-{i}.png");

                // Save the image
                image.Write(outputFilePath);

                Console.WriteLine($"Page {i} converted to image: {outputFilePath}");
            }
        }

        Console.WriteLine("Specified PDF pages converted to images successfully!");
    }
}
