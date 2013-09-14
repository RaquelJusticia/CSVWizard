using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CSVWizard
{
    public class CSVManager : ICSVManager
    {
        private readonly IFileManager _fileManager;

        public CSVManager(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public IEnumerable<IEnumerable<T>> Load<T>(string fileName) where T : IComparable
        {
            var lines = _fileManager.ReadFile(fileName);
            if (lines == null)
            {
                return null;
            }
            var totalList = new List<List<T>>();
            foreach (var line in lines)
            {
                ProcessLine(line, totalList);
            }

            return totalList;
        }

        private static void ProcessLine<T>(string line, List<List<T>> totalList)
        {
            var list = new List<T>();
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

        private static void ParseElement<T>(string element, List<T> list)
        {

            int intResult;
            if (typeof(T) == typeof(int) && int.TryParse(element, out intResult))
            {
                list.Add((T)(object)intResult);
                return;
            }
            double doubleResult;
            if (typeof(T) == typeof(double) && double.TryParse(element, out doubleResult))
            {
                list.Add((T)(object)doubleResult);
                return;
            }
            DateTime dateTimeResult;
            if (typeof(T) == typeof(DateTime) && DateTime.TryParse(element, out dateTimeResult))
            {
                list.Add((T)(object)dateTimeResult);
                return;
            }
            char charResult;
            if (typeof(T) == typeof(char) && char.TryParse(element, out charResult))
            {
                list.Add((T)(object)charResult);
                return;
            }
            list.Add((T)(object)element.Replace("\"\"", "\""));
        }

        private static void CheckNumberOfColumns<T>(List<List<T>> totalList, List<T> list)
        {
            if (totalList.First().Count() != list.Count())
            {
                throw new ColumnMismatchException();
            }
        }
    }
}