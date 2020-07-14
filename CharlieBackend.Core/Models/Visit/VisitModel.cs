using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Visit
{
    public class VisitModel
    {
        [Required]
        [JsonPropertyName("student_id")]
        public virtual long StudentId { get; set; }

        [Required]
        [JsonPropertyName("student_mark")]
        public virtual sbyte? StudentMark { get; set; }

        [Required]
        [JsonPropertyName("presence")]
        public virtual bool Presence { get; set; }

        [Required]
        [JsonPropertyName("comment")]
        public virtual string Comment { get; set; }

    }
}
