using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ProManager.Models
{
    [Index("ProjectName",IsUnique =true,Name ="IDX_Projects")]
    [Table("Projects")]
    public sealed class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string ProjectName { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public ICollection<TaskModel> Tasks { get; set; }
        public Project()
        {
            Tasks = new List<TaskModel>(2);
        }
    }
}
