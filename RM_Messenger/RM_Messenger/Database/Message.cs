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
    
    public partial class Message
    {
        public int Message_ID { get; set; }
        public string SentBy_User_ID { get; set; }
        public string SentTo_User_ID { get; set; }
        public string Text { get; set; }
        public string FontStyle { get; set; }
        public string FontSize { get; set; }
        public Nullable<bool> Bold { get; set; }
        public Nullable<bool> Italic { get; set; }
        public Nullable<bool> Underline { get; set; }
        public string Color { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
