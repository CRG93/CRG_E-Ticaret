using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETICARET.Entity
{
    public class Category
    {
        public int Id { get; set; }
        [DisplayName("Kategori Ad")]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}