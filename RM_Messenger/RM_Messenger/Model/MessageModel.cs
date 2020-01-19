using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RM_Messenger.Model
{
  class MessageModel
  {
    public string SentBy { get; set; }
    public string SentTo { get; set; }
    public string Content { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;
  }
}
