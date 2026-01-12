using Commerce.Application.Interfaces.Out;
using Commerce.Contracts.IntegrationContracts.Orders;

using Azure;
using Azure.Communication.Email;
using Commerce.Application.Interfaces.In;
using Commerce.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;


namespace Commerce.Infrastructure.Email;

public sealed class EmailSender : IEmailSender
{
    private readonly EmailClient _client;
    private readonly EmailOptions _options;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        EmailClient client,
        IOptions<EmailOptions> options,
        ICustomerService customerService,
        IProductService productService,
        ILogger<EmailSender> logger)
    {
        _client = client;
        _options = options.Value;
        _customerService = customerService;
        _productService = productService;
        _logger = logger;
    }

public async Task SendOrderConfirmationEmail(
    Guid customerId,
    Guid orderId,
    IReadOnlyList<OrderProcessedItem> items)
{
    if (items is null || items.Count == 0)
        throw new InvalidOperationException($"Order {orderId} had no items to email.");

    var customer = await _customerService.GetCustomerByIdAsync(customerId)
        ?? throw new InvalidOperationException($"Customer not found: {customerId}");

    if (string.IsNullOrWhiteSpace(customer.Email))
        throw new InvalidOperationException($"Customer {customerId} has no email address.");


    var productNameById = new Dictionary<Guid, string>();

    foreach (var ordered in items)
    {
        if (productNameById.ContainsKey(ordered.ProductId))
            continue;

        var product = await _productService.GetProductByIdAsync(ordered.ProductId);

        var name = product?.Name;
        productNameById[ordered.ProductId] =
            string.IsNullOrWhiteSpace(name) ? ordered.ProductId.ToString() : name!;
    }

    var greetingName = customer.FirstName.Trim();

    var lines = items.Select(i =>
    {
        var qty = i.Qauntity; 
        if (qty <= 0) qty = 1;

        var productName = productNameById.TryGetValue(i.ProductId, out var n) ? n : i.ProductId.ToString();
        return $"{qty}× {productName} @ {i.UnitPrice:C}";
    });

    var greetingLine = string.IsNullOrWhiteSpace(greetingName) ? "<h1>Thank you for shopping with us!</h1>" : $"<h1>Thank you for shopping with us, {greetingName}!</h1>";
    
    var body = $"""
    Thanks for your order!

    Order ID: {orderId}

    Items:
    {string.Join("\n", lines)}
    """;

    var emailMessage = new EmailMessage(
        senderAddress: _options.FromAddress,
        content: new EmailContent("Order confirmation")
        {
            Html = $"""
            <html>
              <body>
                {greetingLine}
                <pre>{body}</pre>
              </body>
            </html>
            """
        },
        recipients: new EmailRecipients(new[]
        {
            new EmailAddress(customer.Email)
        })
    );

    var op = await _client.SendAsync(WaitUntil.Completed, emailMessage);
    _logger.LogInformation("Order confirmation email sent. OrderId={OrderId} CustomerId={CustomerId} OpId={OpId}",
        orderId, customerId, op.Id);
    }

}

