using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext context;
        public ToDoController(ToDoContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ToDoItem> toDoItems = await context.ToDoItems.ToListAsync();
            return View(toDoItems);
        }

        public IActionResult TaskForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TaskForm(ToDoItem toDoItem)
        {
            if (toDoItem.Status == "Open this select menu")
            {
                ModelState.AddModelError("Status", "Status can`t be by default, " +
                    "choose status of your task");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ModelState.ErrorCount);
                return View();
            }

            toDoItem.CreatedAt = DateTime.Now;
            await context.ToDoItems.AddAsync(toDoItem);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Complete(int? id)
        {
            if (id != null)
            {
                var item = await context.ToDoItems.FindAsync(id);

                item.Status = "Done";
                context.ToDoItems.Update(item);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var item = await context.ToDoItems.FindAsync(id);
                return View(item);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ToDoItem item, int id, DateTime createdAt, string oldStatus, string oldTitle, string oldDescription)
        {
            if (ModelState.IsValid)
            {
                if (!IsSimmilar(item, oldStatus, oldTitle, oldDescription))
                {
                    item.Id = id;
                    item.CreatedAt = createdAt;
                    context.ToDoItems.Update(item);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool IsSimmilar(ToDoItem item, string oldStatus, string oldTitle, string oldDescription)
        {
            if (item.Status == oldStatus && item.Description == oldDescription
                && item.Title == oldTitle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var item = await context.ToDoItems.FindAsync(id);

                return View(item);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await context.ToDoItems.FindAsync(id);

            context.ToDoItems.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
