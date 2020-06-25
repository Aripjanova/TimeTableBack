using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using WebApplication.Models;
using WebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    [EnableCors("MyPolicy")]
  //  [ApiController]
    [Route("api/days/")]
     public class DayControllers : ControllerBase
        {
            public DayControllers(AppDb db)
            {
                Db = db;
            }

            // GET api/days
            
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new DayQuery(Db);
                var result = await query.GetAllDays();
                Console.WriteLine(result);
                return new OkObjectResult(result);
            }
           

    
            // PUT api/subject/5
            [HttpPut("{id_day}")]
            public async Task<IActionResult> PutOne(int id_day, [FromBody] Day body)
            {
                Console.WriteLine("id_day"+id_day+"body.name"+body.name+"bode.dayscol"+body.dayscol);
                await Db.Connection.OpenAsync();
                var query = new DayQuery(Db);
                var result = await query.FindOneDay(id_day);
                if (result is null)
                    return new NotFoundResult();
                result.id_day = body.id_day;
                result.name = body.name;
                result.dayscol = body.dayscol;
                await result.UpdateDays();
                return new OkObjectResult(result);
            }

           
            public AppDb Db { get; }
        }
}