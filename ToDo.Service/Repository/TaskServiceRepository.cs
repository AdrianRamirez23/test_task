using Microsoft.EntityFrameworkCore;
using ToDo.Application.Interfaces;
using ToDo.Infraestructure.Data;
using ToDo.Infraestructure.Models;
using ToDo.Service.Dtos;

namespace ToDo.Service.Repository
{
    public class TaskServiceRepository: ITaskServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public TaskServiceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync(string? filter = null, bool? isCompleted = null)
        {
            try
            {
                var query = _dbContext.Tasks.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                    query = query.Where(t => t.Description.Contains(filter));

                if (isCompleted.HasValue)
                    query = query.Where(t => t.IsCompleted == isCompleted.Value);

                return await query.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                }).ToListAsync();
            }
            catch (DbUpdateException ex)
            {

                throw new Exception("An error occurred while accessing the database. Please try again later.", ex);
            }
            catch (Exception ex)
            {

                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
            
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            try
            {
                var task = await _dbContext.Tasks.FindAsync(id);

                if (task == null)
                    throw new KeyNotFoundException("Task not found");

                return new TaskDto
                {
                    Id = task.Id,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt
                };
            }
            catch (DbUpdateException ex)
            {
              
                throw new Exception("An error occurred while accessing the database. Please try again later.", ex);
            }
            catch (Exception ex)
            {
               
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
           
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            try
            {
                var task = new TaskToDo
                {
                    Description = createTaskDto.Description,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Tasks.Add(task);
                await _dbContext.SaveChangesAsync();

                return new TaskDto
                {
                    Id = task.Id,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt
                };
            }
            catch (DbUpdateException ex)
            {
                
                throw new Exception("An error occurred while saving the task to the database.", ex);
            }
            catch (Exception ex)
            {
             
                throw new Exception("An unexpected error occurred while creating the task.", ex);
            }
           
        }

        public async Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
        {
            try
            {
                var task = await _dbContext.Tasks.FindAsync(id);

                if (task == null)
                    throw new KeyNotFoundException("Task not found");

                task.Description = updateTaskDto.Description;
                task.IsCompleted = updateTaskDto.IsCompleted;

                await _dbContext.SaveChangesAsync();

                return new TaskDto
                {
                    Id = task.Id,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt
                };
            }
            catch (DbUpdateException ex)
            {
               
                throw new Exception("An error occurred while updating the task to the database.", ex);
            }
            catch (Exception ex)
            {
               
                throw new Exception("An unexpected error occurred while updating the task.", ex);
            }
            
        }

        public async Task DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _dbContext.Tasks.FindAsync(id);

                if (task == null)
                    throw new KeyNotFoundException("Task not found");

                _dbContext.Tasks.Remove(task);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
              
                throw new Exception("An error occurred while deleting the task to the database.", ex);
            }
            catch (Exception ex)
            {
               
                throw new Exception("An unexpected error occurred while deleting the task.", ex);
            }
          
        }

        public async Task MarkAsCompletedAsync(int id, bool isCompleted)
        {
            try
            {
                var task = await _dbContext.Tasks.FindAsync(id);

                if (task == null)
                    throw new KeyNotFoundException("Task not found");

                task.IsCompleted = isCompleted;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
               
                throw new Exception("An error occurred while marking as complete the task to the database.", ex);
            }
            catch (Exception ex)
            {
                
                throw new Exception("An unexpected error occurred while marking as complete the task.", ex);
            }
           
        }
      
    }
}

