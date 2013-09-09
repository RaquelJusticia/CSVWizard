using System;

namespace CSVWizard
{
    public class ColumnMismatchException : Exception
    {
        public override string Message
        {
            get
            {
                return "Number of columns should be the same in all lines";
            }
        }
    }
}