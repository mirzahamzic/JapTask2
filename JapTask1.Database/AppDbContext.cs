using JapTask1.Core.Dtos;
using JapTask1.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RecipeIngredient> RecipesIngredients { get; set; }

        //stored procedures view entities
        public DbSet<FirstSpDto> FirstSpDto { get; set; }
        public DbSet<SecondSpDto> SecondSpDto { get; set; }
        public DbSet<ThirdSpDto> ThirdSpDto { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //many to many relationship override
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(r => r.Recipe)
                .WithMany(ri => ri.RecipesIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(r => r.Ingredient)
                .WithMany(ri => ri.RecipesIngredients)
                .HasForeignKey(ri => ri.IngredientId);


            //entity configuration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


            //stored procedures view entities
            modelBuilder.Entity<FirstSpDto>(
             eb =>
             {
                 eb.HasNoKey();
                 eb.ToView("View_FirstSpDto");
                 //eb.Property(v => v.).HasColumnName("Name");
             });

            modelBuilder.Entity<SecondSpDto>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("View_SecondSpDto");

            });

            modelBuilder.Entity<ThirdSpDto>(
           eb =>
           {
               eb.HasNoKey();
               eb.ToView("View_ThirdSpDto");

           });
        }


    }
}

