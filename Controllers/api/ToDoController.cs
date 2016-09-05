using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoTnet.Models;
using ToDoTnet.DataEntities;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ToDoTnet.Controllers
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
            return new OkObjectResult(new ToDoTask(item.First()));
        }


        [HttpGet]
        public async Task<List<ToDo>> Get()
        {
            var userName = HttpContext.User.Identity.Name;

            var uID = Ctx.Users.First(u => u.Name == userName).UserID;
            return await  Ctx.ToDos.Where(t=> t.UserID == uID).ToListAsync();
        }

        
        public async Task<IActionResult> Get(string id)
        {
            var gID = Guid.Parse(id);
            return OkOrNotFound(await Ctx.ToDos.FirstAsync(a => a.ToDoID == gID));
        }

        [HttpGet]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<List<ToDo>> GetAll()
        {
            return await Ctx.ToDos.ToListAsync();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ToDoTask todoTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userName = HttpContext.User.Identity.Name;

            var uID = Ctx.Users.First(u => u.Name == userName).UserID;
            var dbEnt = new ToDo() {
                UserID =uID,
                Title = todoTask.Title,
                Description = todoTask.Description,
                Product = todoTask.Product,
                Type = todoTask.Type
            };
            Ctx.ToDos.Add(dbEnt);
            await Ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new {  id = todoTask.Id }, todoTask);
        }

        
        public async Task<IActionResult> Update([FromBody]ToDoTask todoTask)
        {
            if (todoTask == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid todoID;
            if (!Guid.TryParse(todoTask.Id, out todoID))
            {
                return NotFound();
            }
            try
            {
                Ctx.Update(todoTask);
                await Ctx.SaveChangesAsync();
                return Ok(todoTask);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Ctx.ToDos.Any(a=> a.ToDoID == todoID))
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