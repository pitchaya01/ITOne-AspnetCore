using Lazarus.Common.Domain.Seedwork;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITOne_AspnetCore.Domain
{
    public class Address:Entity
    {
        public Address()
        {
            this.AggregateId = Guid.NewGuid();
        }
        
        public static Address AddAddr(string addrNo,Guid customerId)
        {
            var a = new Address();
            a.AddrNo = addrNo;
            a.CustomerId = customerId;
            return a;
        }
        public void UpdateAddr(string addrNo)
        {
            this.AddrNo = addrNo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AggregateId {  get; private set; }
        public string AddrNo {  get; private set; }
        public Guid CustomerId {  get; private set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer {  get; private set; }
    }
}
