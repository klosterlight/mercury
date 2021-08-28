namespace Mercury.Common.Settings
{
    public class MongoDbSettings
    {
        // inits => Prevents any new value is assigned
        public string Host { get; init; }
        public int Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}