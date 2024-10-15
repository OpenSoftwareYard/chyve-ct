using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NodeService(IGenericRepository<Node> repository, IMapper mapper) : GenericService<Node, NodeDTO>(repository, mapper), INodeService
    {
    }
}
