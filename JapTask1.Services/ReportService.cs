using AutoMapper;
using JapTask1.Core.Dtos;
using JapTask1.Core.Entities;
using JapTask1.Core.Interfaces;
using JapTask1.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReportService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<FirstSpDto>>> FirstSp()
        {
            string sql = "exec dbo.spRecipe_GetRecipesWith10Ingredients";

            var list = await _context.FirstSpDto
                            .FromSqlRaw(sql)
                            .ToListAsync();

            var dbRecipes = list.Select(r => _mapper.Map<FirstSpDto>(r)).ToList();

            return new ServiceResponse<List<FirstSpDto>>()
            {
                Data = dbRecipes
            };
        }

        public async Task<ServiceResponse<List<SecondSpDto>>> SecondSp()
        {
            string sql = "exec dbo.spRecipe_GetAllByCategoryName";

            var list = await _context.SecondSpDto
                            .FromSqlRaw(sql)
                            .ToListAsync();

            var dbRecipes = list.Select(r => _mapper.Map<SecondSpDto>(r)).ToList();

            return new ServiceResponse<List<SecondSpDto>>()
            {
                Data = dbRecipes
            };

        }

        public async Task<ServiceResponse<List<ThirdSpDto>>> ThirdSp(double min, double max, int unit)
        {
            string sql = $"exec dbo.spRecipe_GetUsage {min}, {max}, {unit}";

            var list = await _context.ThirdSpDto
                            .FromSqlRaw(sql)
                            .ToListAsync();

            var dbRecipes = list.Select(r => _mapper.Map<ThirdSpDto>(r)).ToList();

            return new ServiceResponse<List<ThirdSpDto>>()
            {
                Data = dbRecipes
            };
        }
    }
}
