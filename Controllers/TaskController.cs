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
    [Route("[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private static List<Note> tasks = new List<Note>(){new Note(){Id = 1, Name = "buy car", Done = false}};
    
        [HttpGet]
        public ActionResult<List<Note>> GetNotes()
        {
            return Ok(tasks);
        }

        [HttpPost]
        public ActionResult<Note> AddNote([FromBody] Note note)
        {
            tasks.Add(note);
            return Ok(note);
        }

        [HttpPatch("{id}")]
        public ActionResult<Note> EditPatchBody(int id, [FromBody] bool done)
        {
            var task = tasks.FirstOrDefault(task => task.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            task.Done = done;
            
            return Ok(task);
        }
        
    }
}