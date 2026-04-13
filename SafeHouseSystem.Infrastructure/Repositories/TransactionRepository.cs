using Microsoft.EntityFrameworkCore;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Infrastructure.Data;

namespace SafeHouseSystem.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        _context.SaveChanges();
    }

    public Transaction? GetById(Guid id)
    {
        return _context.Transactions
            .Include(t => t.Category)
            .FirstOrDefault(t => t.Id == id);
    }


    public IEnumerable<Transaction> GetAll()
    {
        return _context.Transactions
            .Include(t => t.Category)
            .ToList();
    }

    public IEnumerable<(Guid CategoryId, string CategoryDescription, decimal Total)> GetTotalsByCategory()
    {
        return _context.Transactions
            .Include(t => t.Category)
            .AsEnumerable()  
            .GroupBy(t => new { t.CategoryId, t.Category.Description })
            .Select(g => (
                g.Key.CategoryId,
                g.Key.Description,
                g.Sum(t => t.Amount)
            ));
    }

    public IEnumerable<(Guid CategoryId, string CategoryDescription, decimal Total)> GetTotalsByCategoryId(Guid categoryId)
    {
        return _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.CategoryId == categoryId)
            .AsEnumerable()
            .GroupBy(t => new { t.CategoryId, t.Category.Description })
            .Select(g => (
                g.Key.CategoryId,
                g.Key.Description,
                g.Sum(t => t.Amount)
            ));
    }

    public void Delete(Guid id)
    {
        var transaction = _context.Transactions.Find(id);

        if (transaction is null)
            throw new ArgumentException("Transaction not found");

        _context.Transactions.Remove(transaction);
        _context.SaveChanges();
    }
}