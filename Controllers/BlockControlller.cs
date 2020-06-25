
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
    [Route("api/block/")]
     public class BlockControlller : ControllerBase
        {
            public BlockControlller(AppDb db)
            {
                Db = db;
            }

            // GET api/block
            [HttpGet]
            public async Task<IActionResult> GetLatest()
            {
                await Db.Connection.OpenAsync();
                var query = new BlockQuery(Db);
                var result = await query.LatestPostsBlock();
                return new OkObjectResult(result);
            }

            // GET api/block/1
            [HttpGet("{id_groups}")]
            public async Task<IActionResult> GetOne(int id_blocks)
            {
                await Db.Connection.OpenAsync();
                var query = new BlockQuery(Db);
                var result = await query.FindOneBlock(id_blocks);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/block
            [HttpPost]
            [Route("/post")]
            public async Task<IActionResult> Post([FromBody] Block body)
            {
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertBlock();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/block/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutOne(int id_block, [FromBody] Block body)
            {
                await Db.Connection.OpenAsync();
                var query = new BlockQuery(Db);
                var result = await query.FindOneBlock(id_block);
                if (result is null)
                    return new NotFoundResult();
                result.id_teacher = body.id_teacher;
                result.id_subject = body.id_subject;
                result.id_groups = body.id_groups;
                result.id_typeBlock = body.id_typeBlock;
                result.countBlock = body.countBlock;
                await result.UpdateBlock();
                return new OkObjectResult(result);
            }

            // DELETE api/block/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteOne(int id)
            {
                await Db.Connection.OpenAsync();
                var query = new BlockQuery(Db);
                var result = await query.FindOneBlock(id);
                if (result is null)
                    return new NotFoundResult();
                await result.DeleteBlock();
                return new OkResult();
            }

            // DELETE api/block
            [HttpDelete]
            public async Task<IActionResult> DeleteAll()
            {
                await Db.Connection.OpenAsync();
                var query = new BlockQuery(Db);
                await query.DeleteAllBlocks();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}