using AutoMapper;
using JapTask1.Common.Enums;
using JapTask1.Common.Helpers;
using JapTask1.Core.Dtos;
using JapTask1.Core.Dtos.Request;
using JapTask1.Core.Dtos.Response;
using JapTask1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Recipe, GetRecipeDto>()
            .ForMember(dest => dest.Ingredients, o => o.MapFrom(src => src.RecipesIngredients))
            .ForMember(dest => dest.TotalCost, o => o.MapFrom(src => Calculator.RecipeTotalCost(src)));

            CreateMap<Recipe, FirstSpDto>();
            CreateMap<Recipe, SecondSpDto>();


            CreateMap<RecipeIngredient, GetIngredientDto>()
            .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Ingredient.Name))
            .ForMember(dest => dest.Price, o => o.MapFrom(src => Calculator.PricePerIngredient(src.Ingredient.PurchasedQuantity, src.Ingredient.PurchasedUnitOfMeasure, src.Ingredient.PurchasedPrice, src.Unit, src.Quantity)))
            .ForMember(dest => dest.Unit, o => o.MapFrom(src => Enum.GetName(typeof(Units), src.Unit)));

            CreateMap<Category, GetCategoryDto>();
            CreateMap<Ingredient, IngredientDto>();


        }

    }
}
