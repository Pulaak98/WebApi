using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.DatabaseContext
{
    public class WebApiDbContext:DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext>options):base(options)
        {

        }

        public DbSet<User> UserInfo {  get; set; }  
        public DbSet<Product>  Products { get; set; }
    }
}
