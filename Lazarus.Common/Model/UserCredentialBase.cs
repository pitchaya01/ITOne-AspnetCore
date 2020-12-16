using Lazarus.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Model
{
    public class ReceiverCredential
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Token { get; set; }
    }
    public class CustomerCredential
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        
    }
    public class UserCredential
    {
        public UserCredential()
        {
            this.Roles = new List<string>();
        }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public Guid HotelId { get; set; }
        public Guid? RoomId { get; set; }
        public List<string> Roles { get; set; }
   
    }

    


}
