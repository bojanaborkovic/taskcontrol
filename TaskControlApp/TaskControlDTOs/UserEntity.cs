using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TaskControlDTOs
{
  [DataContract]
  public class UserEntity
  {
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string UserName { get; set; }
    [DataMember]
    public string FirstName { get; set; }
    [DataMember]
    public string LastName { get; set; }
    [DataMember]
    public string Email { get; set; }
    [DataMember]
    public string PhoneNumber { get; set; }
    [DataMember]
    public string Password { get; set; }
    [DataMember]
    public string RoleName { get; set; }

    public string UserAsJson()
    {
      return new JavaScriptSerializer().Serialize(GetType().GetProperties());
    }

    public override string ToString()
    {
      return base.ToString();
    }
  }

  [DataContract]
  public class UserInRoleEntity 
  {
    [DataMember]
    public long RoleId { get; set; }

    [DataMember]
    public long UserId { get; set; }
  }
}
