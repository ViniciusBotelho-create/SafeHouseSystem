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

    public void Delete(Guid id)
    {
        var transaction = _context.Transactions.Find(id);

        if (transaction is null)
            throw new ArgumentException("Transaction not found");

        _context.Transactions.Remove(transaction);
        _context.SaveChanges();
    }
}