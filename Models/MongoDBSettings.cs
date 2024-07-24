namespace BackEndAPIFondosBTG.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string FondosCollectionName { get; set; } = null!;
        public string TransaccionesCollectionName { get; set; } = null!;
    }
}
