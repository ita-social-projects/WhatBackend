namespace CharlieBackend.Core
{
    public class AzureStorageBlobAccount
    {
        public readonly string connectionString;

        public AzureStorageBlobAccount(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
