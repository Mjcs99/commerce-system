namespace Commerce.Application.Services;

using Commerce.Application.Exceptions;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Commerce.Domain.Entities;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Customer> GetOrCreateCustomerAsync(string externalCustomerId, string email, string? firstName, string? lastName, CancellationToken ct)
    {
        var customer = await _customerRepository.GetCustomerByExternalIdAsync(externalCustomerId, ct);
        if (customer == null)
        {
            customer = Customer.Create(externalCustomerId, email, firstName, lastName);
            await _customerRepository.AddCustomerAsync(customer, ct);
        }
        if (customer.FirstName != firstName || customer.LastName != lastName || customer.Email != email)
        {
            customer.UpdateDetails(email, firstName, lastName);
        }
        await _unitOfWork.SaveChangesAsync(ct);
        return customer;
    }

    public async Task<Customer> GetCustomerByIdAsync(Guid customerId, CancellationToken ct){
        return await _customerRepository.GetCustomerByIdAsync(customerId, ct) ?? throw new NotFoundException($"Customer not found - {customerId}");
    }

}