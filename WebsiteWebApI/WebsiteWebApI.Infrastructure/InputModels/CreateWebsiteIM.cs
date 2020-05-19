using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebsiteWebApI.Infrastructure.Attributes;

namespace WebsiteWebApI.Infrastructure.InputModels
{
    public class CreateWebsiteIM
    {
        public Guid Id { get; set; }

        [Required, MinLength(3)]
        public string Name { get; set; }

        [Required, MinLength(3)]
        public string URL { get; set; }

        [Required]
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }

        [DisplayName("Existing User")]
        public Guid UserId { get; set; }

        [MinLength(6)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [MinLength(6)]
        [DisplayName("Password")]
        public string Password { get; set; }


        [Required]
        [DisplayName("Home Page Snapshot")]
        public IFormFile HomePageSnapshot { get; set; }


        public string FileProviderName { get; set; }


    }
}
