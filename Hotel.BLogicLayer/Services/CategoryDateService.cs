using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class CategoryDateService : ICategoryDateService
    {
        private IUnitOfWork _dataBaseUnitOfWork;
        private IMapperItem _mapperItem;
        
        public CategoryDateService(IMapperItem mapperItem, IUnitOfWork dataBaseUnitOfWork)
        {
            _mapperItem = mapperItem;
            _dataBaseUnitOfWork = dataBaseUnitOfWork;
        }


        public void CreateCategoryDate(CategoryDateDto categoryDate)
        {
            if (categoryDate.CategoryId == default(Guid))
            {
                throw new ArgumentException("Сan't create categoryDate with empty category");
            }

            //TODO MAKE REPO METHODS AND MAKE LOGIC IN DB
            var categoryDates = GetCategoryDates().Where(ct => ct.CategoryId == categoryDate.CategoryId);
            var categoryDateDtos = categoryDates as CategoryDateDto[] ?? categoryDates.ToArray();
            if (categoryDateDtos.Count() != 0)
            {
                var categoryWithNullEndDate = categoryDateDtos
                    .FirstOrDefault(item => item.EndDate == null);
                if (categoryWithNullEndDate == null)
                {
                    var containedCategoryDateWithEqualDate = categoryDateDtos.Any(item =>
                        categoryDate.StartDate - item.EndDate < TimeSpan.FromDays(1));
                    if (!containedCategoryDateWithEqualDate)
                    {
                        throw new Exception("Db integrity failed with Entiy CategoryDAte");
                    }
                }
                else
                {
                    categoryWithNullEndDate.EndDate = categoryDate.StartDate;
                    _dataBaseUnitOfWork.CategoryDates.Update(
                        _mapperItem.Mapper.Map<CategoryDate>(categoryWithNullEndDate));
                }

            }
            
            var categoryDateFromDto = _mapperItem.Mapper.Map<CategoryDate>(categoryDate);
            _dataBaseUnitOfWork.CategoryDates.Create(categoryDateFromDto);
            _dataBaseUnitOfWork.Save();
        }

        public void DeleteCategoryDate(Guid categoryDateId)
        {
            var categoryDateEntity = GetCategoryDate(categoryDateId);

            var categoryDatesWithEqualCategoryId = GetCategoryDates()
                .Where(date => date.CategoryId == categoryDateEntity.CategoryId);

            var datesWithEqualCategoryId = categoryDatesWithEqualCategoryId as CategoryDateDto[] ?? categoryDatesWithEqualCategoryId.ToArray();
            if (datesWithEqualCategoryId.Count() != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(categoryDateId));
            }
            else
            {
                var previousCategoryDate = datesWithEqualCategoryId
                    .FirstOrDefault(item => categoryDateEntity.StartDate - item.EndDate <= TimeSpan.FromDays(1));

                if (previousCategoryDate != null)
                {
                    previousCategoryDate.EndDate = null;
                    
                    _dataBaseUnitOfWork.CategoryDates.Update(
                        _mapperItem.Mapper.Map<CategoryDate>(previousCategoryDate));
                }
                else
                {
                    throw new Exception("Db error");
                }
            }
                
            _dataBaseUnitOfWork.CategoryDates.Delete(categoryDateId);
        }

        public void UpdateCategoryDate(CategoryDateDto categoryDateToUpdate)
        {
            
            //TODO MAKE REPO MEHODS AND MAKE LOGIC IN MEMORY
            var dates = GetCategoryDates().Where(ct => ct.CategoryId == categoryDateToUpdate.Id);

            if (categoryDateToUpdate.EndDate != null && categoryDateToUpdate.StartDate > categoryDateToUpdate.EndDate)
            {
                (categoryDateToUpdate.StartDate, categoryDateToUpdate.EndDate) =
                    ((DateTime) categoryDateToUpdate.EndDate, categoryDateToUpdate.StartDate);
            }
            
            if (dates.All(ct => (ct.EndDate ?? DateTime.MinValue) <= categoryDateToUpdate.StartDate))
            {
                _dataBaseUnitOfWork.CategoryDates.Update(
                                _mapperItem.Mapper.Map<CategoryDate>(categoryDateToUpdate));
            }
            else
            {
                throw new ArgumentException("Can't update CategoryDate that date Range Intersects with other");
            }
            
        }

        public IEnumerable<CategoryDateDto> GetCategoryDates()
        {
            return _mapperItem.Mapper.Map<IEnumerable<CategoryDate>, IEnumerable<CategoryDateDto>>(
                _dataBaseUnitOfWork.CategoryDates.GetAll());
        }

        public CategoryDateDto GetCategoryDate(Guid id)
        {
            
            var categoryWithParamId = _dataBaseUnitOfWork.CategoryDates.Get(id);
            return _mapperItem.Mapper.Map<CategoryDateDto>(categoryWithParamId);
        }

        public void Dispose()
        {
            _dataBaseUnitOfWork?.Dispose();
        }
    }
}