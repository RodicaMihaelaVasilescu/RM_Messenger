//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RM_Messenger.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddRequest
    {
        public int AddRequest_ID { get; set; }
        public string SentBy_User_ID { get; set; }
        public string SentTo_User_ID { get; set; }
        public string Status { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}