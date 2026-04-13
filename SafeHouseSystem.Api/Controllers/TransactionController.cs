using Microsoft.AspNetCore.Mvc;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;

namespace SafeHouseSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionController(ITransactionService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTransactionDto dto)
    {
        _service.Create(dto);
        return Created("", dto);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var transactions = _service.GetAll();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var transaction = _service.GetById(id);
        if (transaction is null)
            return NotFound(new { message = "Transaction not found" });
        return Ok(transaction);
    }

    [HttpGet("totals-by-category")]
    public IActionResult GetTotalsByCategory()
    {
        var totals = _service.GetTotalsByCategory();
        return Ok(totals);
    }

    [HttpGet("totals-by-category/{categoryId}")]
    public IActionResult GetTotalsByCategoryId(Guid categoryId)
    {
        var totals = _service.GetTotalsByCategoryId(categoryId);
        return Ok(totals);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _service.Delete(id);
        return NoContent();
    }
}