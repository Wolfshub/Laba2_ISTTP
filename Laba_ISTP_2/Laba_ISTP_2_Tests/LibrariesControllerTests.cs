using System.Linq;
using System.Threading.Tasks;
using Laba_ISTP_2.Context;
using Laba_ISTP_2.Controllers;
using Laba_ISTP_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;



public class LibrariesControllerTests
{


    [Fact]
    public async Task GetLibraries_ReturnsSuccessMessage()
    {
        var contextMock = new Mock<ProjectDbContext>();
        var libraries = new List<Library>
            {
                new Library { Id = 1, Name = "Library 1", Location = "Location 1", Books = new List<Book> { new Book { Title = "Book 1" } } },
            };
        contextMock.Setup(c => c.Libraries.Include(It.IsAny<string>())).Returns(contextMock.Object.Libraries);
        contextMock.Object.Libraries.AddRange(libraries);
        var controller = new LibrariesController(contextMock.Object);

        var result = await controller.GetLibraries();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var librariesDtos = Assert.IsType<List<LibraryDto>>(okResult.Value);
        var message = "Success";
        Assert.Equal(message, controller.Response.Headers["Message"]);
    }
}
