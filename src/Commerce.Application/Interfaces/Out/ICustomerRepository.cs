namespace Commerce.Application.Interfaces.Out;
using Commerce.Domain.Entities;
public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByExternalIdAsync(string externalCustomerId);
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    Task AddCustomerAsync(Customer customer);
    Task<int> SaveChangesAsync();
}