namespace Trello.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class CommentReply
    {
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string Text { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int CommentId { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
