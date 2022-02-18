using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MultiShop_Myself.Models
{
    public class ViewModel
    {
        public Customer customer { get; set; }
        public OrderDetail oderDetail { get; set; }
        public Order order { get; set; }
        public Category category { get; set; }
        public Product product { get; set; }

        [DisplayFormat (DataFormatString = "{0:0.#00}")]
        public Double Thanhtien { get; set; }
    }
}