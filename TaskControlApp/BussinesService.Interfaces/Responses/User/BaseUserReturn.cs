using BusinessServices.Interfaces.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BussinesService.Interfaces.Responses.User
{
  [DataContract]
  public class BaseUserReturn : BasicReturn
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

  }

  [DataContract]
  public class SearchUsersReturn : BasicReturn
  {
    [DataMember]
    public List<UserEntity> Users { get; set; }

    [DataMember]
    public int RecordCount { get; set; }
  }

}
