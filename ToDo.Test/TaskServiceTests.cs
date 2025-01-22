using Microsoft.EntityFrameworkCore;
using ToDo.Infraestructure.Data;
using ToDo.Infraestructure.Models;
using ToDo.Service.Dtos;
using ToDo.Service.Repository;

namespace ToDo.Test
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _dbContext;
        private readonly TaskServiceRepository _taskService;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            // Crear una instancia real de AppDbContext
            _dbContext = new AppDbContext(options);
            _taskService = new TaskServiceRepository(_dbContext);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnCreatedTask()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { Description = "Test Task" };

            // Act
            var result = await _taskService.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Description);
            Assert.False(result.IsCompleted);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnTasks()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.Tasks.Add(new TaskToDo { Id = 1, Description = "Task 1", IsCompleted = false, CreatedAt = DateTime.UtcNow });
            _dbContext.Tasks.Add(new TaskToDo { Id = 2, Description = "Task 2", IsCompleted = true, CreatedAt = DateTime.UtcNow });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}