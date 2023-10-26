using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "Display Order and Name Can't be Same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Created Sucessfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Unable to create";
            return View();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Category? editableObj = _unitOfWork.Category.Get(c => c.Id == Id);
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
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                //RedirectToAction(action_name,Cont_namea)
                TempData["Success"] = "Edited Sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Category? neededObj = _unitOfWork.Category.Get(c => c.Id == Id);
            if (neededObj == null)
            {
                return NotFound();
            }
            return View(neededObj);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Category? foundObj = _unitOfWork.Category.Get(c => c.Id == Id);
            if (foundObj == null) { return NotFound(); }
            _unitOfWork.Category.Remove(foundObj);
            _unitOfWork.Save();
            TempData["Success"] = "Deleted Sucessfully";
            return RedirectToAction("Index");
        }


    }
}
