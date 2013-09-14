using System.Collections.Generic;
using System.IO;
using CSVWizard;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System.Linq;

namespace Specs.CSVWizard
{
    [Binding]
    public class StepDefinitions
    {
        private const string KeyCsv = "KeyCsv";

        [Given(@"I have a well formed CSV file")]
        public void GivenIHaveAWellFormedCSVFile()
        {
            string[] lines = { "a,b,c,d", "\"\"\"\",\"\"\"b\"\"\",\"a\"\"\",1", "1.8,\"\"\"g\",\",\",y" };
            File.WriteAllLines("file.csv", lines);
        }

        [When(@"I load the CSV file")]
        public void WhenILoadTheCSVFile()
        {
            var csvManager = new CSVManager(new FileManager());
            var csv = csvManager.Load<string>("file.csv");
            ScenarioContext.Current.Add(KeyCsv, csv);
        }

        [Then(@"I should have the correct structure in memory")]
        public void ThenIShouldHaveTheCorrectStructureInMemory()
        {
            var csv = ScenarioContext.Current.Get<IEnumerable<IEnumerable<string>>>(KeyCsv);
            Assert.That(csv.Count(), Is.EqualTo(3));
            Assert.That(csv.First().Count(), Is.EqualTo(4));
            Assert.That(csv.First().First(), Is.EqualTo("a"));
            Assert.That(csv.First().Skip(1).First(), Is.EqualTo("b"));
            Assert.That(csv.First().Skip(2).First(), Is.EqualTo("c"));
            Assert.That(csv.First().Skip(3).First(), Is.EqualTo("d"));
            Assert.That(csv.Skip(1).First().First(), Is.EqualTo("\""));
            Assert.That(csv.Skip(1).First().Skip(1).First(), Is.EqualTo("\"b\""));
            Assert.That(csv.Skip(1).First().Skip(2).First(), Is.EqualTo("a\""));
            Assert.That(csv.Skip(1).First().Skip(3).First(), Is.EqualTo("1"));
            Assert.That(csv.Skip(2).First().First(), Is.EqualTo("1.8"));
            Assert.That(csv.Skip(2).First().Skip(1).First(), Is.EqualTo("\"g"));
            Assert.That(csv.Skip(2).First().Skip(2).First(), Is.EqualTo(","));
            Assert.That(csv.Skip(2).First().Skip(3).First(), Is.EqualTo("y"));

        }
    }
}