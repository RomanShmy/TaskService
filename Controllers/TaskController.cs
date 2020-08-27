using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using models;
using System.Linq;
using System;
using Task;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace controler
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Note>> GetNotes()
        {
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;
            return Ok(context.GetAllTasks());
        }

        [HttpPost]
        public ActionResult<Note> AddNote([FromBody] Note note)
        {
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;
            context.SaveTask(note);
            return Ok(note);
        }
        
        [HttpPatch("{id}")]
        public ActionResult EditPatchBody(int id, [FromBody]Note edit)
        {
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;
            var task = context.GetTaskById(id);
            if (task.Name == null)
            {
                return NotFound();
            }

            task.Done = edit.Done;

            var result = context.UpdateTaskDone(id, task);

            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var context = HttpContext.RequestServices.GetService(typeof(TaskService)) as TaskService;
            var result = context.Delete(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
        
    }
}