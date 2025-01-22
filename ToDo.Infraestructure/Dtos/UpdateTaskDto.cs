using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Service.Dtos
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "IsCompleted is required.")]
        public bool IsCompleted { get; set; }
    }
}
