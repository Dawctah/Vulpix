using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vulpix.Domain.Commands;
using Vulpix.Test.Helpers;
using Vulpix.Test.TestDoubles;

namespace Vulpix.Test.Tests.Commands
{
    [TestClass]
    public class ChangeItemStatusCommandTests
    {
        private readonly TestHobbyFile testHobbyFile;

        private readonly ChangeItemStatusCommandHandler underTest;

        public ChangeItemStatusCommandTests()
        {
            testHobbyFile = new(ItemHelper.GenerateItem("New Item", itemStatus: Domain.Models.ItemStatus.NotStarted));

            underTest = new();
        }

        [TestMethod]
        public async Task ItemIsSaved()
        {
            await underTest.ExecuteAsync(new(testHobbyFile, testHobbyFile.Items.First(), Domain.Models.ItemStatus.Complete));
            Assert.IsTrue(testHobbyFile.UpdatedItems.Any());
        }

        [TestMethod]
        public async Task ListIsOrganized()
        {
            await underTest.ExecuteAsync(new(testHobbyFile, testHobbyFile.Items.First(), Domain.Models.ItemStatus.Complete));
            Assert.IsTrue(testHobbyFile.Organized);
        }

        [TestMethod]
        public async Task StatusIsUpdated()
        {
            await underTest.ExecuteAsync(new(testHobbyFile, testHobbyFile.Items.First(), Domain.Models.ItemStatus.Complete));
            Assert.IsTrue(testHobbyFile.UpdatedItems.First().Status == Domain.Models.ItemStatus.Complete);
        }
    }
}
