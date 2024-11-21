using Microsoft.EntityFrameworkCore;
public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) {}
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Task>? Tasks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        modelBuilder.Entity<Task>().HasKey(t => t.Id);
        modelBuilder.Entity<Task>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
    }
}
