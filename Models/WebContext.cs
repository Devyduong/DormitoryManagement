using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DormitoryManagement.Models
{
    public class WebContext : IdentityDbContext<AppUser>
    {
        public WebContext() : base("name=DormitoryDB", throwIfV1Schema: false)
        {

        }
        public static WebContext Create()
        {
            return new WebContext();
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<HomeFleet> HomeFleets { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Rent> Rents { get; set; }
    }
}