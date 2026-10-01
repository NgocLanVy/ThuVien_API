using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI_simple.Models.Domain
{
    public class Image
    {
        public int Id { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile File { get; set; }

        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}