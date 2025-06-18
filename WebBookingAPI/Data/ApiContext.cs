using Microsoft.EntityFrameworkCore;
using WebBookingAPI.Models;
namespace WebBookingAPI.Data
{
    public class ApiContext : DbContext
    {
        public virtual DbSet<Books> Books { get; set; } 
        public virtual DbSet<Changes> Changes { get; set; } 

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {

        }
    }
}
