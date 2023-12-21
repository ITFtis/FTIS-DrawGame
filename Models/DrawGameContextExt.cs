using FtisHelperDrawGame.DB.Model;
using System;
using System.Data.Entity;
using System.Linq;

namespace DouImp.Models
{
    public class DrawGameContextExt : Dou.Models.ModelContextBase<User, Role>
    {
        public DrawGameContextExt() : base("name=DouModelContextExt")
        {
            Database.SetInitializer<DrawGameContextExt>(null);
        }

        public DbSet<ACTIVITIES> ACTIVITIES { get; set; }
        //public DbSet<API> APIS { get; set; }
        public DbSet<PARTICIPANT> PARTICIPANTS { get; set; }
        public DbSet<PRIZE> PRIZES { get; set; }
        public DbSet<WINNER> WINNERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<F22cmmEmpData>().HasOptional(s=>s.Seat);
            base.OnModelCreating(modelBuilder);
        }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}