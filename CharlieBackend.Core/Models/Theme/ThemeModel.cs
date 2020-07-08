using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Theme
{
    public class ThemeModel
    {
        [JsonIgnore]
        public virtual long Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
    }
}
