using CarSpot.Domain.Entities;

public interface IMenuService
{
    Task<Menu?> GetByIdAsync(Guid id);
    Task CreateAsync(Menu menu);
    Task Update(Menu menu);
    Task<bool> DeleteAsync(Guid id);
}
