//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Khruphanth.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_Requisition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_Requisition()
        {
            this.T_RequestList = new HashSet<T_RequestList>();
        }
    
        public string RequisitionID { get; set; }
        public string Re_TeaId { get; set; }
        public string Re_DateRequi { get; set; }
        public string Re_StepID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_RequestList> T_RequestList { get; set; }
        public virtual T_Stap T_Stap { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
