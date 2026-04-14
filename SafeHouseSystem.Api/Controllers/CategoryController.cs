using Microsoft.AspNetCore.Mvc;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;

namespace SafeHouseSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        await _service.CreateAsync(dto);
        return Created("", dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category is null)
            return NotFound(new { message = "Category not found" });
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}