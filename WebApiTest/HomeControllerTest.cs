using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Channels;
using WebBookingAPI;
using WebBookingAPI.Controllers;
using WebBookingAPI.Data;
using WebBookingAPI.Models;

namespace WebApiTest
{
    

    public class HomeControllerTest
    {
        private ILogger logger = new FileLogger();
        private readonly Mock<IConfiguration> _mockConfig = new Mock<IConfiguration>();

        [Fact]
        public void Call_CreateEdit_get_Okresponse_and_createdBook()
        {
            // Arrange
            var books = new List<Books>
            {
                new Books { Id = 1, _name = "Alice", _autor = "test", _price = 12, _year = "2000" },
                new Books { Id = 2, _name = "Bob", _autor = "test2", _price = 15, _year = "1999" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Books>>();

            mockSet.As<IQueryable<Books>>().Setup(m => m.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Books>>().Setup(m => m.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Books>>().Setup(m => m.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Books>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

            var changes = new List<Changes>
            {
                new Changes { Id = 1, ActionName = "Create", TimeStamp = DateTime.Now }
            }.AsQueryable();

            var mockChangesSet = new Mock<DbSet<Changes>>();
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.Provider).Returns(changes.Provider);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.Expression).Returns(changes.Expression);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.ElementType).Returns(changes.ElementType);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.GetEnumerator()).Returns(changes.GetEnumerator());


            var mockContext = new Mock<ApiContext>(new DbContextOptions<ApiContext>());
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);
            mockContext.Setup(c => c.Changes).Returns(mockChangesSet.Object);

            var controller = new HomeController(mockContext.Object, logger, _mockConfig.Object);

            var book = new Books();
            book._price = 12;
            book._autor = "test";
            book._year = "1999";
            book._name = "test";

            // Act
            var result = controller.CreateEdit(book);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(jsonResult.Value);

            var book_response = Assert.IsType<Books>(okResult.Value);

            Assert.Equal(0 , book_response.Id);
            Assert.Equal("1999", book_response._year);
            Assert.Equal(12, book_response._price);
            Assert.Equal("test", book_response._name);
            Assert.Equal("test", book_response._autor);
        }

        [Fact]
        public void Call_Get_get_Okresponse_and_Book()
        {
            // Arrange
            var books = new List<Books>
            {
                new Books { Id = 1, _name = "Alice", _autor = "test", _price = 12, _year = "2000" },
                new Books { Id = 2, _name = "Bob", _autor = "test2", _price = 15, _year = "1999" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Books>>();

            mockSet.As<IQueryable<Books>>().Setup(m => m.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Books>>().Setup(m => m.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Books>>().Setup(m => m.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Books>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

            var changes = new List<Changes>
            {
                new Changes { Id = 1, ActionName = "Create", TimeStamp = DateTime.Now }
            }.AsQueryable();

            var mockChangesSet = new Mock<DbSet<Changes>>();
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.Provider).Returns(changes.Provider);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.Expression).Returns(changes.Expression);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.ElementType).Returns(changes.ElementType);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.GetEnumerator()).Returns(changes.GetEnumerator());

            var mockContext = new Mock<ApiContext>(new DbContextOptions<ApiContext>());
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);
            mockContext.Setup(c => c.Changes).Returns(mockChangesSet.Object);

            var controller = new HomeController(mockContext.Object, logger, _mockConfig.Object);

            var book = new Books();
            book._price = 12;
            book._autor = "test";
            book._year = "1999";
            book._name = "test";

            // Act
            var result = controller.GetAll();

            // Assert
            var viewResult = Assert.IsType<JsonResult>(result);

            // Extract the model (should be List<Book>)
            var model = Assert.IsAssignableFrom<List<Books>>(viewResult.Value);

            Assert.Equal(2, model.Count);
            Assert.Equal("Alice", model[0]._name);
            Assert.Equal("test", model[0]._autor);
            Assert.Equal(12, model[0]._price);
            Assert.Equal("2000", model[0]._year);
            Assert.Equal("Bob", model[1]._name);
            Assert.Equal("test2", model[1]._autor);
            Assert.Equal(15, model[1]._price);
            Assert.Equal("1999", model[1]._year);
        }

        [Fact]
        public void Call_GetAllChanges_get_Okresponse_and_Changes()
        {
            // Arrange
            var books = new List<Books>
            {
                new Books { Id = 1, _name = "Alice", _autor = "test", _price = 12, _year = "2000" },
                new Books { Id = 2, _name = "Bob", _autor = "test2", _price = 15, _year = "1999" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Books>>();

            mockSet.As<IQueryable<Books>>().Setup(m => m.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Books>>().Setup(m => m.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Books>>().Setup(m => m.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Books>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());


            var changesList = new List<Changes>();

            var mockChangesSet = new Mock<DbSet<Changes>>();

            mockChangesSet.Setup(m => m.Add(It.IsAny<Changes>()))
                .Callback<Changes>(c => changesList.Add(c));

            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.Provider).Returns(() => changesList.AsQueryable().Provider);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.Expression).Returns(() => changesList.AsQueryable().Expression);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.ElementType).Returns(() => changesList.AsQueryable().ElementType);
            mockChangesSet.As<IQueryable<Changes>>().Setup(m => m.GetEnumerator()).Returns(() => changesList.AsQueryable().GetEnumerator());


            var mockContext = new Mock<ApiContext>(new DbContextOptions<ApiContext>());
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);
            mockContext.Setup(c => c.Changes).Returns(mockChangesSet.Object);

            var controller = new HomeController(mockContext.Object, logger, _mockConfig.Object);

            var book = new Books();
            book.Id = 0;
            book._price = 12;
            book._autor = "test";
            book._year = "1999";
            book._name = "test";

            var book_second = new Books();
            book_second.Id = 0;
            book._price = 11;
            book._autor = "test2";
            book._year = "2009";
            book._name = "test2";

            // Act
            var result = controller.GetAll();
             controller.CreateEdit(book);
            book.Id = 1;
            controller.CreateEdit(book_second);
            book_second.Id = 2;
            result = controller.GetAllChanges();

            // Assert
            var viewResult = Assert.IsType<JsonResult>(result);

            // Extract the model (should be List<Book>)
            var model = Assert.IsAssignableFrom<List<Changes>>(viewResult.Value);

            Assert.Equal(2, model.Count);
            Assert.Contains(model, c => c.ActionName == "Create");
            Assert.All(model, c => Assert.True(c.TimeStamp <= DateTime.Now && c.TimeStamp != default));
        }
    }
}