using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MultiTenancy.Data;

public class ApplicationDbContext : DbContext
{
    public Guid TenantId { get; set; }

    private readonly ITenantService _tenantService;

    public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
        TenantId = _tenantService.GetCurrentTenant().TenantId;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? tenantConnectionString = _tenantService.GetConnectionString();
        if (!string.IsNullOrWhiteSpace(tenantConnectionString))
        {
            string dbProvider = _tenantService.GetDatabaseProvider();
            if (string.Equals(dbProvider, "mssql", StringComparison.OrdinalIgnoreCase))
                optionsBuilder.UseSqlServer(tenantConnectionString);
        }
        base.OnConfiguring(optionsBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (EntityEntry<IMultiTenant>? entry in ChangeTracker.Entries<IMultiTenant>().Where(e => e.State == EntityState.Added))
        {
            entry.Entity.TenantId = TenantId;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
