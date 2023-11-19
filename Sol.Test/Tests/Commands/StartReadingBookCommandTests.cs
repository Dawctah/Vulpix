using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Domain.Models;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
{
    [TestClass]
    public class StartReadingBookCommandTests
    {
        private readonly StartReadingBookCommand underTest;

        private readonly TestSaveFile testSaveFile;
        private readonly Book testBook;

        public StartReadingBookCommandTests()
        {
            underTest = new StartReadingBookCommand();

            testBook = Book.Create("Test Title");
            testSaveFile = new TestSaveFile();
        }

        [TestMethod]
        public void BookIsRemovedFromToBeRead()
        {
            testSaveFile.AddToBeRead(testBook);

            underTest.Execute(Context());
            Assert.IsFalse(testSaveFile.GetToBeRead().Any(), "To Be Read contains book.");
        }

        [TestMethod]
        public void BookIsAddedToCurrentlyReading()
        {
            testSaveFile.AddToBeRead(testBook);

            underTest.Execute(Context());
            Assert.IsTrue(testSaveFile.GetCurrentlyReading().Any(), "Currently Reading does not contain book.");
        }

        [TestMethod]
        public void NonTargetedBooksRemainInTheCorrectRepository()
        {
            testSaveFile.toBeRead.SavedItems.Add(testBook);
            testSaveFile.currentlyReading.SavedItems.Add(Book.Create("So many test books"));
            testSaveFile.toBeRead.SavedItems.Add(Book.Create("Another test book"));

            underTest.Execute(Context());
            Assert.IsTrue(testSaveFile.toBeRead.GetAll().Any(), "To Be Read contains book.");
            Assert.IsTrue(testSaveFile.currentlyReading.GetAll().Any(), "Currently Reading does not contain book.");
        }

        [TestMethod]
        public void BookStartDateIsChanged()
        {
            testSaveFile.AddToBeRead(testBook);
            underTest.Execute(Context());
            Assert.AreNotEqual(DateOnly.MinValue, testSaveFile.currentlyReading.GetAll().First().StartDate, "Start date was not changed.");
        }

        private StartReadingBookCommandContext Context() => new(testBook, testSaveFile);
    }
}