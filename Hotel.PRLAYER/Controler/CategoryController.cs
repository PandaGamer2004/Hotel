using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Models;
using Hotel.PRLAYER.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.PRLAYER.Controler
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private IMapperItem _mapper;
        private ICategoryService _categoryService;
        private ICategoryDateService _categoryDateService;
        public CategoryController(IMapperItem mapper, ICategoryService categoryService, ICategoryDateService categoryDateService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _categoryDateService = categoryDateService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCategories();
            if (!categories.Any())
            {
                return NoContent();
            }

            return Json(_mapper.Mapper.Map<IEnumerable<CategoryModel>>(categories));
        }

        [HttpPost]
        public IActionResult CreateCategory([Bind("CategoryName", "BedCount")]CategoryModel categoryModel, Decimal price)
        {
            if (ModelState.IsValid)
            {
                if (price <= 0)
                {
                    ModelState.AddModelError("", "Price can't be equal a lower than zero");
                }
                else
                {

                    categoryModel.Id = Guid.NewGuid();
                    CreateCategoryDate(price, categoryModel.Id);
                    var mappedCategoryModel = _mapper.Mapper.Map<CategoryDto>(categoryModel);

                    _categoryService.CreateCategory(mappedCategoryModel);
                    return Created(HttpContext.Request.Path.Value, categoryModel);
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPut]
        public IActionResult UpdateCategory([Bind("Id", "CategoryName", "BedCount")]CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                if (_categoryService.GetCategory(categoryModel.Id) == null)
                {
                    
                    ModelState.AddModelError("", "Can't find category to update");
                    var mappedCategory = _mapper.Mapper.Map<CategoryDto>(categoryModel);
                    _categoryService.UpdateCategory(mappedCategory);

                    return Ok();
                }
            }

            return BadRequest(ModelState);
        }


        [HttpPut]
        public IActionResult UpdateCategoryDate([Bind("Id", "StartDate", "EndDate", "Price", "CategoryId")]
            CategoryDateModel categoryDateModel)
        {
            if (ModelState.IsValid)
            {
                if (_categoryDateService.GetCategoryDate(categoryDateModel.Id) != null && _categoryService.GetCategory(categoryDateModel.CategoryId) != null)
                {
                    try
                    {
                        var mappedCategoryDateToDto = _mapper.Mapper.Map<CategoryDateDto>(categoryDateModel);
                        _categoryDateService.UpdateCategoryDate(mappedCategoryDateToDto);
                    }
                    catch (ArgumentException ae)
                    {
                        Debug.WriteLine(ae.Message);
                        ModelState.AddModelError("StartDate", "Can't update CategoryDate that's intersects with other");
                    }
                }
                else
                {
                    ModelState.AddModelError("CategoryId", "Can't find category or categoryDate to Update.");
                }
            }

            return BadRequest(ModelState);
        }


        [HttpDelete]
        public IActionResult DeleteCategory(Guid categoryIdToDelete)
        {
            if (ModelState.IsValid)
            {
                if (_categoryService.GetCategory(categoryIdToDelete) != null)
                {
                    _categoryService.DeleteCategory(categoryIdToDelete);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("","Can't delete Category with given Id");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public IActionResult DeleteCategoryDate(Guid categoryDateToDelete)
        {
            if(ModelState.IsValid)
            {
                if (_categoryDateService.GetCategoryDate(categoryDateToDelete) != null)
                {
                    _categoryDateService.DeleteCategoryDate(categoryDateToDelete);
                }
                else
                {
                    ModelState.AddModelError("", "Can't delete CategoryDate with given Id");
                }
            }

            return BadRequest(ModelState);
        }
        

        private void CreateCategoryDate(Decimal price, Guid categoryModelId)
        {
            var categoryDate = new CategoryDateModel()
            {
                Id = Guid.NewGuid(),
                CategoryId = categoryModelId,
                StartDate = DateTime.Now,
                EndDate = null,
                Price = price,
            };
            
            _categoryDateService.CreateCategoryDate(_mapper.Mapper.Map<CategoryDateDto>(categoryDate));
        }
    }
}