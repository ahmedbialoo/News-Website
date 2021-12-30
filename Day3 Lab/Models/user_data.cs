namespace Day3_Lab.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class user_data
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user_data()
        {
            news = new HashSet<news>();
        }

        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string user_name { get; set; }
        [Required]
        public int password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("password",ErrorMessage ="password not matched!")]
        [NotMapped]
        public int confirm_password { get; set; }

        public int? age { get; set; }

        [StringLength(50)]
        public string adress { get; set; }

        [StringLength(100)]
        [Remote("check","Home",ErrorMessage ="email already exist!")]
        public string email { get; set; }

        [StringLength(50)]
        public string photo { get; set; }

        [StringLength(50)]
        public string cv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<news> news { get; set; }

        public virtual skill skill { get; set; }
    }
}
