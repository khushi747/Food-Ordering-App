using ordermanagement.Dtos.Admin;
using ordermanagement.Models;

namespace ordermanagement.Mappers
{
    public static class MenuMapper
    {
        public static ViewMenuDto ToViewMenuDto(this Menuitem menuitemModel)
        {
            return new ViewMenuDto
            {
                ItemId = menuitemModel.ItemId,
                Name = menuitemModel.Name,
                Price = menuitemModel.Price,    
                Quantity = menuitemModel.Quantity,
                Image = menuitemModel.Image,
            };
        }

        public static Menuitem ToCreateMenuDto(this CreateMenuRequestDto ViewMenuDto)
        {
            return new Menuitem
            {
                
                Name = ViewMenuDto.Name,
                Price = ViewMenuDto.Price,
                Quantity = ViewMenuDto.Quantity,
                Image = ViewMenuDto.Image,
            };
        }
    }
}
