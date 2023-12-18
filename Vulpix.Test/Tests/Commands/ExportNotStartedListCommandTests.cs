using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vulpix.Domain.Commands;
using Vulpix.Test.Helpers;
using Vulpix.Test.TestDoubles;

namespace Vulpix.Test.Tests.Commands
{
    [TestClass]
    public class ExportNotStartedListCommandTests
    {
        private readonly TestHobbyFile testHobbyFile;
        private readonly TestExporter testExporter;

        private readonly ExportNotStartedListCommandHandler underTest;

        public ExportNotStartedListCommandTests()
        {
            testHobbyFile = new(ItemHelper.GenerateItem());
            testExporter = new();

            underTest = new(testExporter);
        }

        [TestMethod]
        public async Task ExporterIsUsed()
        {
            await underTest.ExecuteAsync(new("", "", Enumerable.Empty<string>()));
            Assert.AreNotEqual(0, testExporter.Calls);
        }
    }
}
