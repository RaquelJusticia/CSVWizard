using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using System.Linq;

namespace CSVWizard.IO.UnitTests
{
    public class FileManagerTests
    {
        private const string CSVFile = "TestFiles\\test.csv";

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

        [Test]
        public void ShouldReturnCSV()
        {
            //Arrange
            const string csvLine1 = "a,b,c";
            const string csvLine2 = "d,e,f";

            var fileManager = new FileManager();

            //Act
            var result = fileManager.ReadFile(CSVFile);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First(), Is.EqualTo(csvLine1));
            Assert.That(result.Skip(1).First(), Is.EqualTo(csvLine2));
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

            return File.ReadAllLines(fileName);
        }
    }
}