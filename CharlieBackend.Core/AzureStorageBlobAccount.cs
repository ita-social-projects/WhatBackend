namespace CharlieBackend.Core
{
    public class AzureStorageBlobAccount
    {
        public readonly string ConnectionString;

        public readonly string AccountName;

        public readonly string AccountKey;

        public readonly string EndpointSuffix;

        public readonly string DefaultEndpointsProtocol;

        public AzureStorageBlobAccount(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private void SetBlobAccountFields()
        {
            string[] dividedString = ConnectionString.Split(";");


        }
    }
}
