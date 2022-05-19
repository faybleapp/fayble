using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fayble.Domain.Entities
{
    public class FieldLock
    {
        public string Type { get; set; }
        public string Field { get; set; }
        public bool Locked { get; set; }
    }
}
