using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace ProManager.ViewModels
{
    public sealed class TaskCommentDto
    {
        [Required(ErrorMessage ="Parent task isn`t binded to new comment!")]
        public string TaskName { get; set; }
        public IFormFile File { get; set; }
        public string Text { get; set; }
        public bool IsFile { get => File != null; }
    }
}
