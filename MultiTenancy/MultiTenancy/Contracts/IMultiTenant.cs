namespace MultiTenancy.Contracts;

public interface IMultiTenant
{
    Guid? TenantId { get; set; }
}
