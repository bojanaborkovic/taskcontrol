using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskControl.Models
{
  public class ErrorModel
  {
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public string Description { get; set; }
  }
}