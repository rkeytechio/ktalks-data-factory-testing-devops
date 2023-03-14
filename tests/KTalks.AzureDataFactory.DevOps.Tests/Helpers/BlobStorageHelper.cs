using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using CsvHelper;
using System.Globalization;

namespace KTalks.AzureDataFactory.DevOps.Tests.Helpers
{
    public class BlobStorageHelper
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageHelper()
        {
            var settings = new Settings();
            var blobEndpoint = new Uri($"https://{settings.AzureDataLakeName}.blob.core.windows.net");
            _blobServiceClient = new BlobServiceClient(blobEndpoint, new ClientSecretCredential(settings.TenantId, settings.ClientId, settings.ClientSecret));
        }

        public async Task UploadCsvRecordsToBlob<T>(List<T> records, string containerName, string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            var header = new BlobHttpHeaders
            {
                ContentType = "text/csv"
            };

            await using var writer = new StreamWriter(new MemoryStream());
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(records);
            await writer.FlushAsync();

            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            await blobClient.UploadAsync(writer.BaseStream, new BlobUploadOptions { HttpHeaders = header });
        }

        public async Task<IEnumerable<T>> GetCsvRecordsFromBlob<T>(string containerName, string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlockBlobClient(fileName);

            var blobDownloadResult = await blobClient.DownloadContentAsync();

            var reader = new StreamReader(blobDownloadResult.Value.Content.ToStream());
            var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            return csv.GetRecords<T>();
        }
    }
}
