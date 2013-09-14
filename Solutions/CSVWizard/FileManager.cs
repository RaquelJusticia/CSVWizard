using System.Collections.Generic;
using System.IO;

namespace CSVWizard
{
    public class FileManager : IFileManager
    {
        public IEnumerable<string> ReadFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension != ".csv")
            {
                throw new InvalidExtensionException(extension);
            }

            if (!File.Exists(fileName))
            {
                return null;
            }

            return File.ReadAllLines(fileName);
        }
    }
}