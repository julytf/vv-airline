using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class AppDBContext : IdentityDbContext<User>
{
    private const string connectionString = @"
        Server=localhost;
        Database=vv_airline;
        User ID=admin;
        Password=password;
        TrustServerCertificate=True;
        ";
    IConfiguration _configuration;
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration)
    : base(options)
    {
        _configuration = configuration;
    }
    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning)
                .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Debug)
                .AddConsole();
        });

    public virtual DbSet<Aircraft> Aircrafts { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<AisleCol> AisleCols { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Config> Configs { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<ExitRow> ExitRows { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightSchedule> FlightSchedules { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleBooking> ScheduleBookings { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatClass> SeatClasses { get; set; }

    public virtual DbSet<SeatMap> SeatMaps { get; set; }

    public virtual DbSet<SeatType> SeatTypes { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    public async Task CreateDatabase()
    {
        String databasename = Database.GetDbConnection().Database;

        Console.WriteLine("Create database " + databasename);
        bool result = await Database.EnsureCreatedAsync();
        string resultstring = result ? "Create database successfully" : "Database already exist";
        Console.WriteLine($"Database {databasename} : {resultstring}");
    }

    public async Task DeleteDatabase()
    {
        String databasename = Database.GetDbConnection().Database;
        Console.Write($"Confirm delete database {databasename} (y) ? ");
        string input = Console.ReadLine();

        if (input.ToLower() == "y")
        {
            bool deleted = await Database.EnsureDeletedAsync();
            string deletionInfo = deleted ? "Database deleted" : "Delete database failed";
            Console.WriteLine($"{databasename} {deletionInfo}");
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // string connectionString = _configuration.GetConnectionString("ConnectionStrings");
        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .UseSqlServer(connectionString)
            ;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(e => e.RegistrationNumber).HasName("PK__aircraft__125DB2A2E604A408");

            entity.HasOne(d => d.Model).WithMany(p => p.Aircraft).HasConstraintName("FK__aircraft__model___68487DD7");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__airport__3213E83FBD0BFB0E");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.City).WithMany(p => p.Airports).HasConstraintName("FK__airport__city_id__778AC167");

            entity.HasOne(d => d.District).WithMany(p => p.Airports).HasConstraintName("FK__airport__distric__787EE5A0");

            entity.HasOne(d => d.Ward).WithMany(p => p.Airports).HasConstraintName("FK__airport__ward_id__797309D9");
        });

        modelBuilder.Entity<AisleCol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__aisle_co__3213E83F3B06808E");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Model).WithMany(p => p.AisleCols).HasConstraintName("FK__aisle_col__model__6B24EA82");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__booking__3213E83F278586E2");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Bookings).HasConstraintName("FK__booking__user_id__7D439ABD");

            entity.HasMany(d => d.ServiceNames).WithMany(p => p.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                    "BookingService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceName")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__booking_s__servi__02084FDA"),
                    l => l.HasOne<Booking>().WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__booking_s__booki__01142BA1"),
                    j =>
                    {
                        j.HasKey("BookingId", "ServiceName").HasName("PK__booking___C94B4842E286A9C8");
                        j.ToTable("booking_service");
                        j.HasIndex(new[] { "ServiceName" }, "IX_booking_service_service_name");
                        j.IndexerProperty<long>("BookingId").HasColumnName("booking_id");
                        j.IndexerProperty<string>("ServiceName")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("service_name");
                    });
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__city__3213E83F8F0A62B8");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Config>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK__configs__DFD83CAE40CE0AED");

            entity.Property(e => e.Key).IsFixedLength();
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__district__3213E83F4540C126");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.City).WithMany(p => p.Districts).HasConstraintName("FK__district__city_i__02FC7413");
        });

        modelBuilder.Entity<ExitRow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exit_row__3213E83F21867537");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Model).WithMany(p => p.ExitRows).HasConstraintName("FK__exit_row__model___6A30C649");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__flight__3213E83F67464E02");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.AircraftRegistrationNumberNavigation).WithMany(p => p.Flights).HasConstraintName("FK__flight__aircraft__72C60C4A");

            entity.HasOne(d => d.Route).WithMany(p => p.Flights).HasConstraintName("FK__flight__route_id__73BA3083");
        });

        modelBuilder.Entity<FlightSchedule>(entity =>
        {
            entity.HasKey(e => new { e.FlightId, e.ScheduleId }).HasName("PK__flight_s__0F36FFC35870B195");

            entity.HasOne(d => d.Flight).WithMany(p => p.FlightSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__flight_sc__fligh__75A278F5");

            entity.HasOne(d => d.Schedule).WithMany(p => p.FlightSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__flight_sc__sched__76969D2E");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__model__3213E83F64857539");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__passenge__3213E83FC1DFDF90");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Booking).WithMany(p => p.Passengers).HasConstraintName("FK__passenger__booki__00200768");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__price__3213E83FA4A40A38");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Route).WithMany(p => p.Prices).HasConstraintName("FK__price__route_id__71D1E811");

            entity.HasOne(d => d.SeatClass).WithMany(p => p.Prices).HasConstraintName("FK__price__seat_clas__70DDC3D8");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__route__3213E83F3EC1E94B");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.DepartureAirportNavigation).WithMany(p => p.RouteDepartureAirportNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__route__departure__6EF57B66");

            entity.HasOne(d => d.DestinationAirportNavigation).WithMany(p => p.RouteDestinationAirportNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__route__destinati__6FE99F9F");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__schedule__3213E83F003A11A8");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules).HasConstraintName("FK__schedule__route___74AE54BC");
        });

        modelBuilder.Entity<ScheduleBooking>(entity =>
        {
            entity.HasKey(e => new { e.ScheduleId, e.BookingId }).HasName("PK__schedule__C1B4B034C4B7CC2E");

            entity.HasOne(d => d.Booking).WithMany(p => p.ScheduleBookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__schedule___booki__7F2BE32F");

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleBookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__schedule___sched__7E37BEF6");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__seat__3213E83F7530C246");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).IsFixedLength();

            entity.HasOne(d => d.Model).WithMany(p => p.Seats).HasConstraintName("FK__seat__model_id__6C190EBB");

            entity.HasOne(d => d.SeatClass).WithMany(p => p.Seats).HasConstraintName("FK__seat__seat_class__6D0D32F4");

            entity.HasOne(d => d.SeatType).WithMany(p => p.Seats).HasConstraintName("FK__seat__seat_type___6E01572D");
        });

        modelBuilder.Entity<SeatClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__seat_cla__3213E83FD5D1D927");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SeatMap>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK__seat_map__DC39CAF467886AEF");

            entity.Property(e => e.ModelId).ValueGeneratedNever();

            entity.HasOne(d => d.Model).WithOne(p => p.SeatMap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__seat_map__model___693CA210");
        });

        modelBuilder.Entity<SeatType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__seat_typ__3213E83F4BEA9166");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__service__72E12F1A07F61963");

            entity.Property(e => e.Name).IsFixedLength();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ticket__3213E83FB53CA3F1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets).HasConstraintName("FK__ticket__flight_i__7A672E12");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Tickets).HasConstraintName("FK__ticket__passenge__7C4F7684");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets).HasConstraintName("FK__ticket__seat_id__7B5B524B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F778C8393");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.IdNumber).IsFixedLength();

            entity.HasOne(d => d.City).WithMany(p => p.Users).HasConstraintName("FK__user__city_id__656C112C");

            entity.HasOne(d => d.District).WithMany(p => p.Users).HasConstraintName("FK__user__district_i__66603565");

            entity.HasOne(d => d.Ward).WithMany(p => p.Users).HasConstraintName("FK__user__ward_id__6754599E");

        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ward__3213E83F5DDAAE1F");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.District).WithMany(p => p.Wards).HasConstraintName("FK__ward__district_i__03F0984C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
