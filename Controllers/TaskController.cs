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
        public ActionResult<Note> EditPatchBody(int id, [FromBody] PatchBody body)
        {
            if (!tasks.Exists(task => task.Id == id))
            {
                return NotFound();
            }
           
            tasks.Where(task => task.Id == id).FirstOrDefault(t => t.Done = body.Done);
            Note note = tasks.Where(task => task.Id == id).First();
            
            return Ok(note);
        }
        
    }
}