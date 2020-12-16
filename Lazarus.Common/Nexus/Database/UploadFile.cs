using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lazarus.Common.Model
{
    public class FileUpload
    {
        [Key]
        public string Id { get; set; }
        public byte[] File { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}
