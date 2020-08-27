using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using models;
using System.Linq;
using System;
using Task;
using Microsoft.Extensions.Logging;

namespace controler
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        [HttpGet]
        public ActionResult<List<Note>> GetNotes()
        {
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;
            return Ok(context?.GetAllTasks());
        }

        [HttpPost]
        public ActionResult<Note> AddNote([FromBody] Note note)
        {
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;
            context?.SaveTask(note);
            return Ok(note);
        }
        
        [HttpPatch("{id}")]
        public ActionResult EditPatchBody(int id, [FromBody] bool done)
        {
            
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;

            var result = context.UpdateTaskDone(id, done);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
        
    }
}