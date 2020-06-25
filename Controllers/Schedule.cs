using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using WebApplication.Models;
using WebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers{
    [EnableCors("MyPolicy")]
    //  [ApiController]
    [Route("api/scheduler/")]

    public class SetTableController : Controller
    {  
        public SetTableController(AppDb db)
            {
                Db = db;
     
        }
             // GET api/scheduler
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new SchedulerQuery(Db);
                var result = await query.Sheduler();
                return new OkObjectResult(result);
            }
            // GET api/scheduler/1
            [HttpGet("{id_block}")]
            public async Task<IActionResult> GetOne(int id_block)
            {
                await Db.Connection.OpenAsync();
                var query = new SchedulerQuery(Db);
                var result = await query.FindOneShedulerBlock(id_block);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/scheduler
            [HttpPost]
            
             public async Task<IActionResult> Post([FromHeader] Scheduler body)
            {
                Console.WriteLine("block body"+body.day);    
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.AddBlock();
                return new OkResult();
            }

            // PUT api/scheduler/5
            [HttpPut("{id_block}")]
            public async Task<IActionResult> PutOne(int id_block, [FromBody] Scheduler body)
            {
                await Db.Connection.OpenAsync();
                var query = new SchedulerQuery(Db);
                var result = await query.FindOneShedulerBlock(id_block);
                if (result is null)
                    return new NotFoundResult();
                result.day = body.day;
                result.hour = body.hour;
                result.group = body.group;
                result.teacher = body.teacher;
                await result.UpdateScheduleBlock();
                return new OkObjectResult(result);
            }

            // DELETE api/scheduler/5
            [HttpDelete("{id_block}")]
            public async Task<IActionResult> DeleteOne(int id_block)
            {
                await Db.Connection.OpenAsync();
                var query = new SchedulerQuery(Db);
                var result = await query.FindOneShedulerBlock(id_block);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteInSchedule();
                return new OkResult();
            }

            // DELETE api/scheduler
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new SchedulerQuery(Db);
                await query.DeleteAllSchedule();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}