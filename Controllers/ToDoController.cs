using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoTnet.Models;
using ToDoTnet.DataEntities;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        public TodoController(ToDoContext ctx)
        {
            Ctx = ctx;
        }
        public ToDoContext Ctx { get; set; }

        [AllowAnonymous]
        public IEnumerable<ToDo> GetAll()
        {
            return Ctx.ToDos;
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
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


    }
}