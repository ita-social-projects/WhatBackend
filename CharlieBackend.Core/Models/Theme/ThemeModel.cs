using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Theme
{
    public class ThemeModel
    {
        [Required]
        [JsonIgnore]
        public virtual long Id { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }
    }
}
