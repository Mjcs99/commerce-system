using System.ComponentModel;
using Commerce.Application.Interfaces.In.Outbox;
using Commerce.Application.Interfaces.Out;

namespace Commerce.Application.Services.Outbox;

public sealed class OutboxPublisher : IOutboxPublisher
{
    private readonly IOutbox _efOutbox;
    private readonly IMessageBus _bus;
    private readonly IUnitOfWork _unitOfWork;

    public OutboxPublisher(IOutbox efOutbox, IMessageBus bus, IUnitOfWork unitOfWork)
    {
        _efOutbox = efOutbox;
        _bus = bus;
        _unitOfWork = unitOfWork;
    }
    public async Task<int> PublishPendingAsync(CancellationToken stoppingToken)
    {
        var messages = await _efOutbox.GetUnprocessedAsync(10, stoppingToken);
        foreach (var message in messages) await _bus.PublishAsync(message.Type, message.Payload, stoppingToken);
        _efOutbox.MarkProcessedAsync(messages, DateTime.UtcNow);
        await _unitOfWork.SaveChangesAsync(stoppingToken);
        return messages.Count;
    }
}