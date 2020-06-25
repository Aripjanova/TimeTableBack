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
    [EnableCors("AllowAllOrigin")]
  //  [ApiController]
    [Route("api/subject/")]
     public class SubjectController : ControllerBase
        {
            public SubjectController(AppDb db)
            {
                Db = db;
            }

            // GET api/subject
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new SubjectQuery(Db);
                var result = await query.LatestPostsSubject();
                Console.WriteLine("Aika");
                return new OkObjectResult(result);
            }

            // GET api/subject/1
            [HttpGet("{id_subject}")]
            public async Task<IActionResult> GetOne(int id_subject)
            {
                await Db.Connection.OpenAsync();
                var query = new SubjectQuery(Db);
                var result = await query.FindOneSubject(id_subject);
                if (result is null)
                    return new NotFoundResult();
              
                return new OkObjectResult(result);
            }

            // POST api/subject/post
            [HttpPost]
            [Route("post")]
            public async Task<IActionResult> Post([FromHeader] Subject body)
            {
                Console.WriteLine(body.name);
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertSubject();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/subject/5
            [HttpPut("{id_subject}")]
            public async Task<IActionResult> PutOne(int id_subject, [FromBody] Subject body)
            {
                await Db.Connection.OpenAsync();
                var query = new SubjectQuery(Db);
                var result = await query.FindOneSubject(id_subject);
                if (result is null)
                    return new NotFoundResult();
                result.id_subject = body.id_subject;
                result.name = body.name;
                await result.UpdateSubject();
                return new OkObjectResult(result);
            }

            // DELETE api/subject/5
            [HttpDelete("{id_subject}")]
            public async Task<IActionResult> DeleteOne(int id_subject)
            {
                await Db.Connection.OpenAsync();
                var query = new SubjectQuery(Db);
                var result = await query.FindOneSubject(id_subject);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteSubject();
                return new OkResult();
            }

            // DELETE api/group
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new SubjectQuery(Db);
                await query.DeleteAllSubject();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}