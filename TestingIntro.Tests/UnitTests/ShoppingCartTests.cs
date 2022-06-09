using Moq;
using TestingIntro.BLL;
using Xunit;

namespace TestingIntro.Tests.UnitTests;

public class ShoppingCartTests
{
    public class DbServiceStub : IDbService
    {
        public bool ProcessResult { get; set; }
        public Product? ProductBeingProcessed { get; set; }
        public int ProductIdBeingProcessed { get; set; }

        public bool RemoveShoppingCartItem(int? id)
        {
            if (id != null) 
            {
                ProductIdBeingProcessed = Convert.ToInt32(id);
            }
                
            return ProcessResult;
        }

        public bool SaveShoppingCartItem(Product? prod)
        {
            ProductBeingProcessed = prod;
            return ProcessResult;
        }
    }

    public readonly Mock<IDbService> _dbServiceMock = new();
    public readonly DbServiceStub _dbStub = new() { ProcessResult = true };

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Add_Product_Success(bool useMoq)
    {
        // Arrange
        var shoppingCart = new ShoppingCart(useMoq ? _dbServiceMock.Object : _dbStub);
        _dbStub.ProcessResult = true;

        // Act / When
        var product = new Product(1, "Shoes", 150);
        var result = shoppingCart.AddProduct(product);

        // Assert / Then
        Assert.True(result);
        if (useMoq)
        {
            _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Once);
        }
        else
        {
            Assert.Equal(product, _dbStub.ProductBeingProcessed);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Add_Product_Failure_InvalidPayload(bool useMoq)
    {
        // Arrange
        var shoppingCart = new ShoppingCart(useMoq ? _dbServiceMock.Object : _dbStub);
        _dbStub.ProcessResult = false;

        // Act
        var result = shoppingCart.AddProduct(null);

        // Assert
        Assert.False(result);
        if (useMoq)
        {
            _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Never);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Remove_Product_Success(bool useMoq)
    {
        // Arrange
        var shoppingCart = new ShoppingCart(useMoq ? _dbServiceMock.Object : _dbStub);
        _dbStub.ProcessResult = true;

        // Act
        var product = new Product(1, "Shoes", 150);
        shoppingCart.AddProduct(product);
        var deleteResult = shoppingCart.DeleteProduct(product.Id);

        // Assert
        Assert.True(deleteResult);
        if (useMoq)
        {
            _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Once);
        }
        else
        {
            Assert.Equal(product.Id, _dbStub.ProductIdBeingProcessed);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Remove_Product_Failed(bool useMoq)
    {
        // Arrange
        var shoppingCart = new ShoppingCart(useMoq ? _dbServiceMock.Object : _dbStub);
        _dbStub.ProcessResult = false;

        // Act
        var result = shoppingCart.DeleteProduct(null);

        // Assert
        Assert.False(result);
        if (useMoq)
        {
            _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Never);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Remove_Product_Failed_InvalidId(bool useMoq)
    {
        // Arrange
        var shoppingCart = new ShoppingCart(useMoq ? _dbServiceMock.Object : _dbStub);
        _dbStub.ProcessResult = false;

        // Act
        var result = shoppingCart.DeleteProduct(0);

        // Assert
        Assert.False(result);
        if (useMoq)
        {
            _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Never);
        }
    }
}