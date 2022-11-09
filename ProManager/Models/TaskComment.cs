using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ProManager.Models
{
    [Index("CommentType",IsUnique =false,Name ="IDX_Comments")]
    [Table("TaskComments")]
    public sealed class TaskComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public byte CommentType { get; set; }
        [Required]
        public byte[] Content { get; set; }
        public Guid TaskId { get; set; }
        public TaskModel Task { get; set; }
    }
}
