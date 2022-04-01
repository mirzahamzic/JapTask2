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
					   RETURN @TotalCost 
				END
			";


            var sp1 = @"CREATE procedure [dbo].[spRecipe_GetRecipesWith10Ingredients]

						AS
						BEGIN
	
							SET NOCOUNT ON;
						
						select Recipes.Name, Recipes.Id, 
						
						sum(dbo.CalculateTotalCosts(dbo.RecipesIngredients.Quantity, dbo.Ingredients.PurchasedPrice,
						dbo.Ingredients.PurchasedQuantity,dbo.RecipesIngredients.Unit)) as RecipeTotalCost,
						
						count(dbo.RecipesIngredients.IngredientId) as TotalIngredients
						from Ingredients
						join RecipesIngredients
						on Ingredients.Id = RecipesIngredients.IngredientId
						join Recipes
						on Recipes.Id = RecipesIngredients.RecipeId
						group by Recipes.Name, Recipes.Id, dbo.RecipesIngredients.Unit
						having count(dbo.RecipesIngredients.IngredientId)>=10
						order by TotalIngredients desc
						END";

            var sp2 = @"CREATE PROCEDURE [dbo].[spRecipe_GetAllByCategoryName]

						AS
						BEGIN
	
							SET NOCOUNT ON;

							select Categories.Name as CategoryName, Recipes.Name as RecipeName,

							sum(dbo.CalculateTotalCosts(dbo.RecipesIngredients.Quantity, dbo.Ingredients.PurchasedPrice,
							dbo.Ingredients.PurchasedQuantity,dbo.RecipesIngredients.Unit)) as RecipeTotalCost	

							from Ingredients
							join RecipesIngredients
							on Ingredients.Id = RecipesIngredients.IngredientId
							join Recipes
							on Recipes.Id = RecipesIngredients.RecipeId
							join Categories
							on Categories.Id=Recipes.CategoryId
							group by Categories.Name, Recipes.Name, dbo.RecipesIngredients.Unit
							order by Categories.Name, RecipeTotalCost desc
						END";

            var sp3 = @"ALTER procedure [dbo].[spRecipe_GetUsage]
						@MinCount decimal
						, @MaxCount decimal
						, @Unit int
					as 
					begin

					select count (IngredientId) as UsageCount, Ingredients.Name
						from RecipesIngredients
						join Ingredients
						on RecipesIngredients.IngredientId = Ingredients.Id 
						where RecipesIngredients.Quantity between @MinCount and @MaxCount AND RecipesIngredients.Unit=@Unit 
						group by Ingredients.Name
						order by UsageCount desc
					end";

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
