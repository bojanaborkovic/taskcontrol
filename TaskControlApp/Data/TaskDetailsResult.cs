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
    
    public partial class TaskDetailsResult
    {
        public long TaskId { get; set; }
        public string Title { get; set; }
        public string Asignee { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string IssueType { get; set; }
        public string Priority { get; set; }
        public string TaskStatus { get; set; }
        public string Reporter { get; set; }
        public string Project { get; set; }
    }
}
