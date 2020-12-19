# ZemBlog

Prerequisites to compile and run

- Visual Studio
- .Net 5 sdk
- Clean + Rebuild solution
- DATABASE setup
  - If you created the database using provided script (databaseTablesScript.sql)
    - go to appsettings.json on both API+MVC project and update the DefaultConnectionString value.
    
  - If you prefer can open PacakageManagerConsole(Set default project to Zemblog.Data) and run:
      - update-database (migration is already created in the project)
      * You can also use dotnet equivalent command line tool.
  
- MVC app is set to run as startup, you can choose the project to run o select multiple in vs config.

User credentials for MVC App (harcoded)
- writer role
 - username = escritor
 - password = pass123
 
- editor role
 - username = editor
 - password = pass123

 API (is not using authentication.)
 - It has 2 endpoints 
  - HttpMethod = get, Route = '/api/pendingposts' --> gets all post in pending aproval
  - HttpMethod = post, Route = '/api/pendingposts/updatestatus', Params = postId, action(1-Approve, 2-Reject) --> change post status as selected
  
MVC and API are published on Azure
 
 *Advise: they are using Azure app service free plan.
 
 MVC url --> https://zemblog.azurewebsites.net/
 API url --> https://zemblogapi.azurewebsites.net/


Demo build time ≈ 5 Hr
