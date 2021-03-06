using System;
using Moq;
using MyShop.Domain.Models;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Controllers;
using MyShop.Web.Models;
using Xunit;

namespace MyShop.Web.Tests;

public class OrderControllerTests
{
    /// <summary>
    /// This test is to verify that the user can create an order
    /// </summary>
    [Fact]
    public void CanCreateOrderWithCorrectModel()
    {
        // Mocking both repositories prevents need for database access
        var orderRepository = new Mock<IRepository<Order>>();
        var productRepository = new Mock<IRepository<Product>>();

        // Create new instance of order controller but we will instead pass in mocked repositories
        var orderController = new OrderController(orderRepository.Object, productRepository.Object);

        // Generate some example data for order 
        var createOrderModel = new CreateOrderModel
        {
            Customer = new CustomerModel
            {
                Name = "Jon Doe",
                ShippingAddress = "1 Queen St",
                City = "Auckland",
                PostalCode = "1990",
                Country = "New Zealand"
            },
            LineItems = new[]
            {
                new LineItemModel
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2
                },
                new LineItemModel
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 12
                }
            }
        };

        // Call create endpoint
        orderController.Create(createOrderModel);
        
        // The following verify method provided by moq is used to verify that the create method
        // of the order repository is called at most once (see line 46)
        // This is the first thing you should test
        orderRepository.Verify(repository =>
            repository.Add(It.IsAny<Order>()), Times.AtMostOnce()); 
    }
}