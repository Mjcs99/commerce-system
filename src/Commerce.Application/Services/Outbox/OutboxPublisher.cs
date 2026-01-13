using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Commerce.Application.Exceptions;
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
        if (messages.Count == 0) return 0;

        var now = DateTime.UtcNow;
        var publishedCount = 0;

        foreach (var message in messages)
        {
            stoppingToken.ThrowIfCancellationRequested();

            try
            {
                await _bus.PublishAsync(message.Id, message.Type, message.Payload, stoppingToken);
                publishedCount++;
            }
            catch (OperationCanceledException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                var published = messages.Take(publishedCount).ToList();
                if (publishedCount > 0)
                {
                    _efOutbox.MarkProcessed(published, now);
                    await _unitOfWork.SaveChangesAsync(stoppingToken);
                }

                throw new ServiceBusException(
                    $"Unable to publish outbox message {message.Id} ({message.Type}) to Service Bus.",
                    ex);
            }
        }

        _efOutbox.MarkProcessed(messages, now);
        await _unitOfWork.SaveChangesAsync(stoppingToken);

        return publishedCount;
    }

}