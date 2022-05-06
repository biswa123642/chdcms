  PreRequisites: 
  1.  Sitecore Experience Accelerator 10.2.0 rev. 04247  
  2.  Sitecore module versions
      	2.1 Install Sitecore.PowerShell.Extensions-6.3
      	2.2 Install Sitecore Experience Accelerator 10.2.0 rev. 04247
      	2.3 Install Sitecore ManagementServices 4.1.0 rev. 00492
  3.  Install currect Node version--> "node-v > 14.17.3"
  4.  Install currect Npm version--> "npm -v > 6.14.13"

1. Publish properties in gulp-config config object:
	    webRoot: "C:\\inetpub\\wwwroot\\centralsc.dev.local",
        devRoot: "C:\Projects\CGP",
        solutionName: "CGP",
        buildConfiguration: "Debug",
        MSBuildToolsVersion: 16.0

2.  Run following commands in sequence
		npm init      >>>       to create package.json
        npm install gulp@3.9.1 --save-dev
        npm install gulp-msbuild
        npm install gulp-debug
        npm install gulp-foreach
		    npm install run-sequence
		
3. Open Task Runner Explorer and refresh to see task updated
      -- alternatively from terminal --type the command "gulp" and enter

4. If you see "ReferenceError: Premordials not defined" after running Gulp - 
    1. Create a file at root level named npm-shrinkwrap.json
    2. Copy and paste following lines in that file:
        {
            "dependencies": {
              "graceful-fs": {
                "version": "4.2.2"
                  }
            }
          }
	  3. Run npm install again	

5. make sure identityserver config file is deployed

  Location :- "~\identityserver\Config\Sitecore.IdentityServer.CGP.xml" to the location "<wwwroot>\.<identityserver>\Config"

6. make sure CLI config file is deployed to CM role.

  Location :- "~/src\Project\Website\code\App_Config\Environment\Project\Project.Owin.CGP_CLI.config"

7.  Serialization steps
    7.1 Goto path "~\tools\serialization" and execute the powershell commands,

          > login.ps1  --> to save login details (need to performed first time only)
              >>execute "dotnet tool restore" (need to performed first time only)

          > pull.ps1  --> to serialize the changes from sitecore to git repository

          > push.ps1  --> to update sitecore content tree

          > validate.ps1  --> to fix any error in serialization like to remove orphaned items/folders
		
8. Git Commands
    8.1 Goto the root path and execute the commands,

        > git fetch --> to fetch all the changes to your branch

        > git pull --> to update your branch

        > git merge origin/develop --> to merge all the changes from develop branch to your local branch

        > git commit -m "<commit message>"  --> to commit your staged changes to project history

        > git push --> to upload the changes from your local branch to remote branch


  
