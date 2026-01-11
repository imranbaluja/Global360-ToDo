using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class UpdateTodoDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Text is required.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Text can only contain letters, numbers, and spaces.")]
        public string Text { get; set; } = string.Empty;
    }
}
