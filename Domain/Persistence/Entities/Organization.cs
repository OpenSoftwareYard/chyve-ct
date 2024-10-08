using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities
{
    public class Organization : BaseEntity
    {
        public required string Name { get; set; }

        public required List<string> UserIds { get; set; }
        public required List<Zone> Zones { get; set; }
    }
}
