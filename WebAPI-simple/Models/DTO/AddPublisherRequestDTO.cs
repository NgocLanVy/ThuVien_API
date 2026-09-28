using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_simple.Models.DTO
{
    public class AddPublisherRequestDTO
    {
        [Required(ErrorMessage = "Name không được để trống")]
        public string Name { get; set; }
    }
}