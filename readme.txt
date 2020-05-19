Important Paths:

/ - default page with a list of items - it doesn't use the controller method for convince but implements identical logic

/WebsiteCreate - page for creating a website
/WebsiteEdit/{guid} - page for editing a specific website

/Website/GetWebsite/{guid} - get output model for website
/Website/DeleteWebsite/{guid} - delete website
/Website/Create/ - multi form post request for creating website
/Website/Edit/{guid} - multi form post request for editing website
/Website/GetWebsiteList - get list of website output model (paging only)
/Website/GetWebsiteListWithFiltes - get list of website output model(paging, sorting, filtering) - endpoint for dynamic filtering and sorting for any property in the target entity model

/{*url} - links for a simple preview of a website - authenticated and only owner authorized - allows for links with slashes | other pages and controllers have priority in routing


Website OutputModel: 

public class WebsiteListOM
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string Owner { get; set; }
       
        public string Url { get; set; }

        public string SnapshotUrl { get; set; }
    }

Website CreateModel:

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


Website CreateModel

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

1. Steps to create and initialize the database

Assuming sufficient permissions for creating a database:

1.1 Change connection string in \WebsiteWebApI\appsettings.json
1.2 Migrations are applied automatically on startup so there are no other steps needed;
1.3 Data seed is included with the following items:
User: test@test.com | Qwerty!123
Roles: SiteAdmin, Admin
Categories: Leasure, Sports
FileProviders: SqlBlobFileProvider

2. Steps to run code properly

2.1 Follow 1.1 in other to provide a valid connections string
2.2 Access to nuget.org is required for nuget packages
2.3 Adjust port if needed : Default https://localhost:44304/
2.4 Normal build should be sufficient

3. Assumptions

3.0 Small changes to the interface models were made to add more functionality
3.1 Assuming sufficient permissions for creating a database:
3.2 Username and Password when creating a website are not required if user is selected from the dropdown
3.3 Assuming all files are images
3.4 From Category (Vertical) - predefined list I assumed a that views are required to achieve this in any manner
3.5 Assuming some UI is required there is no specification for a required technology: chosen Razor Pages (for more variety)
3.6 Based on model some additional assumptions were made:
	3.6.1 Presence of files in the model for creating and editing assumes a multipart form data
	3.6.2 Category list leads to a need for a model in the database
	3.6.3 Requirement for extensibility and possibility for complex routing lead to Url structure in the database
	3.6.4 For extensibility sake files and file data are separated in the model. This lead to the need for file providers which would allow for easy addition to cloud storage, file storage and alternative database storages
	3.6.5 Changes of a db engine might required some modification of the entity models, but not the output models or the controllers. AutoMapper  was introduced for this reason.
	3.6.6 User can be created along with a website, but only password could be changed during edit. 

4. Feedback

4.1 Assignment should be more specific in my opinion. There are two many vague requirements, and it is somewhat difficult to gauge the wanted scope of a solution. 
4.2 It should be indicated if more entity models are desired or not
4.3 Extensibility should be better specified

5. Notes

5.1 I am well aware that the size and the complexity of the project might exceed the expected levels. This is closer to a base infrastructure for a more extensive project and there for some additional components were added.
5.2 I believe that BaseDataService.cs is the most interesting part of the solution as it is meant for a generic and dynamic approach to data services for any requests that can be covered by a single repository.
