using System;
using System.Collections;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        public void CreateCategory(CategoryDto category);
        public void DeleteCategory(Guid categoryId);
        public void UpdateCategory(CategoryDto categoryDto);
        public IEnumerable<CategoryDto> GetCategories();
        public CategoryDto GetCategory(Guid id);
    }
}