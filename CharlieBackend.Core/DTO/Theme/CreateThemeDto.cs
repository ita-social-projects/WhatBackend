using System.ComponentModel.DataAnnotations;


namespace CharlieBackend.Core.DTO.Theme
{
    public class CreateThemeDto
    {
        [Required]
        public string Name { get; set; }
    }
}
