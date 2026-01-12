using Commerce.Contracts.IntegrationContracts.Orders;
namespace Commerce.Application.Interfaces.Out;

public interface IEmailSender
{
    public Task SendOrderConfirmationEmail(Guid customerId, Guid orderId, IReadOnlyList<OrderProcessedItem> items, CancellationToken ct);
}
