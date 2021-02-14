using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dbir.Dto;

namespace iidx_dbir_dotnet_blazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicResultController : ControllerBase
    {
        private readonly ILogger<MusicResultController> _logger;

        public MusicResultController(ILogger<MusicResultController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MusicMstResult> Get()
        {
            // TODO: SQLで取得する
            var musicMstResultList = new List<MusicMstResult>();
            MusicMstResult musicMstResult;

            musicMstResult = new MusicMstResult();
            musicMstResult.Name = "V2";
            musicMstResult.PlayStyle = "SP";
            musicMstResult.ChartsType = "HYPER";
            musicMstResult.Mode = "DBR";
            musicMstResultList.Add(musicMstResult);

            musicMstResult = new MusicMstResult();
            musicMstResult.Name = "Cyber Force";
            musicMstResult.PlayStyle = "SP";
            musicMstResult.ChartsType = "ANOTHER";
            musicMstResult.Mode = "DBR";
            musicMstResultList.Add(musicMstResult);

            return musicMstResultList.ToArray();
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}
