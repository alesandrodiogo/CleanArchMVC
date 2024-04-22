using System.ComponentModel.DataAnnotations;

namespace CleanArchMVC.Application.DTOs;
public class ProductDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage ="The name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }
}
