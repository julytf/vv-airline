using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity
    .EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace vv_airline.Models.Data;

public partial class AppDBContext : IdentityDbContext<User>
{
    // private readonly string _connectionString;
    IConfiguration _configuration;

    public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration)
    : base(options)
    {
        _configuration = configuration;
        // _connectionString = connectionString;
        // Console.WriteLine(_connectionString);
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

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Config> Configs { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<ExitRow> ExitRows { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatClass> SeatClasses { get; set; }

    public virtual DbSet<SeatMap> SeatMaps { get; set; }

    public virtual DbSet<SeatType> SeatTypes { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public DbSet<User> AppUsers { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    // public async Task CreateDatabase()
    // {
    //     String databasename = Database.GetDbConnection().Database;

    //     Console.WriteLine("Create database " + databasename);
    //     bool result = await Database.EnsureCreatedAsync();
    //     string resultstring = result ? "Create database successfully" : "Database already exist";
    //     Console.WriteLine($"Database {databasename} : {resultstring}");
    // }

    // public async Task DeleteDatabase()
    // {
    //     String databasename = Database.GetDbConnection().Database;
    //     Console.Write($"Confirm delete database {databasename} (y) ? ");
    //     string input = Console.ReadLine() ?? "";

    //     if (input.ToLower() == "y")
    //     {
    //         bool deleted = await Database.EnsureDeletedAsync();
    //         string deletionInfo = deleted ? "Database deleted" : "Delete database failed";
    //         Console.WriteLine($"{databasename} {deletionInfo}");
    //     }
    // }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            ;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName!.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        modelBuilder.Entity<Route>(entity =>
        {
            entity
                .HasOne(e => e.DepartureAirport)
                .WithMany(e => e.RouteDepartures)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity
                .HasOne(e => e.DestinationAirport)
                .WithMany(e => e.RouteDestinations)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity
                .HasMany(e => e.Districts)
                .WithOne(e => e.Province);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity
                .HasMany(e => e.Flights)
                .WithMany(e => e.Schedules);

            entity
                .HasOne(e => e.Route)
                .WithMany(e => e.Schedules)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity
                .HasMany(e => e.Schedules)
                .WithMany(e => e.Bookings);
        });

        modelBuilder.Entity<SeatMap>(entity =>
        {
            entity
                .HasOne(e => e.Model)
                .WithOne(e => e.SeatMap)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
