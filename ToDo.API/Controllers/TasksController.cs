using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDo.Application.Interfaces;
using ToDo.Service.Dtos;

namespace ToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskServiceRepository _taskService;

        public TasksController(ITaskServiceRepository taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        /// <param name="filter">Optional filter to search tasks by description.</param>
        /// <param name="isCompleted">Optional filter to search tasks by completion status.</param>
        /// <returns>A list of tasks.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieve a list of tasks with optional filters.")]
        [SwaggerResponse(200, "A list of tasks retrieved successfully.", typeof(IEnumerable<TaskDto>))]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetAll([FromQuery] string? filter, [FromQuery] bool? isCompleted)
        {
            var tasks = await _taskService.GetAllTasksAsync(filter, isCompleted);
            return Ok(tasks);
        }

        /// <summary>
        /// Retrieves a task by ID.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>The task with the specified ID.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a task by ID", Description = "Retrieve a task by its ID.")]
        [SwaggerResponse(200, "The task retrieved successfully.", typeof(TaskDto))]
        [SwaggerResponse(404, "Task not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="createTaskDto">The details of the task to create.</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a task", Description = "Create a new task.")]
        [SwaggerResponse(201, "The task was created successfully.", typeof(TaskDto))]
        [SwaggerResponse(400, "Invalid request.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve un error 400 con los detalles de validación
            }
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="updateTaskDto">The updated task details.</param>
        /// <returns>The updated task.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a task", Description = "Update an existing task by ID.")]
        [SwaggerResponse(200, "The task was updated successfully.", typeof(TaskDto))]
        [SwaggerResponse(404, "Task not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);
            return Ok(task);
        }

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a task", Description = "Delete a task by its ID.")]
        [SwaggerResponse(204, "The task was deleted successfully.")]
        [SwaggerResponse(404, "Task not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid task ID.");
            }
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Marks a task as completed or not completed.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <param name="isCompleted">The completion status of the task.</param>
        /// <returns>No content.</returns>
        [HttpPatch("{id}/complete")]
        [SwaggerOperation(Summary = "Mark task as completed", Description = "Update the completion status of a task.")]
        [SwaggerResponse(204, "The task status was updated successfully.")]
        [SwaggerResponse(404, "Task not found.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> MarkAsCompleted(int id, [FromBody] bool isCompleted)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid task ID.");
            }
            await _taskService.MarkAsCompletedAsync(id, isCompleted);
            return NoContent();
        }
    }
}
