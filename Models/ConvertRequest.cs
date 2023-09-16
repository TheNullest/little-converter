using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace LittleConverter.Models
{
    public class ConvertRequest
    {
	   [MaxLength( 20, ErrorMessage = "The property '{0}' should have '{1}' maximum characters" )]
	   public string? NewFileName { get; set; }

	   [Required]
	   public ConvertToType ConvertToType { get; set; }

	   [Required]
	   public string ConvertedFilePath { get; set; }

	   [Required]
	   public IFormFile BaseFile { get; set; }

	   [Required]
	   public bool UploadBaseFile { get; set; } = true;

	   [Required]
	   public bool SaveConvertedFile { get; set; } = true;

    }
}
