using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Panel.Models.Secretary
{
    public class SecretaryViewModel : SecretaryEditViewModel
    {
        public bool IsActive { get; set; }
    }
}
