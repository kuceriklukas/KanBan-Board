namespace Trello.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class CardLabel
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int LabelId { get; set; }

        public virtual Card Card { get; set; }

        public virtual Label Label { get; set; }
    }
}
