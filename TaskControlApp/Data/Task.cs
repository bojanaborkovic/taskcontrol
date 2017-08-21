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
    using System.Collections.Generic;
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.Comments = new HashSet<Comment>();
            this.TaskAsigneeHistories = new HashSet<TaskAsigneeHistory>();
            this.TaskStatusHistories = new HashSet<TaskStatusHistory>();
        }
    
        public long Id { get; set; }
        public string Title { get; set; }
        public int IssueType { get; set; }
        public Nullable<long> Asignee { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public Nullable<int> Reporter { get; set; }
        public int Priority { get; set; }
        public long ProjectId { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskAsigneeHistory> TaskAsigneeHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskStatusHistory> TaskStatusHistories { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
