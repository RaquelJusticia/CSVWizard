using System.IO;
using TechTalk.SpecFlow;

namespace Specs.CSVWizard
{
    [Binding]
    public class BeforeScenarioHooks
    {
         [BeforeScenario("requiresClean")]
         public void DeleteFile()
         {
             if (File.Exists("file.csv"))
             {
                 File.Delete("file.csv");
             }
         }
    }
}