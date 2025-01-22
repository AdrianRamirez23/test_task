

using ToDo.Infraestructure.Models;
using ToDo.Service.Dtos;

namespace ToDo.Application.Interfaces
{
    public interface ITaskServiceRepository
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync(string? filter = null, bool? isCompleted = null);
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task DeleteTaskAsync(int id);
        Task MarkAsCompletedAsync(int id, bool isCompleted);
    }
}
