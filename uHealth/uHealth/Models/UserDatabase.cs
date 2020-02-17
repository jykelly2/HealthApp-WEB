using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace uHealth.Models
{
    public class UserDatabase : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<News> News { get; set; }

        public UserDatabase(DbContextOptions<UserDatabase> options) : base(options)
        {
        }


    }
}
