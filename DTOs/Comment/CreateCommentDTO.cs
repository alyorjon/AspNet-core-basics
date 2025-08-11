using System;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class CreateCommentDTO
{
    [Required]
    [StringLength(100)]
    public string Content { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public int StockId { get; set; }
    
}
