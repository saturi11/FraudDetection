using FraudDetection.BuildingBlocks.Domain;

namespace FraudDetection.Infrastructure.Persistence.Outbox;

public class OutboxEvent
{
    public Guid Id { get; private set; }
    public string EventType { get; private set; } = default!;
    public string Payload { get; private set; } = default!;
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public int RetryCount { get; private set; }


    private OutboxEvent() { }


    public OutboxEvent(string eventType, string payload, DateTime ocurredOn)
    {
        Id = Guid.NewGuid();
        EventType = eventType;
        Payload = payload;
        OccurredOn = ocurredOn;
        RetryCount = 0;
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }

    public void IncrementRetry()
    {
        RetryCount++;
    }
}