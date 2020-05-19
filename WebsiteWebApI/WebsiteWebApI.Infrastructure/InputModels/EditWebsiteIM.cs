using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebsiteWebApI.Infrastructure.InputModels
{
    public class EditWebsiteIM
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
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        [MinLength(6)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DisplayName("Home Page Snapshot")]
        public Guid FileId { get; set; }


        [Required]
        [DisplayName("Home Page Snapshot")]
        public IFormFile HomePageSnapshot { get; set; }


        public string FileProviderName { get; set; }
    }
}
