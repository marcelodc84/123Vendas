using Bogus;
using Data;
using Data.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Tests.Repositories
{
    public class SaleRepositoryTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly SaleRepository _repository;
        private readonly Faker<Sale> _saleFaker;

        private readonly ILogger _logger;
        private readonly IEventPublisher _eventPublisher;

        public SaleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SaleRepository_Tests_Database")
                .Options;

            _context = new AppDbContext(options);

            _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            _eventPublisher = new ConsoleEventPublisher();

            _repository = new SaleRepository(_context, _logger, _eventPublisher);

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

        public async Task InitializeAsync()
        {
            // This method is called before each test method is executed.
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync()
        {
            // This method is called after each test method is executed.
            return Task.CompletedTask;
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

            await _context.Sales.AddRangeAsync(sales);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllSalesAsync();

            // Assert
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(sales);
        }

        [Fact]
        public async Task GetSaleByIdAsync_ShouldReturnSale()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetSaleByIdAsync(sale.SaleNumber);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sale);
        }

        [Fact]
        public async Task UpdateSaleAsync_ShouldUpdateSale()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

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
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteSaleAsync(sale.SaleNumber);
            var result = await _repository.GetSaleByIdAsync(sale.SaleNumber);

            // Assert
            result.Should().BeNull();
        }
    }
}
