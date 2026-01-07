namespace Commerce.Infrastructure.Repositories;
using Commerce.Infrastructure.Persistence;
using Commerce.Application.Interfaces.Out;
using System.Threading.Tasks;
using System;
using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class EfCustomerRepository : ICustomerRepository
{
    private readonly CommerceDbContext _db;

    public EfCustomerRepository(CommerceDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
         _db.Customer.Add(customer);
        await _db.SaveChangesAsync();
    }

    public async Task<Customer?> GetCustomerByExternalIdAsync(string externalCustomerId)
        => await _db.Customer.SingleOrDefaultAsync(c => c.ExternalUserId == externalCustomerId);
   
}
