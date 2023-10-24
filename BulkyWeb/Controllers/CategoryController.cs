using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj) {
            if (obj.Name == obj.DisplayOrder.ToString()) {
                ModelState.AddModelError("", "Display Order and Name Can't be Same");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Created Sucessfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Unable to create";
            return View();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Category? editableObj = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (editableObj == null)
            {
                return NotFound();
            }
            return View(editableObj);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                //RedirectToAction(action_name,Cont_namea)
                TempData["Success"] = "Edited Sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Category? neededObj = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (neededObj == null)
            {
                return NotFound();
            }
            return View(neededObj);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Category? foundObj = _db.Categories.FirstOrDefault(c => c.Id == Id);
            if (foundObj == null) { return NotFound(); }
            _db.Categories.Remove(foundObj);
            _db.SaveChanges();
            TempData["Success"] = "Deleted Sucessfully";
            return RedirectToAction("Index");
        }


    }
}
