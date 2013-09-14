using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace CSVWizard.UnitTests
{
    public class CSVManagerTests
    {
        [Test]
        public void ShouldUseFileManager()
        {
            //Arrange
            const string fileName = "FileName";
            var fileManager = new Mock<IFileManager>();
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            csvManager.Load<string>(fileName);

            //Assert
            fileManager.Verify(f => f.ReadFile(fileName));
        }

        [TestCase("a,b", "a", "b")]
        [TestCase("a,1.8", "a", "1.8")]
        [TestCase("a,1", "a", "1")]
        [TestCase("\"a,b\",c", "a,b", "c")]
        [TestCase("\"\"\"a\"\", \"\"b\"\"\",c", "\"a\", \"b\"", "c")]
        [TestCase("\"\"\"a\"\"\",\"\"\"b\"\"\"", "\"a\"", "\"b\"")]
        [TestCase("\",\",\"\"\"b\"\"\"", ",", "\"b\"")]
        [TestCase("\"\"\"\",\"\"\"b\"\"\"", "\"", "\"b\"")]
        [TestCase("\"\"\"a\"\".\"\"b\"\"\",c", "\"a\".\"b\"", "c")]
        [TestCase("\"\"a, b", "\"a", " b")]
        [TestCase("\"\"a\",b\"\"\"\"", "\"a\"", "b\"\"")]
        public void ShouldParseCSV(string line, string s1, string s2)
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {line});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<string>(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(2));
            Assert.That(result.First().First(), Is.EqualTo(s1));
            Assert.That(result.First().Skip(1).First(), Is.EqualTo(s2));
        }

        [TestCase("\"\"", "\"")]
        [TestCase("\",\"", ",")]
        public void ShouldParseCSVWhenOnlyOneColumn(string element, object expectedElement)
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {element});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<string>(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(1));
            Assert.That(result.First().First(), Is.EqualTo(expectedElement));
        }

        [Test]
        public void ShouldReturnNullWhenFileIsEmpty()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<string>(fileName);

            //Assert
            //Assert
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldThrowColumnMismatchException()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {"a,b,c", "a,b"});
            var csvManager = new CSVManager(fileManager.Object);

            //Act && Assert
            Assert.Throws<ColumnMismatchException>(() => csvManager.Load<string>(fileName));
        }

        [Test]
        public void ShouldParseCSVWithIntOnly()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {"1,2,3"});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<int>(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(3));
            Assert.That(result.First().First(), Is.EqualTo(1));
            Assert.That(result.First().Skip(1).First(), Is.EqualTo(2));
            Assert.That(result.First().Skip(2).First(), Is.EqualTo(3));
        }

        [Test]
        public void ShouldParseCSVWithDoubleOnly()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {"1.8,2.2,3.5"});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<double>(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(3));
            Assert.That(result.First().First(), Is.EqualTo(1.8));
            Assert.That(result.First().Skip(1).First(), Is.EqualTo(2.2));
            Assert.That(result.First().Skip(2).First(), Is.EqualTo(3.5));
        }

        [Test]
        public void ShouldParseCSVWithCharOnly()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {"a,b,c"});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<char>(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(3));
            Assert.That(result.First().First(), Is.EqualTo('a'));
            Assert.That(result.First().Skip(1).First(), Is.EqualTo('b'));
            Assert.That(result.First().Skip(2).First(), Is.EqualTo('c'));
        }

        [Test]
        public void ShouldParseCSVWithDateTimeOnly()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> {"14/09/2013"});
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load<DateTime>(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(1));
            Assert.That(result.First().First(), Is.EqualTo(new DateTime(2013, 09, 14)));
        }
    }
}