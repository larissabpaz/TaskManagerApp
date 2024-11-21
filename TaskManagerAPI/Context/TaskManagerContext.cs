using Microsoft.EntityFrameworkCore;
public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<TaskDto>? Tasks { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<User>()
    //         .HasMany(u => u.Tasks)
    //         .WithOne(t => t.User)
    //         .HasForeignKey(t => t.UserId);
    // }
}
