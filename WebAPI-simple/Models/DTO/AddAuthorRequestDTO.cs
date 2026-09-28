using System.ComponentModel.DataAnnotations;

namespace WebAPI_simple.Models.DTO
{
    public class AddAuthorRequestDTO
    {
        [Required(ErrorMessage = "FullName không được để trống")]
        [MinLength(3, ErrorMessage = "FullName tối thiểu 3 ký tự")]
        public string FullName { get; set; }
    }
}