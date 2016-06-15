namespace Trello.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaskItem")]
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int CardId { get; set; }

        public virtual Card Card { get; set; }
    }
}
