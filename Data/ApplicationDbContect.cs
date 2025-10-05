using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Data.Models;

namespace AssetManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<AssetAssignment> AssetAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<AssetAssignment>()
                .HasOne(aa => aa.Asset)
                .WithMany(a => a.AssetAssignments)
                .HasForeignKey(aa => aa.AssetId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<AssetAssignment>()
                .HasOne(aa => aa.Employee)
                .WithMany(e => e.AssetAssignments)
                .HasForeignKey(aa => aa.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Create unique index on SerialNumber
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.SerialNumber)
                .IsUnique();

            // Create unique index on Email
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FullName = "John Doe",
                    Department = "IT",
                    Email = "john.doe@company.com",
                    PhoneNumber = "1234567890",
                    Designation = "Senior Developer",
                    IsActive = true
                },
                new Employee
                {
                    EmployeeId = 2,
                    FullName = "Jane Smith",
                    Department = "HR",
                    Email = "jane.smith@company.com",
                    PhoneNumber = "0987654321",
                    Designation = "HR Manager",
                    IsActive = true
                },
                new Employee
                {
                    EmployeeId = 3,
                    FullName = "Mike Johnson",
                    Department = "IT",
                    Email = "mike.johnson@company.com",
                    PhoneNumber = "5551234567",
                    Designation = "Junior Developer",
                    IsActive = true
                },
                new Employee
                {
                    EmployeeId = 4,
                    FullName = "Sarah Williams",
                    Department = "Finance",
                    Email = "sarah.williams@company.com",
                    PhoneNumber = "5559876543",
                    Designation = "Accountant",
                    IsActive = true
                },
                new Employee
                {
                    EmployeeId = 5,
                    FullName = "David Brown",
                    Department = "IT",
                    Email = "david.brown@company.com",
                    PhoneNumber = "5556781234",
                    Designation = "DevOps Engineer",
                    IsActive = false
                }
            );

            // Seed Assets
            modelBuilder.Entity<Asset>().HasData(
                new Asset
                {
                    AssetId = 1,
                    AssetName = "Dell Laptop XPS 15",
                    AssetType = "Laptop",
                    MakeModel = "Dell XPS 15 9520",
                    SerialNumber = "DL-XPS-001",
                    PurchaseDate = DateTime.Now.AddYears(-1),
                    WarrantyExpiryDate = DateTime.Now.AddYears(2),
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = false,
                    Specifications = "Intel i7, 16GB RAM, 512GB SSD"
                },
                new Asset
                {
                    AssetId = 2,
                    AssetName = "HP Laptop ProBook",
                    AssetType = "Laptop",
                    MakeModel = "HP ProBook 450 G9",
                    SerialNumber = "HP-PRO-002",
                    PurchaseDate = DateTime.Now.AddYears(-2),
                    WarrantyExpiryDate = DateTime.Now.AddYears(1),
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = false,
                    Specifications = "Intel i5, 8GB RAM, 256GB SSD"
                },
                new Asset
                {
                    AssetId = 3,
                    AssetName = "Samsung Monitor 27\"",
                    AssetType = "Monitor",
                    MakeModel = "Samsung 27\" LED",
                    SerialNumber = "SM-MON-003",
                    PurchaseDate = DateTime.Now.AddMonths(-18),
                    WarrantyExpiryDate = DateTime.Now.AddMonths(6),
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = false,
                    Specifications = "27 inch, 1920x1080, IPS Panel"
                },
                new Asset
                {
                    AssetId = 4,
                    AssetName = "LG Monitor 24\"",
                    AssetType = "Monitor",
                    MakeModel = "LG 24\" LED",
                    SerialNumber = "LG-MON-004",
                    PurchaseDate = DateTime.Now.AddMonths(-12),
                    WarrantyExpiryDate = DateTime.Now.AddYears(2),
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = true,
                    Specifications = "24 inch, 1920x1080"
                },
                new Asset
                {
                    AssetId = 5,
                    AssetName = "Logitech Keyboard",
                    AssetType = "Keyboard",
                    MakeModel = "Logitech K380",
                    SerialNumber = "LG-KB-005",
                    PurchaseDate = DateTime.Now.AddMonths(-6),
                    WarrantyExpiryDate = DateTime.Now.AddYears(1),
                    Condition = "New",
                    Status = "Available",
                    IsSpare = false,
                    Specifications = "Wireless, Bluetooth"
                },
                new Asset
                {
                    AssetId = 6,
                    AssetName = "Logitech Mouse",
                    AssetType = "Mouse",
                    MakeModel = "Logitech MX Master 3",
                    SerialNumber = "LG-MS-006",
                    PurchaseDate = DateTime.Now.AddMonths(-8),
                    WarrantyExpiryDate = DateTime.Now.AddMonths(16),
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = true,
                    Specifications = "Wireless, Ergonomic"
                },
                new Asset
                {
                    AssetId = 7,
                    AssetName = "iPhone 13",
                    AssetType = "Mobile Phone",
                    MakeModel = "Apple iPhone 13",
                    SerialNumber = "APL-IP-007",
                    PurchaseDate = DateTime.Now.AddYears(-1),
                    WarrantyExpiryDate = DateTime.Now.AddMonths(1), // Expiring soon
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = false,
                    Specifications = "128GB, Blue"
                },
                new Asset
                {
                    AssetId = 8,
                    AssetName = "MacBook Pro 14",
                    AssetType = "Laptop",
                    MakeModel = "Apple MacBook Pro 14",
                    SerialNumber = "APL-MBP-008",
                    PurchaseDate = DateTime.Now.AddMonths(-3),
                    WarrantyExpiryDate = DateTime.Now.AddMonths(33),
                    Condition = "New",
                    Status = "Under Repair",
                    IsSpare = false,
                    Specifications = "M2 Pro, 16GB RAM, 512GB SSD"
                },
                new Asset
                {
                    AssetId = 9,
                    AssetName = "Dell Monitor 32\"",
                    AssetType = "Monitor",
                    MakeModel = "Dell UltraSharp 32",
                    SerialNumber = "DL-MON-009",
                    PurchaseDate = DateTime.Now.AddYears(-3),
                    WarrantyExpiryDate = DateTime.Now.AddMonths(-6), // Expired
                    Condition = "Needs Repair",
                    Status = "Retired",
                    IsSpare = false,
                    Specifications = "32 inch, 4K"
                },
                new Asset
                {
                    AssetId = 10,
                    AssetName = "HP Laptop EliteBook",
                    AssetType = "Laptop",
                    MakeModel = "HP EliteBook 840 G8",
                    SerialNumber = "HP-ELT-010",
                    PurchaseDate = DateTime.Now.AddMonths(-10),
                    WarrantyExpiryDate = DateTime.Now.AddYears(2),
                    Condition = "Good",
                    Status = "Available",
                    IsSpare = false,
                    Specifications = "Intel i7, 16GB RAM, 512GB SSD"
                }
            );
        }
    }
}