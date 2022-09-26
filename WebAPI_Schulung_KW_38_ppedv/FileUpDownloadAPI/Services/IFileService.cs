namespace FileUpDownloadAPI.Services
{
    public interface IFileService
    {
        //Hochladen von Daten
        void UploadFile(List<IFormFile> files, string subDirectory);

        //Upload - Size Anzeige
        string SizeConverter(long bytes);


        (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory);

    }
}
