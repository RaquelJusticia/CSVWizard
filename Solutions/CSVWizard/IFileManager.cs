using System.Collections.Generic;

namespace CSVWizard
{
    public interface IFileManager
    {
        IEnumerable<string> ReadFile(string fileName);
    }
}