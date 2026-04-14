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

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction is null)
            throw new ArgumentException("Transaction not found");

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}