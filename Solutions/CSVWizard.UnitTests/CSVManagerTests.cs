﻿using System;
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
            csvManager.Load(fileName);

            //Assert
            fileManager.Verify(f => f.ReadFile(fileName));
        }

        [Ignore] 
        public void ShouldThrowInvalidOperationExceptionIfIsNotCommaSeparated()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string>{"a.b.c"});
            var csvManager = new CSVManager(fileManager.Object);

            //Act && Assert
            Assert.Throws<InvalidOperationException>(() => csvManager.Load(fileName));
        }

        [TestCase("a,b", "a", "b")]
        [TestCase("a,1.8", "a", 1.8)]
        [TestCase("a,1", "a", 1)]
        [TestCase("\"a,b\",c", "a,b", "c")]
        [TestCase("\"\"\"a\"\", \"\"b\"\"\",c", "\"a\", \"b\"", "c")]
        [TestCase("\"\"\"a\"\".\"\"b\"\"\",c", "\"a\".\"b\"", "c")]
        public void ShouldParseCSV(string line, object o1, object o2)
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> { line });
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load(fileName);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(2));
            Assert.That(result.First().First(), Is.EqualTo(o1));
            Assert.That(result.First().Skip(1).First(), Is.EqualTo(o2));
        }

        [Test]
        public void ShouldParseCSVWhenOnlyOneColumn()
        {
            //Arrange
            const string fileName = "file";
            const string character = "1.8";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> { character });
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load(fileName);

            //Assert
            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Count(), Is.EqualTo(1));
            Assert.That(result.First().First(), Is.EqualTo(character));
        }

        [Test]
        public void ShouldReturnNullWhenFileIsEmpty()
        {
            //Arrange
            const string fileName = "file";
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> { });
            var csvManager = new CSVManager(fileManager.Object);

            //Act
            var result = csvManager.Load(fileName);

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
            fileManager.Setup(f => f.ReadFile(fileName)).Returns(new List<string> { "a,b,c", "a,b" });
            var csvManager = new CSVManager(fileManager.Object);

            //Act && Assert
            Assert.Throws<ColumnMismatchException>(() => csvManager.Load(fileName));
        }
    }
}