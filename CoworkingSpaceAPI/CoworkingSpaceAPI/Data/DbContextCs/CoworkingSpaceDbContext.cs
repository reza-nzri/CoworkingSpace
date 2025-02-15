using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoworkingSpaceAPI.Models;

public partial class CoworkingSpaceDbContext : IdentityDbContext<ApplicationUserModel>
{
    public CoworkingSpaceDbContext()
    {
    }

    public CoworkingSpaceDbContext(DbContextOptions<CoworkingSpaceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<AddressType> AddressTypes { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }
    public virtual DbSet<CompanyCeo> CompanyCeos { get; set; }
    public virtual DbSet<CompanyEmployee> CompanyEmployees { get; set; }
    public virtual DbSet<Desk> Desks { get; set; }
    public virtual DbSet<Label> Labels { get; set; }
    public virtual DbSet<LabelAssignment> LabelAssignments { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<UserAddress> UserAddresses { get; set; }
    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=CoworkingSpaceDB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__CAA247C8E6D97059");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.HouseNumber)
                .HasMaxLength(10)
                .HasColumnName("house_number");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        modelBuilder.Entity<AddressType>(entity =>
        {
            entity.HasKey(e => e.AddressTypeId).HasName("PK__AddressT__5ABF11E50EED6709");

            entity.ToTable("AddressType");

            entity.HasIndex(e => e.AddressTypeName, "UQ__AddressT__071A958776CC882F").IsUnique();

            entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");
            entity.Property(e => e.AddressTypeName)
                .HasMaxLength(50)
                .HasColumnName("address_type");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__5DE3A5B11355DBAA");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");

            entity.Property(e => e.CancellationReason)
                .HasMaxLength(255)
                .HasColumnName("cancellation_reason");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.Property(e => e.DeskId).HasColumnName("desk_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");

            entity.Property(e => e.TotalCost)
                 .HasColumnType("decimal(10, 2)")
                 .HasColumnName("total_cost");

            entity.Property(e => e.IsCancelled).HasColumnName("is_cancelled");
            entity.Property(e => e.IsCheckedIn).HasColumnName("is_checked_in");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__room_id");

            entity.HasOne(d => d.Desk).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DeskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__desk_id__7A672E12");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__user_id__797309D9");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__3E267235DB003AF5");

            entity.ToTable("Company");

            entity.HasIndex(e => e.RegistrationNumber, "UQ__Company__125DB2A3918EFD1D").IsUnique();

            entity.HasIndex(e => e.TaxId, "UQ__Company__129B8671395BA472").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(100)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasColumnName("contact_phone");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FoundedDate).HasColumnName("founded_date");
            entity.Property(e => e.Industry)
                .HasMaxLength(50)
                .HasColumnName("industry");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.RegistrationNumber)
                .HasMaxLength(50)
                .HasColumnName("registration_number");
            entity.Property(e => e.TaxId)
                .HasMaxLength(50)
                .HasColumnName("tax_id");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");
        });

        modelBuilder.Entity<CompanyAddress>(entity =>
        {
            entity.HasKey(e => e.CompanyAddressId).HasName("PK__CompanyA__5650FC57FCFDFEB6");

            entity.ToTable("CompanyAddress");

            entity.Property(e => e.CompanyAddressId).HasColumnName("company_address_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("is_default");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Address).WithMany(p => p.CompanyAddresses)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__CompanyAd__addre__693CA210");

            entity.HasOne(d => d.AddressType).WithMany(p => p.CompanyAddresses)
                .HasForeignKey(d => d.AddressTypeId)
                .HasConstraintName("FK__CompanyAd__addre__6A30C649");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyAddresses)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__CompanyAd__compa__68487DD7");
        });

        modelBuilder.Entity<CompanyCeo>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.CeoUserId, e.StartDate }).HasName("PK__CompanyC__D8C45EEBF1ACE5BA");

            entity.ToTable("CompanyCEO");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CeoUserId).HasColumnName("ceo_user_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");

            entity.HasOne(d => d.CeoUser).WithMany(p => p.CompanyCeos)
                .HasForeignKey(d => d.CeoUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyCE__ceo_u__6477ECF3");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyCeos)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyCE__compa__6383C8BA");
        });

        modelBuilder.Entity<CompanyEmployee>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.UserId, e.StartDate }).HasName("PK__CompanyE__E51B67242C37EA90");

            entity.ToTable("CompanyEmployee");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyEmployees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyEm__compa__5FB337D6");

            entity.HasOne(d => d.User).WithMany(p => p.CompanyEmployees)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyEm__user___60A75C0F");
        });

        modelBuilder.Entity<Desk>(entity =>
        {
            entity.HasKey(e => e.DeskId).HasName("PK__Desk__8AF10D0469E707D1");

            entity.ToTable("Desk");

            entity.Property(e => e.DeskId).HasColumnName("desk_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .HasColumnName("currency");
            entity.Property(e => e.DeskName)
                .HasMaxLength(100)
                .HasColumnName("desk_name");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property(e => e.Price)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Room).WithMany(p => p.Desks)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Desk__room_id__73BA3083");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.LabelId).HasName("PK__Label__E44FFA585B07B173");

            entity.ToTable("Label");

            entity.HasIndex(e => e.LabelName, "UQ__Label__A74BEA65FF2F052D").IsUnique();

            entity.Property(e => e.LabelId).HasColumnName("label_id");
            entity.Property(e => e.ColorCode)
                .HasMaxLength(7)
                .HasColumnName("color_code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.LabelName)
                .HasMaxLength(50)
                .HasColumnName("label_name");
        });

        modelBuilder.Entity<LabelAssignment>(entity =>
        {
            entity.HasKey(e => e.LabelAssignmentId).HasName("PK__LabelAss__F27DEC24A29D32BB");

            entity.ToTable("LabelAssignment");

            entity.Property(e => e.LabelAssignmentId).HasColumnName("label_assignment_id");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(50)
                .HasColumnName("entity_type");
            entity.Property(e => e.LabelId).HasColumnName("label_id");

            entity.HasOne(d => d.Label).WithMany(p => p.LabelAssignments)
                .HasForeignKey(d => d.LabelId)
                .HasConstraintName("FK__LabelAssi__label__3F466844");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__19675A8AFBEE5EFB");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.CompanyAddressId).HasColumnName("company_address_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("EUR")
                .HasColumnName("currency");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Price)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.RoomName)
                .HasMaxLength(100)
                .HasColumnName("room_name");
            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .HasColumnName("room_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.CompanyAddress).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CompanyAddressId)
                .HasConstraintName("FK__Room__company_ad__70DDC3D8");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.UserAddressId).HasName("PK__UserAddr__FEC0352ECEF2EE95");

            entity.ToTable("UserAddress");

            entity.Property(e => e.UserAddressId).HasColumnName("user_address_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("is_default");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Address).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__UserAddre__addre__5165187F");

            entity.HasOne(d => d.AddressType).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.AddressTypeId)
                .HasConstraintName("FK__UserAddre__addre__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserAddre__user___5070F446");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__UserSess__69B13FDCAFEF904D");

            entity.ToTable("UserSession");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Browser)
                .HasMaxLength(50)
                .HasColumnName("browser");
            entity.Property(e => e.Device)
                .HasMaxLength(100)
                .HasColumnName("device");
            entity.Property(e => e.FailedLoginAttempts)
                .HasDefaultValue(0)
                .HasColumnName("failed_login_attempts");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastIp)
                .HasMaxLength(45)
                .HasColumnName("last_ip");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.LoginAttempts)
                .HasDefaultValue(1)
                .HasColumnName("login_attempts");
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("login_time");
            entity.Property(e => e.LogoutTime)
                .HasColumnType("datetime")
                .HasColumnName("logout_time");
            entity.Property(e => e.OperatingSystem)
                .HasMaxLength(50)
                .HasColumnName("operating_system");
            entity.Property(e => e.SessionDuration)
                .HasComputedColumnSql("(datediff(minute,[login_time],[logout_time]))", false)
                .HasColumnName("session_duration");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSessi__user___59063A47");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public partial class CoworkingSpaceDbContext
{
    private partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Optional custom configurations go here
    }
}