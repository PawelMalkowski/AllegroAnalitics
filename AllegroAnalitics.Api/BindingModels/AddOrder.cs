using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AllegroAnalitics.Api.BindingModels
{
    public class AddOrder
    {
        [Required]
        [MinLength(2)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "CattegoryId")]
        public List<int> CattegoryId { get; set; }
        [Required]
        [Display(Name = "ParametrList")]
        public List<Parametr> ParametrList { get; set; }
        [Required]
        [Display(Name = "BannedWords")]
        public List<string> BannedWords { get; set; }
        [Required]
        [Display(Name = "NecessaryWords")]
        public List<string> NecessaryWords { get; set; }
    }
}
