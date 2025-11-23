namespace InLovingMemory.Model
{
    public class AppConfig
    {
        public MongoDbSettings MongoDbSettings { get; set; }


    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }

}
