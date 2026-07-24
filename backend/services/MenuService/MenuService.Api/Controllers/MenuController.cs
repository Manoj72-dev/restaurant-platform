using MenuService.Application.DTOs.Requests;
using MenuService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService) => _menuService = menuService;

        //-------------------------------------------------------------------------------------------------------

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _menuService.GetCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] Guid? categoryId, [FromQuery] string?name)
        {
            if(categoryId is not null)
            {
                var byCategory = await _menuService.GetItemsByCategoryAsync(categoryId.Value);
                return Ok(byCategory);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var bySearch = await _menuService.SearchItemsByNameAsync(name);
                return Ok(bySearch);
            }
            return BadRequest(new { message = "Provide either categoryId or name to filter items." });
        }

        [HttpGet("items/{itemId}")]
        public async Task<IActionResult> GetItemById(Guid itemId)
        {
            var result = await _menuService.GetItemByIdAsync(itemId);
            return Ok(result);
        }

        //-------------------------------------------------------------------------------------------------------

        [Authorize(Roles = "Admin")]
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            var result = await _menuService.CreateCategoryAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequest request)
        {
            var result = await _menuService.UpdateCategoryAsync(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _menuService.DeleteCategoryAsync(id);
            return Ok(new { message = "Category deleted." });
        }

        //--------------------------------------------------------------------------------------------------------

        [Authorize(Roles = "Admin")]
        [HttpPost("items")]
        public async Task<IActionResult> CreateMenuItem(CreateMenuItemRequest request)
        {
            var result = await _menuService.CreateMenuItemAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateMenuItem(Guid id, UpdateMenuItemRequest request)
        {
            var result = await _menuService.UpdateMenuItemAsync(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteMenuItem(Guid id)
        {
            await _menuService.DeleteMenuItemAsync(id);
            return Ok(new { message = "Menu item deleted." });
        }

        //---------------------------------------------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost("items/{itemId}/variants")]
        public async Task<IActionResult> AddVariant(Guid itemId, VariantRequest request)
        {
            var result = await _menuService.AddVariantAsync(itemId, request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("variants/{variantId}")]
        public async Task<IActionResult> UpdateVariant(Guid variantId, VariantRequest request)
        {
            var result = await _menuService.UpdateVariantAsync(variantId, request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("variants/{variantId}")]
        public async Task<IActionResult> DeleteVariant(Guid variantId)
        {
            await _menuService.DeleteVariantAsync(variantId);
            return Ok(new { message = "Variant deleted." });
        }
    }
}
