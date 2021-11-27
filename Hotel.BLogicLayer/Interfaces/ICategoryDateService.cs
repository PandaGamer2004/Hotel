using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface ICategoryDateService : IDisposable
    {
        public CategoryDateDto GetCategoryDateWithGivenCategoryId(Guid categoryId);
        public void CreateCategoryDate(CategoryDateDto categoryDate);
        public void DeleteCategoryDate(Guid categoryDateId);
        public void UpdateCategoryDate(CategoryDateDto categoryDateToUpdate);
        public IEnumerable<CategoryDateDto> GetCategoryDates();
        public CategoryDateDto GetCategoryDate(Guid id);
    }
}