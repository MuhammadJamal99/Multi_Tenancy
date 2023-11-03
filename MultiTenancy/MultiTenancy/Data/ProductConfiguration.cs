using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenancy.Constants;

namespace MultiTenancy.Data;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public Guid TenantId { get; set; }

    private readonly ITenantService _tenantService;

    public ProductConfiguration(ITenantService tenantService)
    {
        _tenantService = tenantService;
        TenantId = _tenantService.GetCurrentTenant().TenantId;
    }
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(MulitiTenancyDbProperties.DbTablePrefix + "Product", MulitiTenancyDbProperties.DbSchema);

        builder.HasQueryFilter(p => p.TenantId == TenantId);
    }
}
