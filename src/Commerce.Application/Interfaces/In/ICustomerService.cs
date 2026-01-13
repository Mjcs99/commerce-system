using Commerce.Domain.Entities;

namespace Commerce.Application.Interfaces.In;
public interface ICustomerService
{
    public Task<Customer> GetOrCreateCustomerAsync(string externalCustomerId, string email, string? firstName, string? lastName, CancellationToken ct);
    public Task<Customer> GetCustomerByIdAsync(Guid id, CancellationToken ct);
}