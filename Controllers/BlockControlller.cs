
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
                var query = new Blokquery(Db);
                var result = await query.LatestPostsBlock();
                return new OkObjectResult(result);
            }

            // GET api/block/1
            [HttpGet("{id_block}")]
            public async Task<IActionResult> GetOne(int id_block)
            {
                await Db.Connection.OpenAsync();
                var query = new BlockOne(Db);
                var result = await query.FindOneBlock(id_block);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }

            // POST api/block
            [HttpPost]
       //     [Route("")]
            public async Task<IActionResult> Post([FromHeader] Block body)
            {   Console.WriteLine("block id_block"+body.id_block);
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertBlock();
                Console.WriteLine(body);
                return new OkResult();
            }

            // PUT api/block/5
            [HttpPut("{id_block}")]
            public async Task<IActionResult> PutOne(int id_block, [FromBody] Block body)
            {
                await Db.Connection.OpenAsync();
                var query = new Blokquery(Db);
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
            [HttpDelete("{id_block}")]
            
            public async Task<IActionResult> DeleteOne(int id_block)
            {
                Console.WriteLine("id_block"+id_block);
                await Db.Connection.OpenAsync();
                var query = new BlockOne(Db);
                var result = await query.FindOneBlock(id_block);
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
                var query = new BlockOne(Db);
                await query.DeleteAllBlock();
                return new OkResult();
            }

            public AppDb Db { get; }
        }
}