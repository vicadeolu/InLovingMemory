using Microsoft.EntityFrameworkCore;

namespace InLovingMemory.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
