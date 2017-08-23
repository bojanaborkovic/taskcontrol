using BusinessServices.Interfaces.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BussinesService.Interfaces.Responses.Project
{
  [DataContract]
  public class BaseProjectReturn : BasicReturn
  {
    [DataMember]
    public long ProjectId { get; set; }

  }
}
