using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBookingAPI.Models;
using WebBookingAPI.Data;

namespace WebBookingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApiContext _context;
        public HomeController(ApiContext context)
        {
            _context = context;
        }

        // Create/Edit

        [HttpPost]
        public JsonResult CreateEdit(Books book)
        {
            if (book.Id == 0)
            {
                _context.Books.Add(book);
                SaveChanges("Create");
            }
            else
            {
                var _book = _context.Books.Find(book.Id);

                if (book == null)
                    return new JsonResult(NotFound());

                _book = book;

                SaveChanges("Edit");
            }

            _context.SaveChanges();

            return new JsonResult(Ok(book));
        }


        // Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Books.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }


        // Delete 
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Books.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            _context.Books.Remove(result);

            SaveChanges("Delete");

            return new JsonResult(NoContent());
        }

        // Get all
        [HttpGet("/GetAll")]
        public JsonResult GetAll()
        {
            var result = _context.Books.ToList();

           

            return new JsonResult(result);
        }

        // Get all changes
        [HttpGet("/GetAllChanges")]
        public JsonResult GetAllChanges()
        {
            var result = _context.Changes.ToList();
            return new JsonResult(result);
        }

        // Get all changes
        [HttpGet("/GetAllChangesByID")]
        public JsonResult GetAllChangesByID(int id)
        {
            var result = _context.Changes.Find(id);
            return new JsonResult(result);
        }



        private void SaveChanges(string actionName)
        {
           var change = new Changes();
           change.Id = _context.Changes.ToList().Count + 1;
           change.ActionName = actionName;
           change.TimeStamp = DateTime.Now;

            _context.Changes.Add(change);
        }
    }
}
