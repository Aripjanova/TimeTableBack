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
    [Route("api/teacher/")]
     public class TeacherController : ControllerBase
        {
            public TeacherController(AppDb db)
            {
                Db = db;
            }

            // GET api/teacher
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new TeacherQuery(Db);
                var result = await query.LatestPostsTeacher();
                return new OkObjectResult(result);
            }

            // GET api/group/1
            [HttpGet("{id_teacher}")]
            public async Task<IActionResult> GetOne(int id_teacher)
            {
                await Db.Connection.OpenAsync();
                var query = new TeacherQuery(Db);
                var result = await query.FindOneTeacher(id_teacher);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/teacher/post
            [HttpPost]
            [Route("post")]
             public async Task<IActionResult> Post([FromHeader] Teacher body)
            {
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertTeacher();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/teacher/5
            [HttpPut("{id_teacher}")]
            public async Task<IActionResult> PutOne(int id_teacher, [FromBody] Teacher body)
            {   Console.WriteLine("lk"+body.first_name);
                Console.WriteLine(id_teacher);
                await Db.Connection.OpenAsync();
                var query = new TeacherQuery(Db);
                var result = await query.FindOneTeacher(id_teacher);
                Console.WriteLine(result);
                if (result is null)
                    return new NotFoundResult();
                result.first_name = body.first_name;
                result.last_name = body.last_name;
             
                await result.UpdateTeacher();
                return new OkObjectResult(result);
            }

            // DELETE api/teacher/5
            [HttpDelete("{id_teacher}")]
            public async Task<IActionResult> DeleteOne(int id_teacher)
            {
                Console.WriteLine("id_teacher"+id_teacher);
                await Db.Connection.OpenAsync();
                var query = new TeacherQuery(Db);
                var result = await query.FindOneTeacher(id_teacher);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteTeacher();
                return new OkResult();
            }

            // DELETE api/teacher
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new TeacherQuery(Db);
                await query.DeleteAllTeacher();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}