using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            if (lines == null)
            {
                return null;
            }
            var totalList = new List<List<object>>();
            foreach (var line in lines)
            {
                ProcessLine(line, totalList);
            }

            return totalList;
        }

        private static void ProcessLine(string line, List<List<object>> totalList)
        {
            var list = new List<object>();
            var elements = line.Split(',');

            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                if (elements[i].StartsWith("\""))
                {
                    if (elements[i].EndsWith("\"") && elements[i].Count(e => e == '"') % 2 == 0)
                    {
                        if (elements.Count() > 1)
                        {
                            element = element.Substring(1, element.Length - 2);
                        }
                    }
                    else 
                    {
                        element = TryLinkElements(elements, element, ref i);
                    }
                }

                ParseElement(element, list);
            }
            totalList.Add(list);
            CheckNumberOfColumns(totalList, list);
        }

        private static string TryLinkElements(string[] elements, string element, ref int i)
        {
            var elementBuilder = new StringBuilder(elements[i]);
            var completed = false;
            var k = i;
            for (int j = i + 1; j < elements.Length && !completed; j++)
            {
                k++;
                elementBuilder.Append(",").Append(elements[j]);

                if (elements[j].EndsWith("\"") && elements[j].Count(e => e == '"')%2 != 0)
                {
                    completed = true;
                }
            }
            if (completed)
            {
                i = k;
                element = elementBuilder.ToString();
                element = element.Substring(1, element.Length - 2);
            }
            return element;
        }

        private static void ParseElement(string element, List<object> list)
        {
            int intResult;
            if (int.TryParse(element, out intResult))
            {
                list.Add(intResult);
                return;
            }
            double doubleResult;
            if (double.TryParse(element, out doubleResult))
            {
                list.Add(doubleResult);
                return;
            }
            list.Add(element.Replace("\"\"", "\""));
        }

        private static void CheckNumberOfColumns(List<List<object>> totalList, List<object> list)
        {
            if (totalList.First().Count() != list.Count())
            {
                throw new ColumnMismatchException();
            }
        }
    }
}