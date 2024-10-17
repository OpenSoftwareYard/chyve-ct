using System;

namespace Services;

public interface IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<TDto?> GetById(Guid id);
    Task<IEnumerable<TDto>> GetAll();
    Task<TDto> Create(TDto dto);
    Task<TDto?> Update(Guid id, TDto dto);
    Task<TDto?> Delete(Guid id);
}
