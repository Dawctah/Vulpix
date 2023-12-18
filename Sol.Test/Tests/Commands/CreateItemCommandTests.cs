using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Domain.Models;
using Sol.Test.Helpers;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
{
    [TestClass]
    public class CreateItemCommandTests
    {
        private readonly TestHobbyFile testHobbyFile;
        private readonly int numberOfItemsToStartWith = 3;

        private readonly CreateItemCommandHandler underTest;

        public CreateItemCommandTests()
        {
            testHobbyFile = new(ItemHelper.GenerateMultiple(numberOfItemsToStartWith));

            underTest = new();
        }

        [TestMethod]
        public async Task ItemIsSaved()
        {
            await underTest.ExecuteAsync(new(testHobbyFile, "TestItemName", HobbyType.BookClub));
            Assert.IsTrue(testHobbyFile.InsertedItems.Any(), "Item was not inserted into file.");
        }

        [TestMethod]
        public async Task IndexIsAccurate()
        {
            await underTest.ExecuteAsync(new(testHobbyFile, "TestItemName", HobbyType.BookClub));
            Assert.AreEqual(testHobbyFile.InsertedItems.First().Index, numberOfItemsToStartWith + 1);
        }
    }
}
