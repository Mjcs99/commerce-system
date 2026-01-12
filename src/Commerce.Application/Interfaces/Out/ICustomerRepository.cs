namespace Commerce.Application.Interfaces.Out;
using Commerce.Domain.Entities;
public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByExternalIdAsync(string externalCustomerId, CancellationToken ct);
    Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken ct);
    Task AddCustomerAsync(Customer customer, CancellationToken ct);
}