using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class СategoryService : ICategoryService
    {
        private IUnitOfWork _databaseUnitOfWork;
        private IMapperItem _mapperItem;

        public СategoryService(IMapperItem mapperItem, IUnitOfWork databaseUnitOfWork)
        {
            _mapperItem = mapperItem;
            _databaseUnitOfWork = databaseUnitOfWork;
        }


        public void Dispose()
        {
            _databaseUnitOfWork?.Dispose();
        }

        public void CreateCategory(CategoryDto category)
        {
            var categoryFromDto = _mapperItem.Mapper.Map<Category>(category);
            _databaseUnitOfWork.Categories.Create(categoryFromDto);
            _databaseUnitOfWork.Save();
            
        }

        public void DeleteCategory(Guid categoryId)
        {
            _databaseUnitOfWork.Categories.Delete(categoryId);
            _databaseUnitOfWork.Save();
        }
        
        

        public void UpdateCategory(CategoryDto categoryDto)
        {
            _databaseUnitOfWork.Categories.Update(
                _mapperItem.Mapper.Map<Category>(categoryDto)    
                );
            _databaseUnitOfWork.Save();
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            return _mapperItem.Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(
                _databaseUnitOfWork.Categories.GetAll());
        }

        public CategoryDto GetCategory(Guid id)
        {
            return _mapperItem.Mapper.Map<CategoryDto>(
                _databaseUnitOfWork.Categories.Get(id));
        }
        
    }
}