using CarSpot.Domain.Entities;
using CarSpot.Application.DTOs;

namespace CarSpot.Application.Services
{
    public class MenuTreeBuilder
    {
        public List<MenuResponse> Build(List<Menu> allMenus)
        {
            var menuResponses = allMenus
                .Select(menu => new MenuResponse(
                    menu.Id,
                    menu.Label,
                    menu.Icon,
                    menu.To,
                    menu.ParentId,
                    new List<MenuResponse>()
                ))
                .ToList();

            var menuDict = menuResponses.ToDictionary(m => m.Id);

            var rootMenus = new List<MenuResponse>();

            foreach (var menu in menuResponses)
            {
                if (menu.ParentId is null)
                {
                    rootMenus.Add(menu);
                }
                else if (menuDict.TryGetValue(menu.ParentId.Value, out var parent))
                {
                    parent.Children.Add(menu);
                }
            }

            return rootMenus;
        }
    }
}
