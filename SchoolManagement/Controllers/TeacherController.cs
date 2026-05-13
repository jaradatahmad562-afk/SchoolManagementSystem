//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SchoolManagement.Data;
//using SchoolManagement.Models;

//namespace SchoolManagement.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TeachersController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public TeachersController(AppDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
//        {
//            return await _context.Teachers.AsNoTracking().ToListAsync();
//        }

//        [HttpPost]
//        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
//        {
//            teacher.Subjects = null;

//            _context.Teachers.Add(teacher);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetTeachers), new { id = teacher.Id }, teacher);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTeacher(int id)
//        {
//            var teacher = await _context.Teachers.FindAsync(id);
//            if (teacher == null) return NotFound();

//            _context.Teachers.Remove(teacher);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
//        {
//            if (id != teacher.Id) return BadRequest();

//            _context.Entry(teacher).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!_context.Teachers.Any(e => e.Id == id)) return NotFound();
//                else throw;
//            }

//            return NoContent();
//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return teacher;
        }

        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return Ok(teacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id) return BadRequest();

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Teachers.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}