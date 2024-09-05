
# PDF to Images Converter and Azure Blob Uploader

This C# application converts pages of a PDF file into images and uploads them to Azure Blob Storage. The application supports reading PDF files from a specified URL, converting the pages into images, and then uploading those images to Azure Blob Storage.

## Features

- **PDF to Images Conversion**: Converts each page of a PDF file to an individual image using ImageMagick.
- **Azure Blob Upload**: Automatically uploads the converted images to a specified Azure Blob Storage container.
- **Page Range Selection**: Allows users to specify a range of pages to convert or process the entire document if no range is specified.
- **Dynamic File Input**: User inputs the PDF file name, which is dynamically concatenated into the Blob URL.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Azure Blob Storage Account](https://azure.microsoft.com/en-us/services/storage/blobs/) with container and access permissions.
- [ImageMagick](https://imagemagick.org/) library for handling PDF to image conversion.
- [Ghostscript](https://www.ghostscript.com/download.html) for PDF processing by ImageMagick.

  > **Note**: Ghostscript is required for ImageMagick to process PDFs. Ensure Ghostscript is installed and configured correctly on your system for ImageMagick to work properly.

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/pladee42/PDF-to-Images.git
cd PDF-to-Images
```

### Configuration

1. **Update Azure Settings**: Ensure you have your Azure connection string and container name set up in `appsettings.json`.

    ```json
    {
      "ConnectionStrings": {
        "AzureBlobStorage": "your-connection-string"
      },
      "BlobContainerName": "your-container-name",
      "BlobUrl": "your-blob-url"
    }
    ```

2. **Install Dependencies**:
   - Install [ImageMagick](https://imagemagick.org/script/download.php).
   - Install [Ghostscript](https://www.ghostscript.com/download.html) to enable PDF processing in ImageMagick.

### Usage

1. **Run the Application**

    ```bash
    dotnet run
    ```

2. **Input the PDF File Name**: Enter the file name of the PDF (e.g., `BZB_24060712.pdf`). The file is accessed from a predefined base URL.

3. **Specify Page Range**: Input the start and end pages when prompted. Leave these fields blank to process all pages of the PDF.

### Example Output

- The images are saved locally in the `output` folder with the naming format `{fileNameWithoutExtension}-Page-{i}.png`.
- The images are then automatically uploaded to the specified Azure Blob Storage container.

## Code Overview

### `Program.cs`

- Handles user input for the PDF file name and optional page range.
- Downloads the PDF from the specified Azure Blob URL.
- Converts PDF pages to images using the `PdfToImages` class.
- Uploads the images to Azure Blob Storage.

### `PdfToImages.cs`

- Contains the method `ConvertPdfBytesToImages` to convert the PDF from byte arrays into images.
- Supports reading all pages when no specific range is provided.

### `AzureBlob.cs`

- Manages the connection to Azure Blob Storage.
- Handles the uploading of images to the specified blob container.

## Troubleshooting

- **Azure Connectivity**: Ensure your connection string is correct and that you have appropriate access permissions to the blob container.
- **ImageMagick and Ghostscript Errors**: Confirm that both ImageMagick and Ghostscript are installed correctly and accessible from your environment. Ghostscript is essential for ImageMagick to handle PDFs.

## Contact

For questions, suggestions, or issues, please contact [tan_waris1@hotmail.com](mailto:tan_waris1@hotmail.com).
