using Lazarus.Common.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Domain
{
    [Owned]
    public class MasterValue<T> :ValueObject
    {
        public string Name { get; private set; }
        public T Id { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Id;
        }
        public MasterValue(string name, T value)
        {
            this.Name = name;
            this.Id = value;
        }
        public MasterValue()
        {

        }
    }
    [ComplexType]
    public class MasterIntValue : ValueObject
    {
        public string NAME { get; private set; }
        public int? ID { get; private set; }
        public MasterIntValue()
        {

        }
        public MasterIntValue(string name, int value)
        {
            this.NAME = name;
            this.ID = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return NAME;
            yield return ID;
        }
    }
    [ComplexType]
    public class MasterStringValue : ValueObject
    {
        public MasterStringValue() { }
        public MasterStringValue(string name,string value)
        {
            this.NAME = name;
            this.ID = value;
        }
        public string NAME { get; private set; }
        public string ID { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return NAME;
            yield return ID;
        }
    }
}
