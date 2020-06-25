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
    [Route("api/time/")]
     public class TimeController : ControllerBase
        {
            public TimeController(AppDb db)
            {
                Db = db;
            }

            // GET api/time
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new TimeQuery(Db);
                var result = await query.LatestPostsTime();
                return new OkObjectResult(result);
            }

            // GET api/group/1
            [HttpGet("{time_id_all}")]
            public async Task<IActionResult> GetOne(int time_id_all)
            {
                await Db.Connection.OpenAsync();
                var query = new TimeQuery(Db);
                var result = await query.FindOneTime(time_id_all);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/time
            [HttpPost]
            [Route("post")]
            public async Task<IActionResult> Post([FromBody] Time body)
            {
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertTime();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/group/5
            [HttpPut("{time_id_all}")]
            public async Task<IActionResult> PutOne(int time_id_all, [FromBody] Time body)
            {
                await Db.Connection.OpenAsync();
                var query = new TimeQuery(Db);
                var result = await query.FindOneTime(time_id_all);
                if (result is null)
                    return new NotFoundResult();
                result.id_timeName = body.id_timeName;
                result.id_days = body.id_days;
                await result.UpdateTime();
                return new OkObjectResult(result);
            }

            // DELETE api/time/5
            [HttpDelete("{time_id_all}")]
            public async Task<IActionResult> DeleteOne(int time_id_all)
            {
                await Db.Connection.OpenAsync();
                var query = new TimeQuery(Db);
                var result = await query.FindOneTime(time_id_all);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteTime();
                return new OkResult();
            }

            // DELETE api/time
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new TimeQuery(Db);
                await query.DeleteAllTimes();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}