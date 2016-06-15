namespace Trello.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attachment")]
    public class Attachment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string FileName { get; set; }

        [Required]
        public byte[] FileContent { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CardId { get; set; }

        public virtual Card Card { get; set; }
    }
}
