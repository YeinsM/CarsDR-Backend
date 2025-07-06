using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<Menu?> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id);

        public async Task CreateAsync(Menu menu)
        {
            await _repository.AddAsync(menu);

        }

        public async Task Update(Menu menu)
        {
            await _repository.Update(menu);

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var menu = await _repository.GetByIdAsync(id);

            if (menu == null) return false;

            await _repository.DeleteAsync(id);

            return true;
        }





    }
}
