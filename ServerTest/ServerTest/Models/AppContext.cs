using Microsoft.EntityFrameworkCore;

namespace ServerTest.Models;

public class AppContext:DbContext
{
    public DbSet<MyCurrency> currencies;

    public AppContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=../../appCurrency.db");
    }
}