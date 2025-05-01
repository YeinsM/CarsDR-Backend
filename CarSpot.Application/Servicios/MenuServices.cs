using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;

namespace CarSpot.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<Menu?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task CreateAsync(Menu menu)
        {
            await _repository.AddAsync(menu);

        }

        public async Task Update(Menu menu)
        {
            _repository.Update(menu);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menu = await _repository.GetByIdAsync(id);

            if (menu == null) return false;

            await _repository.DeleteAsync(id);

            return true;
        }





    }
}