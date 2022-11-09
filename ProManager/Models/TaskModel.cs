using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ProManager.Models
{
    [Index("TaskName",IsUnique =true,Name ="IDX_Tasks")]
    [Table("Tasks")]
    public sealed class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public TaskComment TaskComment { get; set; }
    }
}
