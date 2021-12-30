namespace Day3_Lab.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class news
    {
        [Key]
        public int news_id { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(150)]
        public string brief { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        [StringLength(50)]
        public string photo { get; set; }

        public DateTime? datetime { get; set; }

        public int? user_id { get; set; }

        public int? cat_id { get; set; }

        public virtual catalog catalog { get; set; }

        public virtual user_data user_data { get; set; }
    }
}
