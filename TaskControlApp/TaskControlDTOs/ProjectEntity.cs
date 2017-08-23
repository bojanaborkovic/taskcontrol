using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
  [DataContract]
	public class ProjectEntity
	{
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public long OwnerId { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Owner { get; set; }

    public override string ToString()
    {
      return String.Format("Id={0},Name={1}, OwnerId={2}, Description={3}, Owner={4}", Id, Name, OwnerId, Description, Owner);
    }
  }
}
