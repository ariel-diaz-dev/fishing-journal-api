using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Controllers;

namespace Tests.Controllers;

public class BaseControllerTests
{
    private class TestBaseController : BaseController
    {
        public Guid GetAccountId() => AccountId;
    }

    [Fact]
    public void AccountId_WhenSetInHttpContext_ReturnsCorrectValue()
    {
        // Arrange
        var controller = new TestBaseController();
        var accountId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext();
        httpContext.Items["AccountId"] = accountId;
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = controller.GetAccountId();

        // Assert
        Assert.Equal(accountId, result);
    }

    [Fact]
    public void AccountId_WhenNotSetInHttpContext_ThrowsInvalidOperationException()
    {
        // Arrange
        var controller = new TestBaseController();
        var httpContext = new DefaultHttpContext();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => controller.GetAccountId());
        Assert.Contains("AccountId not found in HttpContext", exception.Message);
        Assert.Contains("[RequireAccountJwt]", exception.Message);
    }

    [Fact]
    public void AccountId_WhenWrongTypeInHttpContext_ThrowsInvalidOperationException()
    {
        // Arrange
        var controller = new TestBaseController();
        var httpContext = new DefaultHttpContext();
        httpContext.Items["AccountId"] = "not-a-guid";
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => controller.GetAccountId());
        Assert.Contains("AccountId not found in HttpContext", exception.Message);
    }

    [Fact]
    public void AccountId_WhenHttpContextIsNull_ThrowsNullReferenceException()
    {
        // Arrange
        var controller = new TestBaseController();
        // Not setting ControllerContext, so HttpContext will be null

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => controller.GetAccountId());
    }
}