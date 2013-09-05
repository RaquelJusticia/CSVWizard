using System;

namespace CSVWizard.IO
{
    public class InvalidExtensionException : Exception
    {
        private readonly string _extension;

        public InvalidExtensionException(string extension)
        {
            _extension = extension;
        }

        public override string Message
        {
            get { return "The file does not have a CSV extension. Extension found: " + _extension; }
        }
    }
}