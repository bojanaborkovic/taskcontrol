using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Responses
{
  [DataContract]
  public class BasicReturn
  {
    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public string StatusCode { get; set; }

    public override string ToString()
    {
      return String.Format("StatusCode={0},ErrorMessage={1}", StatusCode, ErrorMessage);
    }
  }
}
