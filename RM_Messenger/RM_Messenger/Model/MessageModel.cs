﻿using System;
using System.Windows.Documents;

namespace RM_Messenger.Model
{
  class MessageModel
  {
    public string SentBy { get; set; }
    public string ToolTip { get; set; }
    public string SentTo { get; set; }
    public DateTime Date { get; set; }
    public FlowDocument Content { get; set; }
  }
}
