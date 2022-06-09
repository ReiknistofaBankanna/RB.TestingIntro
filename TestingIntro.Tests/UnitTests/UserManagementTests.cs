using System.Linq;
using TestingApp.Functionality;
using Xunit;

namespace TestingIntro.Tests.UnitTests;

public class UserManagementTests
{

    [Fact]
    public void Add_CreateUser()
    {
        // Arrange 
        var userManagement = new UserManagement();

        // Act
        userManagement.Add(new("Mohamad", "Lawand"));

        // Assert
        var savedUser = Assert.Single(userManagement.AllUsers);
        Assert.NotNull(savedUser); // checking that the usermanagement list is not empty
        Assert.Equal("Mohamad", savedUser.firstName);
        Assert.Equal("Lawand", savedUser.LastName);
        Assert.False(savedUser.VerifiedEmail);
    }

    [Fact]
    public void Update_UpdateMobileNumber()
    {
        // Arrange
        var userManagement = new UserManagement();

        // Act
        userManagement.Add(new("Mohamad", "Lawand"));

        var firstUser = userManagement.AllUsers.First();
        firstUser.Phone = "+44000000032";

        userManagement.UpdatePhone(firstUser);

        // Assert
        var savedUser = Assert.Single(userManagement.AllUsers);
        Assert.NotNull(savedUser);
        Assert.Equal("+44000000032", savedUser.Phone);
    }
}