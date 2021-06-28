using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AllegroAnalitics.Api.BindingModels
{
    public class Parametr
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
