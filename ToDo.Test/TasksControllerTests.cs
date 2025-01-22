using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Api.Controllers;
using ToDo.API.Controllers;
using ToDo.Application.Interfaces;
using ToDo.Service.Dtos;

namespace ToDo.Test
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskServiceRepository> _taskServiceMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _taskServiceMock = new Mock<ITaskServiceRepository>();
            _controller = new TasksController(_taskServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithTasks()
        {
            // Arrange
            var tasks = new List<TaskDto>
            {
                new TaskDto { Id = 1, Description = "Task 1", IsCompleted = false },
                new TaskDto { Id = 2, Description = "Task 2", IsCompleted = true }
            };

            _taskServiceMock.Setup(service => service.GetAllTasksAsync(null, null))
                            .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAll(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkWithTask()
        {
            // Arrange
            var task = new TaskDto { Id = 1, Description = "Task 1", IsCompleted = false };
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(1, returnedTask.Id);
            Assert.Equal("Task 1", returnedTask.Description);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { Description = "Task 1" };
            var createdTask = new TaskDto { Id = 1, Description = "Task 1", IsCompleted = false };

            _taskServiceMock.Setup(service => service.CreateTaskAsync(createTaskDto))
                            .ReturnsAsync(createdTask);

            // Act
            var result = await _controller.Create(createTaskDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedTask = Assert.IsType<TaskDto>(createdResult.Value);
            Assert.Equal(1, returnedTask.Id);
            Assert.Equal("Task 1", returnedTask.Description);
        }
    }
}
