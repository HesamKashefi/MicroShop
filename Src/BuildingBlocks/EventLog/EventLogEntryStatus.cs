namespace EventLog;

public enum EventLogEntryStatus : byte
{
    NotPublished = 0,
    InProgress,
    Published,
    PublishFailed
}