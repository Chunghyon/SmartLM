using FaceDeviceHttpPcServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceDeviceHttpPcServer.Data;

public class FaceDeviceDbContext : DbContext
{
    public FaceDeviceDbContext(DbContextOptions<FaceDeviceDbContext> options)
        : base(options)
    {
    }

    public DbSet<PersonEntity> People => Set<PersonEntity>();
    public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();
    public DbSet<DevicePersonEntity> DevicePeople => Set<DevicePersonEntity>();
    public DbSet<PendingDeleteEntity> PendingDeletes => Set<PendingDeleteEntity>();
    public DbSet<DeletedUserIdEntity> DeletedUserIds => Set<DeletedUserIdEntity>();
    public DbSet<IdentifyRecordEntity> IdentifyRecords => Set<IdentifyRecordEntity>();
    public DbSet<DepartmentEntity> Departments => Set<DepartmentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // device_people composite key
        modelBuilder.Entity<DevicePersonEntity>(e =>
        {
            e.HasKey(x => new { x.DeviceSn, x.UserId });

            e.HasOne(x => x.Device)
                .WithMany(d => d.DevicePeople)
                .HasForeignKey(x => x.DeviceSn)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Person)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // pending_deletes composite key
        modelBuilder.Entity<PendingDeleteEntity>(e =>
        {
            e.HasKey(x => new { x.DeviceSn, x.UserId });

            e.HasOne(x => x.Device)
                .WithMany(d => d.PendingDeletes)
                .HasForeignKey(x => x.DeviceSn)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // identify_records
        modelBuilder.Entity<IdentifyRecordEntity>(e =>
        {
            e.HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceSn)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Optional: case-insensitive comparison helpers can be added later if needed
    }
}
