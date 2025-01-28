﻿using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Dtos;
using ordermanagement.Mappers;
using ordermanagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly OrdermanagementdbContext _context;

        public AdminController(OrdermanagementdbContext context)
        {
            _context = context;
        }


        [HttpGet("menu")]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuitems = await _context.Menuitems.ToListAsync(); 
            var menuDto = menuitems.Select(s => s.ToViewMenuDto());

            if (menuitems == null)
            {
                return NotFound();
            }
            return Ok(menuDto);
        }


        [HttpGet("menu/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var menuitem = await _context.Menuitems.FindAsync(id);
            if (menuitem == null) { 
                return NotFound();
            }
            return Ok(menuitem.ToViewMenuDto());
        }

        [HttpPost("menu")]
       public async Task<IActionResult> Create([FromBody] CreateMenuRequestDto createMenuRequestDto)
        {

            var menuModel = createMenuRequestDto.ToCreateMenuDto();
            await _context.Menuitems.AddAsync(menuModel);
            await _context.SaveChangesAsync();
            return Ok(menuModel.ToViewMenuDto());
        }

        [HttpPut("menu/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMenuRequestDto updateDto)
        {
            var menuModel = await _context.Menuitems.FirstOrDefaultAsync(x => x.ItemId == id);

            if (menuModel == null)
            {
                return NotFound();
            }
            //menuModel.ItemId = updateDto.id;
            menuModel.Name = updateDto.Name;
            menuModel.Price = updateDto.Price;
            menuModel.Image = updateDto.Image;

            await _context.SaveChangesAsync();
            return Ok(menuModel.ToViewMenuDto());
            
        }

        [HttpDelete("menu/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
             var menuModel = await _context.Menuitems.FirstOrDefaultAsync(x => x.ItemId == id);
            if (menuModel == null)
            {
                return NotFound();
            }
             _context.Menuitems.Remove(menuModel);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
