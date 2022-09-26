using System.IO.Compression;

namespace FileUpDownloadAPI.Services
{
    public class FileService : IFileService
    {
        //private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment; ->obselete (darf noch verwendet werden)
        private IHostEnvironment _hostEnvironment;


        public FileService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void UploadFile(List<IFormFile> files, string subDirectory)
        {
            subDirectory = subDirectory ?? string.Empty; //wenn subDirector == null, wird string.Empty zugewiesen


            string targetDirectory = Path.Combine(_hostEnvironment.ContentRootPath, subDirectory);

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            files.ForEach(async file =>
            {
                if (file.Length <= 0)
                    return;

                string filePath = Path.Combine(targetDirectory, file.FileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                } //Dipose soll hier aufgerufen werden   
            });
        }

        

        public string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            switch (fileSize)
            {
                case var _ when fileSize < kilobyte:
                    return $"Less then 1KB";
                case var _ when fileSize < megabyte:
                    return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
                case var _ when fileSize < gigabyte:
                    return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
                case var _ when fileSize >= gigabyte:
                    return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
                default:
                    return "n/a";
            }
        }

        public (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory)
        {
            string zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
            IList<string> files = Directory.GetFiles(Path.Combine(_hostEnvironment.ContentRootPath, subDirectory));

            byte[] compressBytes = GetZipArchive(files);

            return ("application/zip", compressBytes, zipName);
        }

        private byte[] GetZipArchive(IList<string> filePaths)
        {
            IList<InMemoryFile> files = new List<InMemoryFile>();

            foreach (string file in filePaths)
                files.Add(LoadFromFile(file));


            byte[] archiveFile;

            using (MemoryStream archiveStream = new MemoryStream())
            {
                //ZipArchive -> repsäentiert das ZipFile 
                using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (InMemoryFile currentMemoryFile in files)
                    {
                        ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(currentMemoryFile.FileName, CompressionLevel.Optimal);
                        
                        using Stream zipStream = zipArchiveEntry.Open();
                        zipStream.Write(currentMemoryFile.Content, 0, currentMemoryFile.Content.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }

        private InMemoryFile LoadFromFile(string filePath)
        {
            //lese mit FileStream Datei ein
            using FileStream fs = File.OpenRead(filePath);

            using MemoryStream ms = new MemoryStream();

            //übertrage diese in MemoryFile
            fs.CopyTo(ms);

            ms.Seek(0, SeekOrigin.Begin);

            return new InMemoryFile() { Content = ms.ToArray(), FileName = Path.GetFileName(filePath) };
        } //fs.Dispose() + ms.Dispose()
    }


    class InMemoryFile
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; } // ausgelesene Inhalt der Datei als byte - Array 
    }
}
