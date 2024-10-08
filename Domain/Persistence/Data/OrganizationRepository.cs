using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public  class OrganizationRepository(ChyveContext context)
    {
        private readonly ChyveContext _context = context;
    }
}
