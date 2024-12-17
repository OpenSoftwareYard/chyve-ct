using System;
using Persistence.Entities;

namespace ChyveClient;

public interface IServiceConfigGenerator
{
    Task<string> GenerateConfig(Service service);
}
