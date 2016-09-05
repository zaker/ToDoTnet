using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoTnet.Models;
using ToDoTnet.DataEntities;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
        public class TodoController : Controller
    {
        public TodoController(ToDoContext ctx)
        {
            Ctx = ctx;
        }
        public ToDoContext Ctx { get; set; }

       
        public IActionResult Read(string id)
        {
            Guid todoID;
            if (!Guid.TryParse(id, out todoID))
            {
                return null;
            }
            var item = from t in Ctx.ToDos
                       where t.ToDoID == todoID
                       select t;
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }


        [HttpGet]
        public async Task<List<ToDo>> Get()
        {
            return await  Ctx.ToDos.ToListAsync();
        }


        public async Task<IActionResult> Get(string id)
        {
            var gID = Guid.Parse(id);
            return OkOrNotFound(await Ctx.ToDos.FirstAsync(a => a.ToDoID == gID));
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ToDo todoTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Get
            Ctx.ToDos.Add(todoTask);
            await Ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { ToDo = todoTask.ToDoID }, todoTask);
        }

        
        public async Task<IActionResult> Update([FromBody]ToDo todoTask)
        {
            if (todoTask == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Ctx.Update(todoTask);
                await Ctx.SaveChangesAsync();
                return Ok(todoTask);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Ctx.ToDos.Any(a=> a.ToDoID == todoTask.ToDoID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            Guid gID;
            if (Guid.TryParse(id,out gID))
            {
                return NotFound();
            } 
            var todoTask = new ToDo
            {
                ToDoID = gID
            };
            Ctx.Attach(todoTask);
            Ctx.Remove(todoTask);
            await Ctx.SaveChangesAsync();
            return NoContent();
        }
        private IActionResult OkOrNotFound(object p)
        {
            throw new NotImplementedException();
        }
    }
}