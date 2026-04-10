using Microsoft.AspNetCore.Mvc;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;

namespace SafeHouseSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _service;

    public PersonController(IPersonService service)
    {
        _service = service;
    }


    [HttpPost]
    public IActionResult Create([FromBody] CreatePersonDto dto)
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
        var persons = _service.GetAll();
        return Ok(persons);
    }


    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var person = _service.GetById(id);

        if (person is null)
            return NotFound(new { message = "Person not found" });

        return Ok(person);
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
            return NotFound(new { message = "Person not found" });
        }
    }
}