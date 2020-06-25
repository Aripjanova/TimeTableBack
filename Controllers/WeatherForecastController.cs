/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
  //  [ApiController]
    [Route("api/group/")]
     public class WeatherForecastController : ControllerBase
        {
            public WeatherForecastController(AppDb db)
            {
                Db = db;
            }

            // GET api/group
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new GroupsQuery(Db);
                var result = await query.LatestPostsGroups();
                return new OkObjectResult(result);
            }

            // GET api/group/1
            [HttpGet("{id_groups}")]
            public async Task<IActionResult> GetOne(int id_groups)
            {
                await Db.Connection.OpenAsync();
                var query = new GroupsQuery(Db);
                var result = await query.FindOneGroups(id_groups);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/group
            [HttpPost]
            [Route("post")]
            public async Task<IActionResult> Post([FromBody] Groups body)
            {
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertGroup();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/group/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutOne(int id_groups, [FromBody] Groups body)
            {
                await Db.Connection.OpenAsync();
                var query = new GroupsQuery(Db);
                var result = await query.FindOneGroups(id_groups);
                if (result is null)
                    return new NotFoundResult();
                result.name = body.name;
                result.id_class = body.id_class;
                await result.UpdateGroup();
                return new OkObjectResult(result);
            }

            // DELETE api/group/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteOne(int id)
            {
                await Db.Connection.OpenAsync();
                var query = new GroupsQuery(Db);
                var result = await query.FindOneGroups(id);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteGroup();
                return new OkResult();
            }

            // DELETE api/group
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new GroupsQuery(Db);
                await query.DeleteAllGroups();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}
*/