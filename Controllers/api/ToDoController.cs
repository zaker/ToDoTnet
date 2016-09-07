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


        public async Task<IActionResult> Read(string id)
        {
            Guid todoID;
            if (!Guid.TryParse(id, out todoID))
            {
                return null;
            }
            var gID = Guid.Parse(id);
            var uID = Ctx.Users.First(u => u.Name == HttpContext.User.Identity.Name).UserID;
            var item = await (from t in Ctx.ToDos
                              where t.ToDoID == gID && t.UserID == uID
                              select t).FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }

            return OkOrNotFound(new ToDoTaskModel(item.ToDoID)
            {
                Done = item.DoneDate.HasValue,
                Description = item.Description,
                Title = item.Title,
                Priority = item.Priority,
                Product = item.Product,
                Type = item.Type
            });



        }


        [HttpGet]
        public async Task<List<ToDoTaskModel>> Get()
        {
            var userName = HttpContext.User.Identity.Name;

            var uID = Ctx.Users.First(u => u.Name == userName).UserID;

            var items = await (from t in Ctx.ToDos
                               where t.UserID == uID
                               orderby t.DoneDate descending, t.Priority descending
                               select t).ToListAsync();

            List<ToDoTaskModel> tt = new List<ToDoTaskModel>();

            items.ForEach(item => tt.Add(new ToDoTaskModel(item.ToDoID)
            {
                Done = item.DoneDate.HasValue,
                Description = item.Description,
                Title = item.Title,
                Priority = item.Priority,
                Product = item.Product,
                Type = item.Type
            }));

            return tt;
        }



        [HttpGet]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<List<ToDoTask>> GetAll()
        {
            var items = await (from t in Ctx.ToDos
                               orderby t.DoneDate descending, t.Priority descending
                               select t).ToListAsync();

            List<ToDoTask> tt = new List<ToDoTask>();

            items.ForEach(t => tt.Add(new ToDoTask(t)));


            return tt;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ToDoTaskModel todoTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userName = HttpContext.User.Identity.Name;

            var uID = Ctx.Users.First(u => u.Name == userName).UserID;
            var dbEnt = new ToDo()
            {
                UserID = uID,
                Title = todoTask.Title,
                Description = todoTask.Description,
                Product = todoTask.Product,
                Type = todoTask.Type,
                Priority = todoTask.Priority
            };
            Ctx.ToDos.Add(dbEnt);

            await Ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dbEnt.ToDoID }, todoTask);
        }


        public async Task<IActionResult> Update([FromBody]ToDoTaskModel todoTask)
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
            if (!Guid.TryParse(todoTask.ID, out todoID))
            {
                return NotFound();
            }

            var userName = HttpContext.User.Identity.Name;

            var uID = Ctx.Users.First(u => u.Name == userName).UserID;

            var dbTask = await (from t in Ctx.ToDos
                                where t.ToDoID == todoID && t.UserID == uID
                                select t).FirstOrDefaultAsync();

            if (dbTask == null)
            {
                return NotFound();
            }

            dbTask.Title = todoTask.Title;
            dbTask.Description = todoTask.Description;
            dbTask.Product = todoTask.Product;
            dbTask.Type = todoTask.Type;
            if (!dbTask.DoneDate.HasValue && todoTask.Done)
            {
                dbTask.DoneDate = DateTime.UtcNow;
            }
            dbTask.Priority = todoTask.Priority;


            try
            {

                await Ctx.SaveChangesAsync();
                return Ok(new ToDoTask(dbTask));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Ctx.ToDos.Any(a => a.ToDoID == todoID))
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
            if (Guid.TryParse(id, out gID))
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
        private IActionResult OkOrNotFound(object result)
        {
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}