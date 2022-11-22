using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ShoppingApp.Models
{
    public class MyContext: DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<UserList> UserList { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductUserList> ProductUserList { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=ShoppingApp;Trusted_Connection=True");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
    
        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //}

    }


}
