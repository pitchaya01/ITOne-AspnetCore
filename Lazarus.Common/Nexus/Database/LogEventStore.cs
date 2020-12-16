using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lazarus.Common.Nexus.Database
{
    public class  LogEventStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AggregateId { get; set; }
         public string EventName { get; set; }
         public string Val { get; set; }
         public string  Status { get; set; }
        public string Message { get; set; }
         public DateTime CreateDate { get; set; }
         public DateTime? CommitDateTime { get; set; }
    }
}
