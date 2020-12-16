using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Model
{
    public class LogMessage
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime CreateDate { get; set; }
        public string StackTrace { get; set; }
        public string CreateBy { get; set; }
        public string Token { get; set; }
        public string Module { get; set; }
        public string Domain { get; set; }
        public TimeSpan? ExecutionTime { get; set; }

    }
}
