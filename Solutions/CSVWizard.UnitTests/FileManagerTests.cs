using System.Linq;
using NUnit.Framework;

namespace CSVWizard.UnitTests
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
}