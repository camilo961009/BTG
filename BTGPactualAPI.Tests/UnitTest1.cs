using Xunit;
using Moq;
using BTGPactualAPI.Controllers;
using BTGPactualAPI.Models;
using BTGPactualAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTGPactualAPI.Tests
{
    public class FondosControllerTests
    {
    [Fact]
        public async Task Post_ValidFondo_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockMongoDBService = new Mock<MongoDBService>();
            var mockEmailService = new Mock<EmailService>();

            var controller = new FondosController(mockMongoDBService.Object, mockEmailService.Object);

            var nuevoFondo = new Fondo
            {
                Id = "1",
                Nombre = "Fondo de prueba",
                MontoMinimo = 1000
            };

            // Act
            var result = await controller.Post(nuevoFondo);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedFondo = Assert.IsType<Fondo>(createdAtActionResult.Value);
            Assert.Equal(nuevoFondo.Id, returnedFondo.Id);
            Assert.Equal(nuevoFondo.Nombre, returnedFondo.Nombre);
            Assert.Equal(499000, returnedFondo.MontoInicial); // Verificar el monto inicial actualizado
        }
    }
}


