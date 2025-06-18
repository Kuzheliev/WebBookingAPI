using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using WebBookingAPI;
using WebBookingAPI.Controllers;
using WebBookingAPI.Data;
using WebBookingAPI.Models;

namespace WebApiTest
{
    

    public class HomeControllerTest
    {
        private ILogger logger = new FileLogger();

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

            var mockContext = new Mock<ApiContext>(new DbContextOptions<ApiContext>());
            mockContext.Setup(c => c.Books).Returns(mockSet.Object);

            var controller = new HomeController(mockContext.Object, logger);

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
    }
}