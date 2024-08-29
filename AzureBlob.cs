using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;


public class AzureBlob
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureBlob(string connectionString, string containerName)
    {
        _connectionString = connectionString;
        _containerName = containerName;
    }

    public async Task UploadImagesAsync(string directoryPath)
    {
        // Get a reference to the container
        BlobContainerClient containerClient = new BlobContainerClient(_connectionString, _containerName);

        // Ensure the container exists
        await containerClient.CreateIfNotExistsAsync();

        // Get the list of files in the output directory
        string[] files = Directory.GetFiles(directoryPath, "*.png");

        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);

            // Get a reference to the blob (file) to upload
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                await blobClient.UploadAsync(fileStream, true);
            }

            Console.WriteLine($"Uploaded {fileName} to Azure Blob Storage.");
        }

        Console.WriteLine("All files uploaded successfully!");
    }
}
