using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Domain.Models;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
{
    [TestClass]
    public class FinishBookCommandTests
    {
        private readonly FinishBookCommand underTest;

        private readonly TestSaveFile testSaveFile;
        private readonly Book testBook;

        public FinishBookCommandTests()
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
        public void BookIsAddedToFinished()
        {
            testSaveFile.AddCurrentlyReading(testBook);

            underTest.Execute(Context());
            Assert.IsTrue(testSaveFile.GetFinished().Any(), "To Be Read does not contain book.");
        }

        [TestMethod]
        public void NonTargetedBooksRemainInTheCorrectRepository()
        {
            testSaveFile.currentlyReading.SavedItems.Add(testBook);
            testSaveFile.currentlyReading.SavedItems.Add(Book.Create("Another test book"));
            testSaveFile.finished.SavedItems.Add(Book.Create("So many test books"));

            underTest.Execute(Context());
            Assert.IsTrue(testSaveFile.finished.GetAll().Any(), "Currently Reading does not contain books.");
            Assert.IsTrue(testSaveFile.currentlyReading.GetAll().Any(), "To Be Read does not contain books.");
        }

        [TestMethod]
        public void BookFinishDateIsChanged()
        {
            testSaveFile.AddCurrentlyReading(testBook);
            underTest.Execute(Context());
            Assert.AreNotEqual(DateOnly.MinValue, testSaveFile.GetFinished().First().FinishDate, "Finish date was not changed.");
        }

        private FinishBookCommandContext Context() => new(testBook, testSaveFile);
    }
}
