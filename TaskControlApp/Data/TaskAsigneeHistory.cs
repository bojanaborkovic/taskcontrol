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
    
    public partial class TaskAsigneeHistory
    {
        public long Id { get; set; }
        public long AsigneeBefore { get; set; }
        public long AsigneeAfter { get; set; }
        public System.DateTime ChangeDate { get; set; }
        public long TaskId { get; set; }
    
        public virtual Task Task { get; set; }
    }
}
