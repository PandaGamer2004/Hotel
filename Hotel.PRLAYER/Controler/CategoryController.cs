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
        public IActionResult CreateCategory(CategoryCreateModel categoryCreateModel)
        {
            if (ModelState.IsValid)
            {
                if (categoryCreateModel.Price <= 0)
                {
                    ModelState.AddModelError("", "Price can't be equal a lower than zero");
                }
                else
                {
                    try
                    {
                        var categoryModel = new CategoryModel
                        {
                            Id = Guid.NewGuid(),
                            BedCount = categoryCreateModel.BedCount,
                            CategoryName = categoryCreateModel.CategoryName
                        };

                        CreateCategoryDateAndSendToDb(categoryCreateModel.Price, categoryModel.Id);
                        var mappedCategoryModel = _mapper.Mapper.Map<CategoryDto>(categoryModel);

                        _categoryService.CreateCategory(mappedCategoryModel);
                        return Created(HttpContext.Request.Path.Value, categoryModel);
                    }
                    catch (ArgumentException ae)
                    {
                        Debug.WriteLine(ae.Message);
                        ModelState.AddModelError("", "It's not possible create category that already exists");
                    }
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPut]
        public IActionResult UpdateCategory(CategoryToUpdateModel categoryToUpdateModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoryModel = new CategoryModel
                    {
                        Id = categoryToUpdateModel.Id,
                        BedCount = categoryToUpdateModel.BedCount,
                        CategoryName = categoryToUpdateModel.CategoryName,
                    };

                    var categoryDateToUpdate =
                        _categoryDateService.GetCategoryDateWithGivenCategoryId(categoryModel.Id);

                    categoryDateToUpdate.Price = categoryToUpdateModel.Price;
  
                    var mappedCategory = _mapper.Mapper.Map<CategoryDto>(categoryModel);
                    _categoryService.UpdateCategory(mappedCategory);
                    _categoryDateService.UpdateCategoryDate(categoryDateToUpdate);

                    return Ok();
                }catch(ArgumentException ae)
                {
                    Debug.WriteLine(ae.Message);
                    return NotFound();
                }
              }
            
            return BadRequest(ModelState);
        }


        [HttpPut]
        [Route("CategoryDate")]
        public IActionResult UpdateCategoryDate([Bind("Id", "StartDate", "EndDate", "Price")]
            CategoryDateModel categoryDateModel)
        {
            if (ModelState.IsValid)
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
                catch(KeyNotFoundException ke)
                {
                    Debug.WriteLine(ke.Message);
                    return NotFound();
                }
                
            }

            return BadRequest(ModelState);
        }


        [HttpDelete]
        public IActionResult DeleteCategory(Guid categoryIdToDelete)
        {
            if (ModelState.IsValid)
            {
                try             
                {
                    _categoryService.DeleteCategory(categoryIdToDelete);
                    return Ok();
                }
                catch(ArgumentException ae)
                {
                    Debug.WriteLine(ae.Message);
                    return NotFound();
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("CategoryDate")]
        public IActionResult DeleteCategoryDate(Guid categoryDateToDelete)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _categoryDateService.DeleteCategoryDate(categoryDateToDelete);
                }
                catch(KeyNotFoundException ae)
                {
                    Debug.WriteLine(ae.Message);
                    return NotFound();
                }
            }

            return BadRequest(ModelState);
        }
        

        private void CreateCategoryDateAndSendToDb(Decimal price, Guid categoryModelId)
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