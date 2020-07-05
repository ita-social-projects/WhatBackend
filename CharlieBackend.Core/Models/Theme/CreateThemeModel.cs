using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Theme
{
    public class CreateThemeModel : ThemeModel
    {
        [JsonIgnore]
        public override long Id { get; set; }
    }
}
