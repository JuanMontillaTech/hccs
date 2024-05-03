using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace ERP.API.Model
{
    public class DownloadRequest
    {
        public string Url { get; set; }
        public string DestinationPath { get; set; }
    }
    public static class FileDownloader
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task DownloadFileAsync(string url, string destinationPath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(destinationPath))
            {
                throw new ArgumentException("URL y ruta de destino son requeridos.");
            }

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create))
                {
                    await contentStream.CopyToAsync(fileStream);
                }
            }
        }
    }
}
