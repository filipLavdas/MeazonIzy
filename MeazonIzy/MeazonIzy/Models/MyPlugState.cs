using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeazonIzy.Models
{
   public class MyPlugState
    {
        public bool? IsOn { get; set; }
        public bool? IsOnline { get; set; }
        public bool? IsScheduleEnabled { get; set; }
        public string Mac { get; set; }
        public int? Number { get; set; }
        public string ReplyTo { get; set; }
        public Schedule Schedule{ get; set; }

        public ObservableCollection<MySchedule> MyScheduleList { get; set; }

    }
}
