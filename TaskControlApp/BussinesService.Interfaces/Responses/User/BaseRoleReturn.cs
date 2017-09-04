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
  public class BaseRoleReturn : BasicReturn
  {
    [DataMember]
    public long Id { get; set; }

    [DataMember]
    public string RoleName { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public DateTime? DateCreated { get; set; }
  }

  [DataContract]
  public class SearchRolesReturn : BasicReturn
  {
    [DataMember]
    public List<RoleEntity> Roles { get; set; }

    [DataMember]
    public int RecordCount { get; set; }
  }
}
