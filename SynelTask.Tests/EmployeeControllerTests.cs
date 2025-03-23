using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SynelTask.Controllers;
using System.Text;

namespace SynelTask.Tests;

public class EmployeeControllerTests
{
    [Fact]
    public async Task ImportFile_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var context = new ApplicationDbContext(options);

        var controller = new EmployeeController(context);

        var fileContent = "PayrollNumber,Forenames,Surname\nCOOP08,John,Doe";
        var fileName = "employees.csv";
        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        // Act
        var result = await controller.Import(fileMock.Object);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(1, await context.Employees.CountAsync());
    }

    [Fact]
    public async Task ImportFile_EmptyFile_ReturnsError()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDbEmpty")
            .Options;
        var context = new ApplicationDbContext(options);

        var controller = new EmployeeController(context);

        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(""));
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns("empty.csv");
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        // Act
        var result = await controller.Import(fileMock.Object);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("The file contains no valid employee data.", controller.TempData["Error"]);
    }
}
