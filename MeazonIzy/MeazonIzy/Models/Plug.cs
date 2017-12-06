using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeazonIzy.Models
{
  public  class Plug
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Mac { get; set; }
        public string Name { get; set; }
      public MyPlugState MyPlugState { get; set; }

    }
}
