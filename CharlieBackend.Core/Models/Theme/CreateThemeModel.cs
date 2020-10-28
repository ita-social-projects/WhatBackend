using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Theme
{
    public class CreateThemeModel : ThemeModel
    {
        [Required]
        [JsonIgnore]
        public override long Id { get; set; }
    }
}
