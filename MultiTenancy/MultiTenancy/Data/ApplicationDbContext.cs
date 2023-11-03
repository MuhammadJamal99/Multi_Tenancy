using Microsoft.EntityFrameworkCore;


namespace MultiTenancy.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
}
