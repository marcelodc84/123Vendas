using Domain.Entities;
using Domain.Interfaces;

namespace Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly List<Sale> _sales = new();

        public Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return Task.FromResult(_sales.AsEnumerable());
        }

        public Task<Sale> GetSaleByIdAsync(int saleNumber)
        {
            var sale = _sales.FirstOrDefault(s => s.SaleNumber == saleNumber);
            return Task.FromResult(sale);
        }

        public Task AddSaleAsync(Sale sale)
        {
            _sales.Add(sale);
            return Task.CompletedTask;
        }

        public Task UpdateSaleAsync(Sale sale)
        {
            var existingSale = _sales.FirstOrDefault(s => s.SaleNumber == sale.SaleNumber);
            if (existingSale != null)
            {
                _sales.Remove(existingSale);
                _sales.Add(sale);
            }
            return Task.CompletedTask;
        }

        public Task DeleteSaleAsync(int saleNumber)
        {
            var sale = _sales.FirstOrDefault(s => s.SaleNumber == saleNumber);
            if (sale != null)
            {
                _sales.Remove(sale);
            }
            return Task.CompletedTask;
        }
    }
}
