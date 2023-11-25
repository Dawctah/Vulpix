using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Test.Helpers;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
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
