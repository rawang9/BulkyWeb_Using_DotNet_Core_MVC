using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductsList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductsList);
        }
        public IActionResult Create() {
            //IEnumerable<SelectListItem> CategoryList = 
            //ViewBag.CategoryList = CategoryList;
            ProductVM ProductVM = new ProductVM() {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                };
            return View(ProductVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM) {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product
                    .Add(productVM.Product);
                _unitOfWork.Save();
                TempData["Success"] = "Product Created";
                return RedirectToAction("Index");
            }
            else {
                TempData["error"] = "Failed to Add the Product";
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
                return View();
            }
            
        }
        public IActionResult Edit(int ?Id) {
            if (Id == null) { return NotFound(); }
            Product? editableObj = _unitOfWork.Product.Get(x => x.Id == Id);
            if (editableObj == null) {
                return NotFound();
            }
            return View(editableObj);
        }
        [HttpPost,ActionName("Edit")]
        public IActionResult EditProduct(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Updated Product Successfully!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Failed to Add the Product";
            return View();
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Product? deleteableObj = _unitOfWork.Product.Get(x => x.Id == Id);
            if (deleteableObj == null)
            {
                return NotFound();
            }
            return View(deleteableObj);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int? Id)
        {
            if (Id == null) { return NotFound(); }
            Product? deletabeObj = _unitOfWork.Product.Get(x => x.Id == Id);
            if (deletabeObj == null)
            {
                TempData["error"] = "Failed to Delete the Product";
                return View();
            }
            _unitOfWork.Product.Remove(deletabeObj);
            _unitOfWork.Save();
            TempData["Success"] = "Deleted Product Successfully!";
            return RedirectToAction("Index");
        }

    }
}
