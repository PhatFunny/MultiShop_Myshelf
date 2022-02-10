using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MultiShop_Myself.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name="Role Name")]
        public string Name { get; set; }
    }
}