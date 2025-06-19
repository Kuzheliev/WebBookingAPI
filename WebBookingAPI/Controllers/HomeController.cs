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
        private readonly ILogger _fileLogger;
        private readonly IConfiguration _configuration;
        public HomeController(ApiContext context, ILogger logger, IConfiguration configuration)
        {
            _context = context;
            _fileLogger = logger;
            _configuration = configuration;
        }   

        // Create/Edit

        [HttpPost]
        public JsonResult CreateEdit(Books book)
        {
            if (_context.Books == null || _context == null) return new JsonResult(NoContent());

            if (book.Id == 0)
            {
                _context.Books.Add(book);
                SaveChanges(GetActionName("Create"));
            }
            else
            {
                var _book = _context.Books.Find(book.Id);

                if (book == null)
                    return new JsonResult(NotFound());

                _book = book;

                SaveChanges(GetActionName("Edit"));
            }

            _context.SaveChanges();

            _fileLogger.Log("CreateEdit called in HomeController", "specific name");

            return new JsonResult(Ok(book));
        }


        // Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            if (_context.Books == null || _context == null) return new JsonResult(NoContent());

            var result = _context.Books.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            _fileLogger.Log("Get called in HomeController", "");

            return new JsonResult(Ok(result));
        }


        // Delete 
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            if (_context.Books == null || _context == null) return new JsonResult(NoContent());

            var result = _context.Books.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            _context.Books.Remove(result);

            SaveChanges(GetActionName("Delete"));

            _fileLogger.Log("Delete called in HomeController", "");

            return new JsonResult(NoContent());
        }

        // Get all
        [HttpGet("/GetAll")]
        public JsonResult GetAll()
        {
            if (_context.Books == null || _context == null) return new JsonResult(NoContent());

            var result = _context.Books.ToList();


            _fileLogger.Log("Get All called in HomeController", "");

            return new JsonResult(result);
        }

        // Get all changes
        [HttpGet("/GetAllChanges")]
        public JsonResult GetAllChanges()
        {
            if(_context.Changes == null || _context == null) return new JsonResult(NoContent());

            var result = _context.Changes.ToList();

            _fileLogger.Log("Get All Changes called in HomeController", "");

            return new JsonResult(result);
        }

        // Get all changes
        [HttpGet("/GetChangesByID")]
        public JsonResult GetChangesByID(int id)
        {
            if (_context.Changes == null || _context == null) return new JsonResult(NoContent());

            var result = _context.Changes.Find(id);

            _fileLogger.Log("Get All Changes By ID called in HomeController", "");

            return new JsonResult(result);
        }



        private void SaveChanges(string actionName)
        {
            var change = new Changes
            {
                ActionName = actionName,
                TimeStamp = DateTime.Now
            };

            _context.Changes.Add(change);
            _context.SaveChanges();

        }

        private string GetActionName(string key)
        {
            return _configuration[$"ChangeActions:{key}"] ?? key;
        }
    }
}
