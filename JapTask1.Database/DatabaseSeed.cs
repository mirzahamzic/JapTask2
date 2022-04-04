using JapTask1.Common.Enums;
using JapTask1.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Database
{
    public class DatabaseSeed
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            List<Recipe> recipes = new List<Recipe>();


            if (!context.Ingredients.Any())
            {
                context.Ingredients.AddRange(
                    new Ingredient { Name = "Brasno", PurchasedQuantity = 10, PurchasedPrice = 50, PurchasedUnitOfMeasure = Units.Kg },
                    new Ingredient { Name = "Ulje", PurchasedQuantity = 20, PurchasedPrice = 100, PurchasedUnitOfMeasure = Units.L },
                    new Ingredient { Name = "Secer", PurchasedQuantity = 20, PurchasedPrice = 75, PurchasedUnitOfMeasure = Units.Kg },
                    new Ingredient { Name = "Jaja", PurchasedQuantity = 200, PurchasedPrice = 145.50, PurchasedUnitOfMeasure = Units.kom },
                    new Ingredient { Name = "Mlijeko", PurchasedQuantity = 30, PurchasedPrice = 75.5, PurchasedUnitOfMeasure = Units.L },
                    new Ingredient { Name = "Masline", PurchasedQuantity = 10, PurchasedPrice = 80, PurchasedUnitOfMeasure = Units.Kg },
                    new Ingredient { Name = "Sir trapist", PurchasedQuantity = 15, PurchasedPrice = 200, PurchasedUnitOfMeasure = Units.Kg },
                    new Ingredient { Name = "Kecap", PurchasedQuantity = 2, PurchasedPrice = 25.60, PurchasedUnitOfMeasure = Units.Kg },
                    new Ingredient { Name = "Cokolada", PurchasedQuantity = 5, PurchasedPrice = 500, PurchasedUnitOfMeasure = Units.Kg },
                    new Ingredient { Name = "Puding", PurchasedQuantity = 7, PurchasedPrice = 58, PurchasedUnitOfMeasure = Units.Kg }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Pite" },
                    new Category { Name = "Kolaci" },
                    new Category { Name = "Pizza" },
                    new Category { Name = "Mexico" },
                    new Category { Name = "Italijanska" }
                 );
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                string password = "admin123456";
                User user = new()
                {
                    Name = "Admin"
                };

                byte[] passwordSalt;
                byte[] passwordHash;

                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.CreatedAt = DateTime.Now;

                context.Users.Add(user);
                await context.SaveChangesAsync();

            };

            if (!context.Recipes.Any())
            {
                //create random recipes

                var random = new Random();
                var recipeName = new string[] { "Sirnica", "Burek", "Margarita", "Saher torta", "Lazanje", "Tortilja", "Quesedillas", "Tacos", "Snicla" };
                var recipeList = new List<Recipe>();

                for (int i = 0; i < 20; i++)
                {
                    int index = random.Next(recipeName.Length);
                    recipeList.Add(new Recipe { Name = recipeName[index], CategoryId = random.Next(1, 5), Description = "Some seeded description", UserId = 1 });
                }
                await context.Recipes.AddRangeAsync(recipeList);
                await context.SaveChangesAsync();
            }

            if (!context.RecipesIngredients.Any())
            {

                var random = new Random();
                var recipesIngredients = new List<RecipeIngredient>();

                for (int i = 0; i < 100; i++)
                {
                    recipesIngredients.Add(new RecipeIngredient { RecipeId = random.Next(1, 20), IngredientId = random.Next(1, 10), Quantity = random.Next(10, 1000), Unit = Units.Gr });
                }
                for (int i = 0; i < 100; i++)
                {
                    recipesIngredients.Add(new RecipeIngredient { RecipeId = random.Next(1, 20), IngredientId = random.Next(1, 10), Quantity = random.NextDouble() * 2, Unit = Units.Kg });
                }
                await context.RecipesIngredients.AddRangeAsync(recipesIngredients);
                await context.SaveChangesAsync();
            }
        }
    }
}

