﻿using JapTask1.Common.Enums;
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
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                List<Recipe> recipes = new List<Recipe>();


                if (!context.Ingredients.Any())
                {
                    context.Ingredients.AddRange(
                        new Ingredient()
                        {
                            Name = "Brasno",
                            PurchasedQuantity = 10,
                            PurchasedPrice = 50,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-1)

                        },
                        new Ingredient()
                        {
                            Name = "Secer",
                            PurchasedQuantity = 12,
                            PurchasedPrice = 75,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Ulje",
                            PurchasedQuantity = 25,
                            PurchasedPrice = 120,
                            PurchasedUnitOfMeasure = Units.L,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Piletina",
                            PurchasedQuantity = 50,
                            PurchasedPrice = 350,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Maslinovo ulje",
                            PurchasedQuantity = 10,
                            PurchasedPrice = 150,
                            PurchasedUnitOfMeasure = Units.L,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Trapist",
                            PurchasedQuantity = 18,
                            PurchasedPrice = 230,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Mocarela",
                            PurchasedQuantity = 8,
                            PurchasedPrice = 160,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Paradajz Sos",
                            PurchasedQuantity = 10,
                            PurchasedPrice = 50,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Kvasac",
                            PurchasedQuantity = 7,
                            PurchasedPrice = 58,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Sampinjoni",
                            PurchasedQuantity = 5,
                            PurchasedPrice = 35,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Cokalada",
                            PurchasedQuantity = 18,
                            PurchasedPrice = 250,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Kore za tortu",
                            PurchasedQuantity = 5,
                            PurchasedPrice = 200,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        }, new Ingredient()
                        {
                            Name = "Orasi",
                            PurchasedQuantity = 10,
                            PurchasedPrice = 150,
                            PurchasedUnitOfMeasure = Units.Kg,
                            CreatedAt = DateTime.Now.AddDays(-2)

                        });

                    await context.SaveChangesAsync();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category()
                        {
                            Name = "Pite",
                        },

                        new Category()
                        {
                            Name = "Kolaci",
                        },

                        new Category()
                        {
                            Name = "Torte",
                        },

                        new Category()
                        {
                            Name = "Meksicka",
                        },

                        new Category()
                        {
                            Name = "Pice",
                        },

                        new Category()
                        {
                            Name = "Paste",
                        },

                        new Category()
                        {
                            Name = "Antipaste",
                        });

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

                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
                    var recipeName = new char[8];
                    var recipeDescription = new char[60];
                    var random = new Random();
                    var recipeList = new List<Recipe>();

                    for (int i = 0; i < 50; i++)
                    {

                        for (int j = 0; j < recipeName.Length; j++)
                        {
                            recipeName[j] = chars[random.Next(chars.Length)];
                        }

                        for (int x = 0; x < recipeDescription.Length; x++)
                        {
                            recipeDescription[x] = chars[random.Next(chars.Length)];
                        }

                        var finalRecipeName = new String(recipeName);
                        var finalDescription = new String(recipeDescription);

                        recipeList.Add(new Recipe { Name = finalRecipeName, Description = finalDescription, CategoryId = random.Next(1, 6), UserId = 1 });
                    }

                    await context.Recipes.AddRangeAsync(recipeList);
                    await context.SaveChangesAsync();
                }

                if (!context.RecipesIngredients.Any())
                {
                    //create random recipesingredient

                    var random = new Random();
                    var recipesIngredients = new List<RecipeIngredient>();

                    for (int i = 0; i < 500; i++)
                    {
                        recipesIngredients.Add(new RecipeIngredient { IngredientId = random.Next(1, 7), RecipeId = random.Next(1, 50), Quantity = random.Next(10, 1000), Unit = i % 4 == 0 ? Units.Gr : Units.Kg });
                    }

                    await context.RecipesIngredients.AddRangeAsync(recipesIngredients);
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
