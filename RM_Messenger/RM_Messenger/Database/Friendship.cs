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
    
    public partial class Friendship
    {
        public int Friendship_ID { get; set; }
        public string User_ID { get; set; }
        public string Friend_ID { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
