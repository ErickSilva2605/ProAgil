using Microsoft.EntityFrameworkCore;
using ProAgil.WebAPI.Models;

namespace ProAgil.WebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> context) : base(context)
        {
        }
        public DbSet<Evento> Evento { get; set; }
    }
}