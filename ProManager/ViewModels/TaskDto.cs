using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace ProManager.ViewModels
{ 
    public sealed class TaskDto
    {
        //IN EDIT SENDING BY HIDDEN INPUT!
        [Required(ErrorMessage ="This field is required!")]
        [StringLength(255,MinimumLength =1,ErrorMessage ="Name length should be: > 0 and < 256!")]
        public string TargetName { get; set; }
        public bool IsProject { get; set; }
        public string ProjectName { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
