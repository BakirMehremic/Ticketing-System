using Microsoft.EntityFrameworkCore;
using TicketingSys.Models;

namespace TicketingSys.Settings;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Response> Responses { get; set; }
    
    public DbSet<ResponseAttachment> ResponseAttachments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<TicketCategory> TicketCategories { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<UserDepartmentAccess> UserDepartmentAccess { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Id); // Index on Id column

        // Department
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);

            // Required name
            entity.Property(d => d.Name).IsRequired();

            // Unique index on name (case-sensitive)
            entity.HasIndex(d => d.Name).IsUnique();
        });

        // Response
        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(r => r.Id);

            // Relationship: Response -> Ticket
            entity.HasOne(r => r.Ticket)
                .WithMany(t => t.Responses)
                .HasForeignKey(r => r.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Response -> User (sender)
            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure properties
            entity.Property(r => r.Message).IsRequired();
        });

        // ResponseAttachment
        modelBuilder.Entity<ResponseAttachment>(entity =>
        {
            entity.HasKey(ra => ra.Id);

            // Require path to attachment
            entity.Property(ra => ra.Path).IsRequired();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(ra => ra.Id);

            // Mark Title as required
            entity.Property(t => t.Title).IsRequired();

            // Mark Title as required
            entity.Property(t => t.Urgency).IsRequired();

            entity.HasOne(t => t.SubmittedBy)
                .WithMany()
                .HasForeignKey(t => t.SubmittedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Ticket -> AssignedTo (user to which ticket is assigned)
            entity.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationship: Ticket -> TicketCategory
            entity.HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationship: Ticket -> Department
            entity.HasOne(t => t.Department)
                .WithMany()
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketAttachment
            modelBuilder.Entity<TicketAttachment>(entity =>
            {
                // Map to a specific table
                entity.ToTable("TicketAttachments");

                entity.HasKey(x => x.Id);

                // Required Path property
                entity.Property(x => x.Path).IsRequired();
            });

            modelBuilder.Entity<TicketCategory>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).IsRequired();

                entity.HasIndex(x => x.Name).IsUnique();

            });

            modelBuilder.Entity<UserDepartmentAccess>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.User)
                    .WithMany(u => u.DepartmentAccesses)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Department)
                    .WithMany()
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new // add index to userid query and userid + deptid query
                    { x.UserId, x.DepartmentId }).IsUnique(); // prevent duplicate access
            });
        });
    }
}