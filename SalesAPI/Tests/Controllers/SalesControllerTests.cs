using API.Controllers;
using Bogus;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Controllers
{
    public class SalesControllerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly SalesController _controller;
        private readonly Faker<Sale> _saleFaker;

        public SalesControllerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _controller = new SalesController(_saleRepository);
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
        public async Task GetAllSales_ShouldReturnOkResultWithSales()
        {
            // Arrange
            var sales = _saleFaker.Generate(5);
            _saleRepository.GetAllSalesAsync().Returns(Task.FromResult((IEnumerable<Sale>)sales));

            // Act
            var result = await _controller.GetAllSales();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(sales);
        }

        [Fact]
        public async Task GetSaleById_ShouldReturnOkResultWithSale()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            _saleRepository.GetSaleByIdAsync(sale.SaleNumber).Returns(Task.FromResult(sale));

            // Act
            var result = await _controller.GetSaleById(sale.SaleNumber);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(sale);
        }

        [Fact]
        public async Task GetSaleById_ShouldReturnNotFoundResult()
        {
            // Arrange
            _saleRepository.GetSaleByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<Sale>(null));

            // Act
            var result = await _controller.GetSaleById(1);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateSale_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var sale = _saleFaker.Generate();

            // Act
            var result = await _controller.CreateSale(sale);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201);
            createdAtActionResult.Value.Should().BeEquivalentTo(sale);
        }

        [Fact]
        public async Task UpdateSale_ShouldReturnNoContentResult()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            _saleRepository.GetSaleByIdAsync(sale.SaleNumber).Returns(Task.FromResult(sale));

            // Act
            var result = await _controller.UpdateSale(sale.SaleNumber, sale);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UpdateSale_ShouldReturnBadRequestResult()
        {
            // Arrange
            var sale = _saleFaker.Generate();

            // Act
            var result = await _controller.UpdateSale(sale.SaleNumber + 1, sale);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task DeleteSale_ShouldReturnNoContentResult()
        {
            // Arrange
            var sale = _saleFaker.Generate();
            _saleRepository.GetSaleByIdAsync(sale.SaleNumber).Returns(Task.FromResult(sale));

            // Act
            var result = await _controller.DeleteSale(sale.SaleNumber);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }
    }
}
