using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Commands;
using Sol.Test.TestDoubles;

namespace Sol.Test.Tests.Commands
{
    [TestClass]
    public class CreateBookCommandTests
    {
        private readonly TestSaveFile testSaveFile;

        private readonly CreateBookCommand underTest;

        public CreateBookCommandTests()
        {
            testSaveFile = new();
            underTest = new();
        }

        [TestMethod]
        public void BookIsInserted()
        {
            underTest.Execute(new("Test title", testSaveFile));
            Assert.IsTrue(testSaveFile.GetToBeRead().Any(), "Book was not saved in repository.");
        }

        [TestMethod]
        public void InvalidBooksAreNotAdded()
        {
            underTest.Execute(Context());
            underTest.Execute(Context("Test title"));
            Assert.IsFalse(testSaveFile.GetToBeRead().Any(), "Book was added when it is invalid.");
        }

        private static CreateBookCommandContext Context(string title = "") => new(title, new TestSaveFile());
    }
}
