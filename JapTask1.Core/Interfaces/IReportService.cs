using JapTask1.Core.Dtos;
using JapTask1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapTask1.Core.Interfaces
{
    public interface IReportService
    {
        Task<ServiceResponse<List<FirstSpDto>>> FirstSp();
        Task<ServiceResponse<List<SecondSpDto>>> SecondSp();
        Task<ServiceResponse<List<ThirdSpDto>>> ThirdSp(double min, double max, int unit);
    }
}
