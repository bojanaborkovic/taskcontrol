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
  public class BaseProjectReturn : BasicReturn
  {
    [DataMember]
    public long Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public long OwnerId { get; set; }

    [DataMember]
    public long Owner { get; set; }

  }

  [DataContract]
  public class ProjectStatisticsReturn : BasicReturn
  {
    [DataMember]
    public List<TaskEntity> Tasks { get; set; }

    [DataMember] 
    public int RecordCount { get; set; }
  }
}
