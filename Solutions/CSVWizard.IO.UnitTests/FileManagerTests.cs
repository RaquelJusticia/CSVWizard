using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace CSVWizard.IO.UnitTests
{
    public class FileManagerTests
    {
        [Test]
        public void ShouldReturnNullIfFileDoesNotExist()
        {
            //Arrange
            var fileManager = new FileManager();

            //Act
            var result = fileManager.ReadFile("fileName.csv");

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ShouldThrowInvalidExtensionIfNotCSV()
        {
            //Arrange
            var fileManager = new FileManager();

            //Act && Assert
            Assert.Throws<InvalidExtensionException>(() => fileManager.ReadFile("filename.a"));
            
        }
    }

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

    public class FileManager
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

            throw new Exception();
        }
    }
}