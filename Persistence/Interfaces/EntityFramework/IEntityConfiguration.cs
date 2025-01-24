using Microsoft.EntityFrameworkCore;

namespace Persistence.Interfaces.EntityFramework;

public interface IEntityConfiguration
{
    void Configure(ModelBuilder builder);
}