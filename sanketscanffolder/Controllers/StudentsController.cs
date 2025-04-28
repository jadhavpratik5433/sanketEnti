using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sanketscanffolder.Data;
using sanketscanffolder.Models;

namespace sanketscanffolder.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MVCDemoDbContext _context;

        public StudentsController(MVCDemoDbContext context)
        {
            //HttpContext.Session.SetInt32("StudentId", 0);
            _context = context;
        }

        //GET: Students
        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetInt32("StudentId") > 0)
            //{
            //    return RedirectToAction("GetByStudent", new { studentId = HttpContext.Session.GetInt32("StudentId") });
            //}
            return View(await _context.Students.ToListAsync());
        }

        //public IActionResult Index(int? selectedStudentId)
        //{
        //    var students = _context.Students.ToList();
        //    var semesters = selectedStudentId != null
        //                    ? _context.Semesters.Where(s => s.studId == selectedStudentId).ToList()
        //                    : new List<Semester>();

        //    var viewModel = new StudentSemesterViewModel
        //    {
        //        Students = students,
        //        Semesters = semesters,
        //        SelectedStudentId = selectedStudentId
        //    };

        //    return View(viewModel);
        //}


        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Stuent_Name,Age,Address")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Stuent_Name,Age,Address")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        public IActionResult GetByStudent(int studentId,string studentName)
        {
            TempData["StudentId"] = studentId;
            TempData["StudentName"] = studentName;
            var semesters = _context.Semesters
                .Where(s => s.studId == studentId)
                .ToList();

            return PartialView("GetByStudent", semesters);
        }

    }
}
