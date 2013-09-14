using System;
using System.Collections.Generic;

namespace CSVWizard
{
    public interface ICSVManager
    {
        IEnumerable<IEnumerable<T>> Load<T>(string fileName) where T : IComparable;
    }
}