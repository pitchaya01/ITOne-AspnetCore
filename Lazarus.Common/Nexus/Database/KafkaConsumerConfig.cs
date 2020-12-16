using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lazarus.Common.Nexus.Database
{
    public class KafkaConsumerConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Consumer { get; set; }
        public string GroupId { get; set; }
        public int Offset { get; set; }
        public int Partition { get; set; }
        public string Topic { get; set; }

    }
}
