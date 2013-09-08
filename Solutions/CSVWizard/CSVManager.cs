using System;
using System.Collections.Generic;

namespace CSVWizard
{
    public class CSVManager
    {
        private readonly IFileManager _fileManager;

        public CSVManager(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public IEnumerable<IEnumerable<object>> Load(string fileName)
        {
            var lines = _fileManager.ReadFile(fileName);
            var totalList = new List<List<object>>();
            foreach (var line in lines)
            {
                if (line.Contains(",") == false)
                {
                    throw new InvalidOperationException();
                }

                ProcessLine(line, totalList);
            }

            return totalList;
        }

        private static void ProcessLine(string line, List<List<object>> totalList)
        {
            var list = new List<object>();
            var elements = line.Split(',');
            foreach (var element in elements)
            {
                int intResult;
                double doubleResult;
                if (int.TryParse(element, out intResult))
                {
                    list.Add(intResult);
                    continue;
                }
                if (double.TryParse(element, out doubleResult))
                {
                    list.Add(doubleResult);
                    continue;
                }
                list.Add(element);
            }
            totalList.Add(list);
        }
    }
}