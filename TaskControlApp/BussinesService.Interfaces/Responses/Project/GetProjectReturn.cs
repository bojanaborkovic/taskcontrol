using BusinessServices.Interfaces.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BussinesService.Interfaces.Responses.Project
{
  [DataContract]
  public class GetProjectReturn : BasicReturn
  {

    [DataMember]
    public List<ProjectEntity> Projects { get; set; }

    [DataMember]
    public int RecordCount { get; set; }

  
  }
}
