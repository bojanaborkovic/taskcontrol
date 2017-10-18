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
  public class ProjectNotesReturn : BasicReturn
  {

    [DataMember]
    public List<Note> Notes { get; set; }

    [DataMember]
    public int RecordCount { get; set; }


  }

  [DataContract]
  public class Note
  {
    [DataMember]
    public long NoteId { get; set; }

    [DataMember]
    public string Content { get; set; }

    [DataMember]

    public long AuthorId { get; set; }

    [DataMember]
    public long ProjectId { get; set; }

    public DateTime DateCreated { get; set; }

    [DataMember]
    public string AuthorName { get; set; }

    [DataMember]
    public string ProjectName { get; set; }

    [DataMember]
    public string DateCreatedString
    {
      get { return this.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.DateCreated = DateTime.Parse(value); }
    }


  }
}
