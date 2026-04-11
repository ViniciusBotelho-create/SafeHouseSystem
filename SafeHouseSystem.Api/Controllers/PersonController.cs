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

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdatePersonDto dto)
    {
        try
        {
            _service.Update(id, dto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
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


    [HttpGet("summary")]
    public IActionResult GetSummary()
    {
        var summary = _service.GetSummary();
        return Ok(summary);
    }
}