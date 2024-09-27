using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public SaleRepository(AppDbContext context, ILogger logger, IEventPublisher eventPublisher)
        {
            _context = context;
            _logger = logger;
            _eventPublisher = eventPublisher;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales.Include(s => s.Items).ToListAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            return await _context.Sales.Include(s => s.Items)
                                       .FirstOrDefaultAsync(s => s.SaleNumber == id);
        }

        public async Task AddSaleAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            _logger.Information("Sale added: {SaleNumber}", sale.SaleNumber);

            //event notification
            SaleCreated scEvent = new SaleCreated { SaleNumber = sale.SaleNumber, Customer = sale.Customer };
            await _eventPublisher.PublishAsync(scEvent);
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();

            _logger.Information("Sale updated: {SaleNumber}", sale.SaleNumber);

            //event notification
            SaleUpdated suEvent = new SaleUpdated { SaleNumber = sale.SaleNumber, Customer = sale.Customer };
            await _eventPublisher.PublishAsync(suEvent);
        }

        public async Task DeleteSaleAsync(int saleNumber)
        {
            var sale = await _context.Sales.FindAsync(saleNumber);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();

                _logger.Information("Sale deleted: {SaleNumber}", saleNumber);

                //event notification
                SaleCancelled scEvent = new SaleCancelled { SaleNumber = sale.SaleNumber };
                await _eventPublisher.PublishAsync(scEvent);
            }
        }
    }
}
