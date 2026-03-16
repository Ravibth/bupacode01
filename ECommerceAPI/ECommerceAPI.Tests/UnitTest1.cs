using ECommerceAPI.Services;
using ECommerceAPI.Models;
using ECommerceAPI.Data;
using ECommerceAPI.Repositories;
using Xunit;

namespace ECommerceAPI.Tests;

public class UnitTest1
{
    [Fact]
    public void CouponValidation_FLAT50_Valid()
    {
        // Arrange
        var cartRepository = new InMemoryCartRepository();
        var couponRepository = new InMemoryCouponRepository();
        var service = new CouponService(cartRepository, couponRepository);
        var cartId = "test";
        FakeStore.Carts[cartId] = new Cart
        {
            Id = cartId,
            Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 1, Price = 600 } }
        };

        // Act
        var cart = service.ApplyCoupon(cartId, "FLAT50");

        // Assert
        Assert.Equal(50, cart.Discount);
        Assert.Equal(550, cart.TotalAmount);
    }

    [Fact]
    public void CouponValidation_FLAT50_Invalid_Subtotal()
    {
        // Arrange
        var cartRepository = new InMemoryCartRepository();
        var couponRepository = new InMemoryCouponRepository();
        var service = new CouponService(cartRepository, couponRepository);
        var cartId = "test2";
        FakeStore.Carts[cartId] = new Cart
        {
            Id = cartId,
            Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 1, Price = 400 } }
        };

        // Assert
        Assert.Throws<ArgumentException>(() => service.ApplyCoupon(cartId, "FLAT50"));
    }

    [Fact]
    public void CheckoutPricing_CalculatesCorrectly()
    {
        // Arrange
        var cartRepository = new InMemoryCartRepository();
        var productRepository = new InMemoryProductRepository();
        var orderRepository = new InMemoryOrderRepository();
        var service = new CheckoutService(cartRepository, productRepository, orderRepository);
        var cartId = "test3";
        FakeStore.Carts[cartId] = new Cart
        {
            Id = cartId,
            Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, Price = 100 } },
            Discount = 10
        };

        // Act
        var order = service.Checkout(cartId);

        // Assert
        Assert.Equal(200, order.Subtotal);
        Assert.Equal(10, order.Discount);
        Assert.Equal(190, order.GrandTotal);
    }

    [Fact]
    public void StockError_InsufficientStock()
    {
        // Arrange
        var cartRepository = new InMemoryCartRepository();
        var productRepository = new InMemoryProductRepository();
        var orderRepository = new InMemoryOrderRepository();
        var service = new CheckoutService(cartRepository, productRepository, orderRepository);
        var cartId = "test4";
        FakeStore.Carts[cartId] = new Cart
        {
            Id = cartId,
            Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 100, Price = 100 } }
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.Checkout(cartId));
    }
}
