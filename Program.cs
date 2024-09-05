using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        // Set up the host and configuration
        var host = CreateHostBuilder(args).Build();

        // Get the services from the host
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var pdfToImages = new PdfToImages();
        
        // Get connection string and container name from configuration
        string connectionString = configuration.GetConnectionString("AzureBlobStorage")!;
        string containerName = configuration["BlobContainerName"]!;
        string baseUrl = configuration["BlobUrl"]!;

        // User inputs the name of the file to be downloaded
        Console.WriteLine("Enter the name of the PDF file (e.g., BZB_24060712.pdf):");
        string fileName = Console.ReadLine()!;
        string fileUrl = $"{baseUrl}{fileName}";

        // Input the range of pages
        Console.WriteLine("Enter the start page (leave blank to process all pages):");
        string? startPageInput = Console.ReadLine();

        Console.WriteLine("Enter the end page (leave blank to process all pages):");
        string? endPageInput = Console.ReadLine();

        // Convert page input to nullable integers
        int? startPage = string.IsNullOrWhiteSpace(startPageInput) ? null : int.Parse(startPageInput);
        int? endPage = string.IsNullOrWhiteSpace(endPageInput) ? null : int.Parse(endPageInput);

        // Download the PDF file from Azure Blob URL
        try
        {
            byte[] pdfBytes = await DownloadPdfFromBlob(fileUrl);
            Console.WriteLine("PDF downloaded successfully.");

            // Specify output directory for images
            string outputDirectory = "output";

            // Convert downloaded PDF bytes to images with the specified range
            pdfToImages.ConvertPdfBytesToImages(pdfBytes, outputDirectory, fileName, startPage, endPage);

            // Upload images to Azure Blob Storage
            var azureBlob = new AzureBlob(connectionString, containerName);
            await azureBlob.UploadImagesAsync(outputDirectory);

            Console.WriteLine("Images uploaded to Azure Blob Storage successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Method to download PDF as byte array from the given URL
    static async Task<byte[]> DownloadPdfFromBlob(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }

    // Create and configure the host builder
    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Register your services here if needed
            });
}
