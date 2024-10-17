using AutoMapper;
using Persistence.Data;

namespace Services;

public class GenericService<TEntity, TDto>(IGenericRepository<TEntity> repository, IMapper mapper) : IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IGenericRepository<TEntity> _repository = repository;
    protected readonly IMapper _mapper = mapper;

    public async Task<TDto> Create(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.Add(entity);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<TDto?> Delete(Guid id)
    {
        var entity = await _repository.Delete(id);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<IEnumerable<TDto>> GetAll()
    {
        var entities = await _repository.GetAll();
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public async Task<TDto?> GetById(Guid id)
    {
        var entity = await _repository.GetById(id);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<TDto?> Update(Guid id, TDto dto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
        {
            return null;
        }

        _mapper.Map(dto, entity);
        var newEntity = await _repository.Update(entity);

        return _mapper.Map<TDto>(newEntity);
    }
}
