//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    
    public partial class GetTaskResult
    {
        public long TaskId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int IssueType { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public long ProjectId { get; set; }
        public string Reporter { get; set; }
        public string Asignee { get; set; }
        public long ReporterId { get; set; }
        public long AsigneeId { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
    }
}
