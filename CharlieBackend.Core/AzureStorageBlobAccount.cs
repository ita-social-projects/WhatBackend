namespace CharlieBackend.Core
{
    public class AzureStorageBlobAccount
    {
        public readonly string ConnectionString;

        public AzureStorageBlobAccount(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
