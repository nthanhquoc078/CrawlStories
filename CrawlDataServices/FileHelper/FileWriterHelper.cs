namespace CrawlDataServices.FileHelper
{
    public class FileWriterHelper : IFileWriterHelper
    {
        private string _fileName;
        private string _folderName;
        private string _filePath;
        public string FolderName
        {
            get => _folderName;
            set
            {
                _folderName = value;
                GenerateFilePath();
            }
        }
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                GenerateFilePath();
            }
        }
        public FileWriterHelper()
        {
            _folderName = "TempFolder";
            FileName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
        public FileWriterHelper(string folderName)
        {
            _folderName = folderName;
            FileName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
        public FileWriterHelper(string folderName, string fileName)
        {
            _folderName = folderName;
            this.FileName = fileName;
        }
        public async Task WriteToFileAsync(string content)
        {
            if(_filePath.IsNullOrEmpty())
            {
                GenerateFilePath();
            }
            try
            {
                string directory = Path.GetDirectoryName(_filePath) ?? string.Empty;
                if (directory.IsNullOrEmpty())
                {
                    return;
                }
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write the content to the file
                await File.WriteAllTextAsync(_filePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void GenerateFilePath()
        {
            _filePath = CustomSettings.SaveFileUrl + string.Format("{0}\\{1}", FolderName, FileName);
        }
        public async Task AddTextToFileAsync(string content)
        {
            if (_filePath.IsNullOrEmpty())
            {
                GenerateFilePath();
            }
            try
            {
                string directory = Path.GetDirectoryName(_filePath) ?? string.Empty;
                if (directory.IsNullOrEmpty())
                {
                    return;
                }
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write the content to the file
                await File.AppendAllTextAsync(_filePath, content + Environment.NewLine);
            }
            catch (Exception ex)
            {
                var logger = new Logger();
                await logger.Error(ex.ToString());
            }
        }
    }
}
