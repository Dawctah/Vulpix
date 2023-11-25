using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
{
    [TestClass]
    public class SaveHobbiesToFileCommandTests
    {
        private readonly TestExporter testExporter;

        private readonly SaveHobbiesToFileCommandHandler underTest;

        public SaveHobbiesToFileCommandTests()
        {
            testExporter = new();

            underTest = new(testExporter);
        }

        [TestMethod]
        public async Task ExporterIsUsed()
        {
            await underTest.ExecuteAsync(new(new TestHobbyFile(), "", ""));
            Assert.AreNotEqual(0, testExporter.Calls);
        }
    }
}
