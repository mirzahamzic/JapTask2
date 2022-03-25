using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Core.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double TotalCosts { get; set; }
        public int TotalIngredients { get; set; }
    }
}
