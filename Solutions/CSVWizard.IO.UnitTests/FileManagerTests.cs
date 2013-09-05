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
            var result = fileManager.ReadFile("fileName");

            //Assert
            Assert.That(result, Is.Null);
        }
    }

    public class FileManager
    {
        public IEnumerable<string> ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            throw new Exception();
        }
    }
}