using System;
using ProManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace ProManager.Implementations
{
    public sealed class DesignTimeRepositoryFactory : IDesignTimeDbContextFactory<Repository>
    {
        public Repository CreateDbContext(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentNullException();
            }
            return new Repository(new DbContextOptionsBuilder<Repository>().UseSqlServer(args[0]).Options);
        }
    }
}
