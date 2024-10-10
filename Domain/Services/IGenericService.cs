using System;

namespace Services;

public interface IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<TDto?> GetById(int id);
    Task<IEnumerable<TDto>> GetAll();
    Task<TDto> Create(TDto dto);
    Task<TDto?> Update(int id, TDto dto);
    Task<TDto?> Delete(int id);
}
