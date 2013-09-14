using System.Collections.Generic;

namespace CSVWizard
{
    public interface ICSVManager
    {
        IEnumerable<IEnumerable<object>> Load(string fileName);
    }
}