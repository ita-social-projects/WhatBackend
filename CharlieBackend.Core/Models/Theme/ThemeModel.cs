using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Theme
{
    public class ThemeModel
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual long Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
    }
}
