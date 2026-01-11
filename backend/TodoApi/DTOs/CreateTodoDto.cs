using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class CreateTodoDto
    {
        [Required(ErrorMessage = "Text is required.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Text can only contain letters, numbers, and spaces.")]
        public string Text { get; set; } = string.Empty;
    }
}