using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Set up the host and configuration
        var host = CreateHostBuilder(args).Build();

        // Get the services from the host
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var pdfToImages = new PdfToImages();
        
        // Read settings from configuration
        string pdfFilePath = @"input/BZB_24060712.pdf";
        string outputDirectory = @"output";
        string connectionString = configuration.GetConnectionString("AzureBlobStorage");
        string containerName = configuration["BlobContainerName"];

        // Input the range of pages
        Console.WriteLine("Enter the start page (leave blank to process all pages):");
        string startPageInput = Console.ReadLine();

        Console.WriteLine("Enter the end page (leave blank to process all pages):");
        string endPageInput = Console.ReadLine();

        int? startPage = string.IsNullOrWhiteSpace(startPageInput) ? (int?)null : int.Parse(startPageInput);
        int? endPage = string.IsNullOrWhiteSpace(endPageInput) ? (int?)null : int.Parse(endPageInput);

        // Convert PDF to images
        pdfToImages.ConvertPdfToImages(pdfFilePath, outputDirectory, startPage, endPage);

        // Upload images to Azure Blob Storage
        var azureBlob = new AzureBlob(connectionString, containerName);
        await azureBlob.UploadImagesAsync(outputDirectory);
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Load the appsettings.json
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Register your services here if needed
            });
}
