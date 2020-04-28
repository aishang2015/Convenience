using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.CapMQ
{
    public enum CapMessageQueryType
    {
        RabbitMQ,
        Kafka,
        AzureServiceBus,
    }
    public enum CapDataBaseType
    {
        SqlServer,
        MySql,
        PostgreSQL,
        InMemory
    }
}
