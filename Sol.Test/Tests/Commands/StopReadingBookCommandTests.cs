using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Domain.Models;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
{
    [TestClass]
    public class StopReadingBookCommandTests
    {
        private readonly StopReadingBookCommand underTest;

        private readonly TestSaveFile testSaveFile;
        private readonly Book testBook;

        public StopReadingBookCommandTests()
        {
            underTest = new();
            testSaveFile = new();
            testBook = Book.Create("My test book <3").Start();
        }

        [TestMethod]
        public void BookIsRemovedFromCurrentlyReading()
        {
            testSaveFile.AddCurrentlyReading(testBook);

            underTest.Execute(Context());
            Assert.IsFalse(testSaveFile.GetCurrentlyReading().Any(), "Currently Reading contains book.");
        }

        [TestMethod]
        public void BookIsAddedToToBeRead()
        {
            testSaveFile.AddCurrentlyReading(testBook);

            underTest.Execute(Context());
            Assert.IsTrue(testSaveFile.GetToBeRead().Any(), "To Be Read does not contain book.");
        }

        [TestMethod]
        public void NonTargetedBooksRemainInTheCorrectRepository()
        {
            testSaveFile.currentlyReading.SavedItems.Add(testBook);
            testSaveFile.currentlyReading.SavedItems.Add(Book.Create("So many test books"));
            testSaveFile.toBeRead.SavedItems.Add(Book.Create("Another test book"));

            underTest.Execute(Context());
            Assert.IsTrue(testSaveFile.currentlyReading.GetAll().Any(), "Currently Reading does not contain books.");
            Assert.IsTrue(testSaveFile.toBeRead.GetAll().Any(), "To Be Read does not contain books.");
        }

        [TestMethod]
        public void BookStartDateIsChanged()
        {
            testSaveFile.AddCurrentlyReading(testBook);
            underTest.Execute(Context());
            Assert.AreEqual(DateOnly.MinValue, testSaveFile.GetToBeRead().First().StartDate, "Start date was not changed.");
        }

        private StopReadingBookCommandContext Context() => new(testBook, testSaveFile);
    }
}
