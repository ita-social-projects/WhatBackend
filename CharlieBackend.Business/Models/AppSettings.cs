namespace CharlieBackend.Business.Models
{
    public class AppSettings
    {
        public static string Url { get; set; } 
        public static string Key { get; set; }
        public static string Name { get; set; }

        public static void GetData(string url, string key, string name)
        {
            Url = url;
            Key = key;
            Name = name;
        }
    }

}