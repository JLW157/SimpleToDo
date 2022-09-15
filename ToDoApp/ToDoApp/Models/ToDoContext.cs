using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> opts) : base(opts) { }
     
        
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
