using Bogus;
using Data.Repositories;
using Domain.Entities;
using FluentAssertions;

namespace Tests.Repositories
{
    public class SaleRepositoryTests
    {
        private readonly SaleRepository _repository;
        private readonly Faker<Sale> _saleFaker;

        public SaleRepositoryTests()
        {
            _repository = new SaleRepository();
            _saleFaker = new Faker<Sale>()
                .RuleFor(s => s.SaleNumber, f => f.Random.Int(1, 1000))
                .RuleFor(s => s.SaleDate, f => f.Date.Past())
                .RuleFor(s => s.Customer, f => f.Person.FullName)
                .RuleFor(s => s.TotalAmount, f => f.Finance.Amount())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f => new Faker<SaleItem>()
                    .RuleFor(i => i.Product, f => f.Commerce.ProductName())
                    .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                    .RuleFor(i => i.UnitPrice, f => f.Finance.Amount())
                    .RuleFor(i => i.Discount, f => f.Finance.Amount(0, 10))
                    .RuleFor(i => i.TotalItemAmount, (f, i) => i.Quantity * i.UnitPrice - i.Discount)
                    .Generate(3).ToList())
                .RuleFor(s => s.IsCancelled, f => f.Random.Bool());
        }

        [Fact]
        public async Task AddSaleAsync_ShouldAddSale()
        {
            // Arrange
            var sale = _saleFaker.Generate();

            // Act
            await _repository.AddSaleAsync(sale);
            var result = await _repository.GetSaleByIdAsync(sale.SaleNumber);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sale);
        }

        [Fact]
        public async Task GetAllSalesAsync_ShouldReturnAllSales()
        {
            // Arrange
            var sales = _saleFaker.Generate(5);
            foreach (var sale in sales)
            {
                await _repository.AddSaleAsync(sale);
            }

            // Act
            var result = await _repository.GetAllSalesAsync();

            // Assert
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(sales);
        }

        [Fact]
        public async Task UpdateSaleAsync_ShouldUpdateSale()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            await _repository.AddSaleAsync(sale);
            sale.Customer = "Updated Customer";

            // Act
            await _repository.UpdateSaleAsync(sale);
            var result = await _repository.GetSaleByIdAsync(sale.SaleNumber);

            // Assert
            result.Should().NotBeNull();
            result.Customer.Should().Be("Updated Customer");
        }

        [Fact]
        public async Task DeleteSaleAsync_ShouldRemoveSale()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            await _repository.AddSaleAsync(sale);

            // Act
            await _repository.DeleteSaleAsync(sale.SaleNumber);
            var result = await _repository.GetSaleByIdAsync(sale.SaleNumber);

            // Assert
            result.Should().BeNull();
        }
    }
}
