
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AllegroAnalitics.Api.BindingModels
{
    public class GetAvarage
    {
        [Display(Name = "OrderId")]
        [Required]
        public uint OrderID { get; set; }
        
        [Display(Name = "startDate")]
        public DateTime StartDate { get; set; }
    }
}
