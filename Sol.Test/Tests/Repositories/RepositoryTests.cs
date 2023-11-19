using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Test.Tests.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        private readonly Repository<Book> underTest;

        public RepositoryTests()
        {
            underTest = new();
        }

        [TestMethod]
        public void ItemsAreAdded()
        {
            underTest.Insert(Book.Create("Test Book"));
            Assert.IsTrue(underTest.GetAll().Any(), "Repository does not contain item");
        }

        [TestMethod]
        public void ItemsAreRemoved()
        {
            var book = Book.Create("Test book");
            underTest.Insert(book);
            underTest.Remove(book);

            Assert.IsFalse(underTest.GetAll().Any(), "Repository contains item");
        }

        [TestMethod]
        public void ItemsAreUpdated()
        {
            var book = Book.Create("So much testing");
            underTest.Insert(book);

            var newBook = book.Start();

            underTest.Update(newBook);
        }
    }
}
