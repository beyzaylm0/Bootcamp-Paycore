using PaycoreProject.Helpers;
using PaycoreProject.Model;
using System.Collections.Generic;

namespace PaycoreProject.Services.Abstract
{
    public interface ICategoryService
    {
        BaseResponse<CategoryDto> GetById(int id);
        BaseResponse<IEnumerable<CategoryDto>> GetAll();
        BaseResponse<CategoryDto> Insert(CategoryDto insertResource);
        BaseResponse<CategoryDto> Update(int id, CategoryDto updateResource);
        BaseResponse<CategoryDto> Remove(int id);

    }
}
