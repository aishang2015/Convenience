namespace Convience.CapMQ
{
    // 消息队列类型
    public enum CapMessageQueryType
    {
        RabbitMQ,
        Kafka,
    }

    // 消息持久化类型
    public enum CapDataBaseType
    {
        SqlServer,
        PostgreSQL,
        InMemory
    }
}
