using Microsoft.EntityFrameworkCore;
using WebBookingAPI.Models;
namespace WebBookingAPI.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Books> Books { get; set; } 
        public DbSet<Changes> Changes { get; set; } 

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }
    }
}
