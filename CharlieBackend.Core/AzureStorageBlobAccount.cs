using System;
using System.Collections.Generic;
using System.Text;

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
