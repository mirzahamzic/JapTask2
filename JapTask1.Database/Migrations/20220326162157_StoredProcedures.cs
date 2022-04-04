using Microsoft.EntityFrameworkCore.Migrations;

namespace JapTask1.Database.Migrations
{
    public partial class StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var calculateTotalCosts = @"

				CREATE FUNCTION [dbo].[CalculateTotalCosts]  
				(  
					   @recipesIngredientQuantity decimal
					   , @ingredientPurchasedPrice decimal
					   , @ingredientPurchasedQuantity decimal
					   , @recipesIngredientUnit int
				)  
				RETURNS float  
				AS  
				BEGIN  
					   DECLARE @TotalCost float;
					   SELECT @TotalCost = case
						when @recipesIngredientUnit = 1 OR @recipesIngredientUnit=3 
						then
							(@recipesIngredientQuantity * @ingredientPurchasedPrice/@ingredientPurchasedQuantity)/1000
						else
							@recipesIngredientQuantity * @ingredientPurchasedPrice/@ingredientPurchasedQuantity
						end
					   RETURN ROUND(@TotalCost,2) 
				END
			";


            var sp1 = @"CREATE procedure [dbo].[spRecipe_GetRecipesWith10Ingredients]

						AS
						BEGIN
	
							SET NOCOUNT ON;
						
						SELECT Recipes.Name, Recipes.Id, 
						
						SUM(dbo.CalculateTotalCosts(dbo.RecipesIngredients.Quantity, dbo.Ingredients.PurchasedPrice,
						dbo.Ingredients.PurchasedQuantity,dbo.RecipesIngredients.Unit)) AS RecipeTotalCost,
						
						COUNT(dbo.RecipesIngredients.IngredientId) AS TotalIngredients
						FROM Ingredients
						JOIN RecipesIngredients
						ON Ingredients.Id = RecipesIngredients.IngredientId
						JOIN Recipes
						ON Recipes.Id = RecipesIngredients.RecipeId
						GROUP BY Recipes.Name, Recipes.Id, dbo.RecipesIngredients.Unit
						HAVING count(dbo.RecipesIngredients.IngredientId)>=10
						ORDER BY TotalIngredients desc
						END";

            var sp2 = @"CREATE PROCEDURE [dbo].[spRecipe_GetAllByCategoryName]

						AS
						BEGIN
	
							SET NOCOUNT ON;

							SELECT Categories.Name as CategoryName, Recipes.Name AS RecipeName,

							SUM(dbo.CalculateTotalCosts(dbo.RecipesIngredients.Quantity, dbo.Ingredients.PurchasedPrice,
							dbo.Ingredients.PurchasedQuantity,dbo.RecipesIngredients.Unit)) AS RecipeTotalCost	

							FROM Ingredients
							JOIN RecipesIngredients
							ON Ingredients.Id = RecipesIngredients.IngredientId
							JOIN Recipes
							ON Recipes.Id = RecipesIngredients.RecipeId
							JOIN Categories
							ON Categories.Id=Recipes.CategoryId
							GROUP BY Categories.Name, Recipes.Name, dbo.RecipesIngredients.Unit
							ORDER BY Categories.Name, RecipeTotalCost desc
						END";

            var sp3 = @"CREATE procedure [dbo].[spRecipe_GetUsage]
						@MinCount decimal
						, @MaxCount decimal
						, @Unit int
					AS 
					BEGIN

					SELECT COUNT (IngredientId) AS UsageCount, Ingredients.Name
						FROM RecipesIngredients
						JOIN Ingredients
						ON RecipesIngredients.IngredientId = Ingredients.Id 
						WHERE RecipesIngredients.Quantity between @MinCount and @MaxCount AND RecipesIngredients.Unit=@Unit 
						GROUP BY Ingredients.Name
						ORDER BY UsageCount desc
					END";

            migrationBuilder.Sql(calculateTotalCosts);
            migrationBuilder.Sql(sp1);
            migrationBuilder.Sql(sp2);
            migrationBuilder.Sql(sp3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
