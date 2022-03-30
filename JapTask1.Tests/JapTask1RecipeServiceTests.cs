using AutoMapper;
using JapTask1.Api.Controllers;
using JapTask1.Common.Enums;
using JapTask1.Core.Dtos.Request;
using JapTask1.Core.Dtos.Response;
using JapTask1.Core.Entities;
using JapTask1.Core.Interfaces;
using JapTask1.Database;
using JapTask1.Services.RecipeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Tests
{
    [TestFixture]
    public class JapTask1RecipeServiceTests
    {
        private RecipeService _recipeService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IMapper> mockMapper;
        private Mock<IHttpContextAccessor> mockHttpContextAccessor;

        private DbContextOptions<AppDbContext> _options;
        private AppDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            mockConfiguration = new Mock<IConfiguration>();
            mockMapper = new Mock<IMapper>();
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "temp_jap").Options;
            _context = new AppDbContext(_options);

            _recipeService = new RecipeService(_context, mockMapper.Object, mockConfiguration.Object, mockHttpContextAccessor.Object);

            SetupDatabase();
        }

        [Test]
        public void AddSameIngredient_InputIngredient_ThrowException()
        {
            var testIngredients = new List<AddRecipeIngredientDto>();
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 1, Quantity = 200, Unit = Common.Enums.Units.Gr });
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 1, Quantity = 300, Unit = Common.Enums.Units.Gr });

            var testRecipe = new AddRecipeDto()
            {
                Name = "Test Recipe",
                Description = "Test",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                AddRecipeIngredientDto = testIngredients
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await _recipeService.Create(testRecipe));
        }

        [Test]
        public void AddNoIngredient_InputIngredient_ThrowException()
        {
            var testIngredients = new List<AddRecipeIngredientDto>();

            var testRecipe = new AddRecipeDto()
            {
                Name = "Test Recipe",
                Description = "Test",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                AddRecipeIngredientDto = testIngredients
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await _recipeService.Create(testRecipe));
        }

        [Test]
        public void AddMultipleSameIngredient_InputIngredient_ThrowException()
        {
            var testIngredients = new List<AddRecipeIngredientDto>();
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 1, Quantity = 200, Unit = Common.Enums.Units.Gr });
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 1, Quantity = 300, Unit = Common.Enums.Units.Gr });
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 2, Quantity = 500, Unit = Common.Enums.Units.Gr });

            var testRecipe = new AddRecipeDto()
            {
                Name = "Test Recipe",
                Description = "Test",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                AddRecipeIngredientDto = testIngredients
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await _recipeService.Create(testRecipe));
        }

        [Test]
        public async Task SaveRecipe_AddRecipe_CheckTheValuesFromDB()
        {
            //arrange
            var testIngredients = new List<AddRecipeIngredientDto>();
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 1, Quantity = 200, Unit = Common.Enums.Units.Gr });

            var testRecipe = new AddRecipeDto()
            {
                Name = "Test Recipe",
                Description = "Test",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                AddRecipeIngredientDto = testIngredients
            };

            //act
            await _recipeService.Create(testRecipe);

            //assert
            var dbRecipes = await _context.Recipes.FirstOrDefaultAsync(r => r.Name == testRecipe.Name);

            Assert.AreEqual(testRecipe.Name, dbRecipes.Name);
            Assert.AreEqual(testRecipe.Description, dbRecipes.Description);
            Assert.AreEqual(testRecipe.CategoryId, dbRecipes.CategoryId);
            Assert.IsNotNull(dbRecipes.CreatedAt);
            Assert.True(testRecipe.AddRecipeIngredientDto.Any());
        }
        [Test]
        public async Task SaveRecipe_AddRecipeWithMultiIngredients_CheckTheValuesFromDB()
        {
            //arrange
            var testIngredients = new List<AddRecipeIngredientDto>();
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 1, Quantity = 200, Unit = Common.Enums.Units.Gr });
            testIngredients.Add(new AddRecipeIngredientDto() { IngredientId = 2, Quantity = 0.5, Unit = Common.Enums.Units.Kg });

            var testRecipe = new AddRecipeDto()
            {
                Name = "Test2 Recipe",
                Description = "Test",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                AddRecipeIngredientDto = testIngredients
            };

            //act
            await _recipeService.Create(testRecipe);

            //assert
            var dbRecipes = await _context.Recipes.FirstOrDefaultAsync(r => r.Name == testRecipe.Name);

            Assert.AreEqual(testRecipe.Name, dbRecipes.Name);
            Assert.AreEqual(testRecipe.Description, dbRecipes.Description);
            Assert.AreEqual(testRecipe.CategoryId, dbRecipes.CategoryId);
            Assert.IsNotNull(dbRecipes.CreatedAt);
            Assert.True(testRecipe.AddRecipeIngredientDto.Any());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TestLoadMore(int page_size)
        {
            //arrange


            //act
            var result = await _recipeService.Get(new BaseSearch { Limit = 0, PageSize = page_size });

            //assert
            Assert.That(result.Data.Count, Is.EqualTo(page_size));
        }

        public void SetupDatabase()
        {
            _context.Recipes.AddRange(
                new Recipe { CategoryId = 1, UserId = 1, },
                new Recipe { CategoryId = 1, UserId = 1 },
                new Recipe { CategoryId = 1, UserId = 1 });

            _context.Ingredients.AddRange(
                new Ingredient { Id = 1, Name = "Brasno" },
                new Ingredient { Id = 2, Name = "Secer" });

            _context.Categories.AddRange(
                new Category { Name = "Prvi" },
                new Category { Name = "Drugi" });

            _context.RecipesIngredients.AddRange(
                new RecipeIngredient { IngredientId = 1, RecipeId = 1 },
                new RecipeIngredient { IngredientId = 2, RecipeId = 3 });

            _context.Users.Add(new() { Name = "admin" });

            _context.SaveChanges();
        }
    }


}

