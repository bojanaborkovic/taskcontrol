using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TaskControlDTOs
{
  public class UserEntity
  {
    public long Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
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
}
