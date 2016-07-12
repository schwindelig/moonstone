using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace moonstone.ui.web.Models.ViewModels.Group
{
    public class CreateGroupViewModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Name { get; set; }
    }
}