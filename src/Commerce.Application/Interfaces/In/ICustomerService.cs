using Commerce.Domain.Entities;

namespace Commerce.Application.Interfaces.In;
public interface ICustomerService
{
    // Fix signatures later
    public Task<Customer> GetOrCreateCustomerAsync(string externalCustomerId, string email, string? firstName, string? lastName);
}