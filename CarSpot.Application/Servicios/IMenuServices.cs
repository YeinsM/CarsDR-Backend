using CarSpot.Domain.Entities;

public interface IMenuService
{
    Task<Menu?> GetByIdAsync(int id);
    Task CreateAsync(Menu menu);
    Task Update(Menu menu);
    Task<bool> DeleteAsync(int id);
}