using Microsoft.EntityFrameworkCore;
using RDC.API.Models.Company;
using RDC.API.Models.Drone;
using RDC.API.Models.Flight;
using RDC.API.Models.Subscription;
using RDC.API.Models.SubscriptionPayment;
using RDC.API.Models.User;

namespace RDC.API.Context
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<DroneModel> Drone { get; set; }
        public DbSet<CompanyModel> Company { get; set; }

        public DbSet<UserModel> User { get; set; }

        public DbSet<FlightModel> Flight { get; set; }

        public DbSet<SubscriptionModel> Subscription { get; set; }

        public DbSet<SubscriptionPaymentModel> SubscriptionPayment { get; set; }

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
