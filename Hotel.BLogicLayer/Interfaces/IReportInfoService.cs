using System.Threading.Tasks;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface IReportInfoService
    {
        public Task<ProfitReportDto> GetProfitReport(DateStartEndPair dateForReport);
    }
}