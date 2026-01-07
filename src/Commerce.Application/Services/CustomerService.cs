namespace Commerce.Application.Services;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Commerce.Domain.Entities;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> GetOrCreateCustomerAsync(string externalCustomerId, string email, string? firstName, string? lastName)
    {
        var customer = await _customerRepository.GetCustomerByExternalIdAsync(externalCustomerId);
        if (customer == null)
        {
            customer = Customer.Create(externalCustomerId, email, firstName, lastName);
            await _customerRepository.AddCustomerAsync(customer);
        }
        return customer;
        
    }


}