using JapTask1.Common.Enums;
using JapTask1.Common.Helpers;
using JapTask1.Core.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Tests
{
    [TestFixture]
    public class JapTask1CalculatorTest
    {
        [Test]
        public void IngredientUnitCost_Ingredient_GetCorrectCalculation()
        {
            var ingredient = new Ingredient()
            {
                Name = "Test ingredient",
                PurchasedQuantity = 1,
                PurchasedPrice = 10,
                PurchasedUnitOfMeasure = Units.Kg,
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var recipe = new Recipe()
            {
                Id = 1,
                Name = "Test",
                Description = "Test recipe",
                CreatedAt = DateTime.Now,
            };

            var recipeIngredient = new RecipeIngredient()
            {
                Id = 1,
                Recipe = recipe,
                RecipeId = recipe.Id,
                Ingredient = ingredient,
                IngredientId = ingredient.Id,
                Quantity = 500,
                Unit = Units.Gr,
            };

            var result = Calculator.PricePerIngredient(
                        ingredient.PurchasedQuantity,
                        ingredient.PurchasedUnitOfMeasure,
                        ingredient.PurchasedPrice,
                        recipeIngredient.Unit,
                        recipeIngredient.Quantity);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void IngredientUnitCost2_Ingredient_GetCorrectCalculation()
        {
            var ingredient = new Ingredient()
            {
                Name = "Test ingredient",
                PurchasedQuantity = 25,
                PurchasedPrice = 250,
                PurchasedUnitOfMeasure = Units.Kg,
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var recipe = new Recipe()
            {
                Id = 1,
                Name = "Test",
                Description = "Test recipe",
                CreatedAt = DateTime.Now,
            };

            var recipeIngredient = new RecipeIngredient()
            {
                Id = 1,
                Recipe = recipe,
                RecipeId = recipe.Id,
                Ingredient = ingredient,
                IngredientId = ingredient.Id,
                Quantity = 1500,
                Unit = Units.Gr,
            };

            var result = Calculator.PricePerIngredient(
                        ingredient.PurchasedQuantity,
                        ingredient.PurchasedUnitOfMeasure,
                        ingredient.PurchasedPrice,
                        recipeIngredient.Unit,
                        recipeIngredient.Quantity);

            Assert.AreEqual(15, result);
        }

        [Test]
        public void RecipeTotalCost_Recipe_CheckIfCalculateCorrectly()
        {
            var ingredient1 = new Ingredient()
            {
                Name = "Test ingredient",
                PurchasedQuantity = 1,
                PurchasedPrice = 10,
                PurchasedUnitOfMeasure = Units.Kg,
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var ingredient2 = new Ingredient()
            {
                Name = "Test ingredient 2",
                PurchasedQuantity = 1,
                PurchasedPrice = 10,
                PurchasedUnitOfMeasure = Units.Kg,
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var recipe = new Recipe()
            {
                Id = 1,
                Name = "Test",
                Description = "Test recipe",
                CreatedAt = DateTime.Now,
                RecipesIngredients = new List<RecipeIngredient>()
                {
                    new RecipeIngredient()
                    {
                        Id = 1,
                        Ingredient = ingredient1,
                        IngredientId= ingredient1.Id,
                        Quantity = 1,
                        Unit = Units.Kg,
                    },
                     new RecipeIngredient()
                    {
                        Id = 2,
                        Ingredient = ingredient2,
                        IngredientId = ingredient2.Id,
                        Quantity = 1,
                        Unit = Units.Kg,
                    },
                 }
            };

            var result = Calculator.RecipeTotalCost(recipe);
            Assert.AreEqual(20, result);
        }

        [Test]
        public void RecipeTotalCost2_Recipe_CheckIfCalculateCorrectly()
        {
            var ingredient1 = new Ingredient()
            {
                Name = "Test ingredient",
                PurchasedQuantity = 1,
                PurchasedPrice = 20,
                PurchasedUnitOfMeasure = Units.Kg,
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var ingredient2 = new Ingredient()
            {
                Name = "Test ingredient 2",
                PurchasedQuantity = 1,
                PurchasedPrice = 20,
                PurchasedUnitOfMeasure = Units.Kg,
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var recipe = new Recipe()
            {
                Id = 1,
                Name = "Test",
                Description = "Test recipe",
                CreatedAt = DateTime.Now,
                RecipesIngredients = new List<RecipeIngredient>()
                {
                    new RecipeIngredient()
                    {
                        Id = 1,
                        Ingredient = ingredient1,
                        IngredientId= ingredient1.Id,
                        Quantity = 1000,
                        Unit = Units.Gr,
                    },
                     new RecipeIngredient()
                    {
                        Id = 2,
                        Ingredient = ingredient2,
                        IngredientId = ingredient2.Id,
                        Quantity = 1000,
                        Unit = Units.Gr,
                    },
                 }
            };

            var result = Calculator.RecipeTotalCost(recipe);
            Assert.AreEqual(40, result);
        }

    }
}
