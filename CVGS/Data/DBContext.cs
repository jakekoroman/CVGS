using System;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;

namespace CVGS.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        { }

        public DbSet<User> User { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<Game> Game { get; set; }

        public DbSet<GameReview> GameReview { get; set; }

        public DbSet<CreditCard> CreditCard { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<WishList> WishList { get; set; }

        public DbSet<FriendList> FriendList { get; set; }

        public DbSet<Friend> Friend { get; set; }

        public DbSet<FamilyList> FamilyList { get; set; }

        public DbSet<Family> Family { get; set; }

        public DbSet<GameRatings> GameRatings { get; set; }

        public DbSet<EventRegistry> EventRegistry { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}
