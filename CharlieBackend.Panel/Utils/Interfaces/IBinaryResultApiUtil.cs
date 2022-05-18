using System.Threading.Tasks;

namespace CharlieBackend.Panel.Utils.Interfaces
{
    public interface IBinaryResultApiUtil
    {
        public Task<byte[]> PostAsync<TPostData>(string url, TPostData postData);

        public Task<byte[]> GetAsync(string url);
    }
}
