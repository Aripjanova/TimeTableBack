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
    [Route("api/room/")]
     public class ClassRoomController : ControllerBase
        {
            public ClassRoomController(AppDb db)
            {
                Db = db;
            }

            // GET api/room
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new ClassRoomQuery(Db);
                var result = await query.LatestPostsClassRoom();
                return new OkObjectResult(result);
            }

            // GET api/room/1
            [HttpGet("{id_classRoom}")]
            public async Task<IActionResult> GetOne(int id_classRoom)
            {
                await Db.Connection.OpenAsync();
                var query = new ClassRoomQuery(Db);
                var result = await query.FindOneRoom(id_classRoom);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/room/post
            [HttpPost]
       
            public async Task<IActionResult> Post([FromBody] ClassRoom body)
            { Console.WriteLine("aikaiakaiak"+body.ID);
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertClassRoom();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/room/5
            [HttpPut("{id_classRoom}")]
            public async Task<IActionResult> PutOne(int id_classRoom, [FromBody] ClassRoom body)
            {
                await Db.Connection.OpenAsync();
                var query = new ClassRoomQuery(Db);
                var result = await query.FindOneRoom(id_classRoom);
                if (result is null)
                    return new NotFoundResult();
                result.ID = body.ID;
                result.name = body.name;
                await result.UpdateClassRoom();
                return new OkObjectResult(result);
            }

            // DELETE api/room/5
            [HttpDelete("{id_classRoom}")]
            public async Task<IActionResult> DeleteOne(int id)
            {
                await Db.Connection.OpenAsync();
                var query = new ClassRoomQuery(Db);
                var result = await query.FindOneRoom(id);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteClassRoom();
                return new OkResult();
            }

            // DELETE api/room
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new ClassRoomQuery(Db);
                await query.DeleteAllClassRooms();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}