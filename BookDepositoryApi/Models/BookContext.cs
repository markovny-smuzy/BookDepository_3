using Microsoft.EntityFrameworkCore;

namespace BookDepositoryApi.Models
{
    public class BookContext : DbContext
    {
        public DbSet<ConcreteBook> Books { get; set; }

        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
            // Конструктор, который принимает параметры конфигурации
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация модели, если необходимо
            modelBuilder.Entity<ConcreteBook>()
                .HasKey(b => b.ISBN); // Предполагаем, что ISBN является уникальным идентификатором
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=books.db");
        }
    }
}