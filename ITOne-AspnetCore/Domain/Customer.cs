using Lazarus.Common.Domain.Seedwork;
using Shared.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Domain
{
    public class Customer: Entity,IAggregateRoot
    {
        public Customer()
        {
            this.AggregateId = Guid.NewGuid();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AggregateId {  get; private set; }
        public string Name {  get; private set; }
        public virtual List<Address> Addresses {  get; private set; }

        public static Customer Create(string name)
        {
            var c = new Customer();
            c.Name = name;
          //  c.AddDomainEvent(new CustomerCreatedEvent() { Name = c.Name });
            return c;
        }
        public void UpdateName(string name)
        {
            this.Name = name;
            this.AddDomainEvent(new CustomerCreatedEvent() { Name = "AAA" });
        }
        public void AddAddress(string addrNo)
        {
            var a= Address.AddAddr(addrNo,this.AggregateId);
            this.Addresses.Add(a);
        }
    }
}
