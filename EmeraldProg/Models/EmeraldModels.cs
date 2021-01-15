using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmeraldProg.Models
{
    public class Location
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationID { get; set; }

        [Required]
        [StringLength(200)]
        public string LocationName { get; set; }
    }

    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; }
    }

    public class ItemType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string ItemTypeName { get; set; }
    }

    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Required]
        [Column(TypeName = "nchar(3)")]
        public string SerialNo { get; set; }
        public int CategoryID { get; set; }
        public int LocationID { get; set; }
        public int ItemTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string ItemName { get; set; }

        [StringLength(200)]
        public string Vendor { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public double ItemPrice { get; set; }

        [DataType(DataType.Currency)]
        public double InstalPrice { get; set; }

        public int Qty { get; set; }

        [DataType(DataType.Date)]
        public DateTime PurchasedDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? InstalDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RunDate { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }

        [ForeignKey("ItemTypeID")]
        public virtual ItemType ItemType { get; set; }
    }
}
