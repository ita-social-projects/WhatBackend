using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Visit
{
    public class VisitModel
    {
        [Required]
        [JsonPropertyName("student_id")]
        public virtual long StudentId { get; set; }

        //[Required] //can be null in bd 
        [JsonPropertyName("student_mark")]
        public virtual sbyte? StudentMark { get; set; }

        [Required]
        [JsonPropertyName("presence")]
        public virtual bool Presence { get; set; }

       //[Required] //can be null in bd
        [JsonPropertyName("comment")]
        [StringLength(1024)]
        public virtual string Comment { get; set; }

    }
}
