﻿using System;
using System.Collections.Generic;
using System.Text;

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
            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                if (elements[i].StartsWith("\""))
                {
                    var elementBuilder = new StringBuilder(elements[i]);
                    for (int j = i+1; j < elements.Length; j++)
                    {
                        i++;
                        elementBuilder.Append(",").Append(elements[j]);
                        if (elements[j].EndsWith("\""))
                        {
                            var elementString = elementBuilder.ToString();
                            element = elementString.Substring(1, elementString.Length - 2).Replace("\"\"", "\"");
                            break;
                        }
                    }
                }
                
                ParseElement(element, list);
            }
            totalList.Add(list);
        }

        private static void ParseElement(string element, List<object> list)
        {
            int intResult;
            double doubleResult;
            if (int.TryParse(element, out intResult))
            {
                list.Add(intResult);
                return;
            }
            if (double.TryParse(element, out doubleResult))
            {
                list.Add(doubleResult);
                return;
            }
            list.Add(element);
        }
    }
}