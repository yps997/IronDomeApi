using Microsoft.EntityFrameworkCore;
using IronDomeApi.Moddels;
using System;
using Microsoft.CodeAnalysis.Emit;

namespace IronDomeApi.Services
{
    public class DBService : DbContext
    {

        public DBService(DbContextOptions<DBService> options) : base(options)
        {
            if (Database.EnsureCreated() && Login.Count() == 0)
            { Seed(); }
        }

        public DbSet<Attack> Attacks { get; set; }
        public DbSet<LoginObject> Login { get; set; }
        public DbSet<Defense> Defenses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Database.EnsureCreated();
           
            base.OnModelCreating(modelBuilder);
        }
        public void Seed()
        {
            LoginObject login = new LoginObject()
            {
                UserName = "admin",
                Password = "1234"
            };
            Login.Add(login);
            SaveChanges();

        }
    }
    
}

       
   





        
  





        //public static List<Attack> AttacksList = new List<Attack>();

        //public static Defense Defenses = new Defense();
  
