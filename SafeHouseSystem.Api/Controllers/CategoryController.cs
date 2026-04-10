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
    public IActionResult Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            _service.Create(dto);

            return Created("", dto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _service.GetAll();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var category = _service.GetById(id);

        if (category is null)
            return NotFound(new { message = "Category not found" });

        return Ok(category);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _service.Delete(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound(new { message = "Category not found" });
        }
    }
}