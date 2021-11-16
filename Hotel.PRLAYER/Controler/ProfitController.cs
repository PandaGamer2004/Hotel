using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.Interfaces;
using Hotel.PRLAYER.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace Hotel.PRLAYER.Controler
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProfitController : Controller
    {
        private IReportInfoService _reportSerice;
        private IMapperItem _mapper;
        public ProfitController(IReportInfoService reportSerice, IMapperItem mapper)
        {
            _reportSerice = reportSerice;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("GetProfit")]
        public async Task<IActionResult> GetProfitReport(DateStartEndPair dateForReport)
        {

            if (ModelState.IsValid)
            {
                var reportModel = await _reportSerice.GetProfitReport(dateForReport);
                return Json(_mapper.Mapper.Map<ProfitReportModel>(reportModel));
            }

            return BadRequest(ModelState);
        }

        
    }
    

}