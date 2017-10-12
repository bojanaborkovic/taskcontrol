using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
  [DataContract]
  public class RoleEntity
  {
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Description { get; set; }
  
    public DateTime DateCreated { get; set; }

    [DataMember]
    public string DateTimeString
    {
      get { return this.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.DateCreated = DateTime.Parse(value); }
    }

    [DataMember]
    public List<long> ProjectAccess { get; set; }
  }
}
