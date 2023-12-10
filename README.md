# Graph User Admin

## Technology
This is a web application built on .NET8 with a Blazor WASM UI which is hosted as a RCL (Razor Component Library) within a web API project. 
The application is protected by authentication based on Microsoft Entra ID and it uses Microsoft.Graph to interact with an Azure Entra ID (or Azure AD B2C) directory.

## Purpose
Managing Graph Users and illustrating how to use paging with Graph resources.

## Set up
1. In your Azure Entra ID tenant set up a new application registration for the API. 
   This app registration should have the following settings:
    - Supported account types: "Accounts in this organizational directory only"
    - Redirect URI (web) = "https://localhost:5001/swagger/oauth2-redirect.html"
    - Expose Access and Id tokens (needed just for the Swagger UI authorisation - optional)
    - Create an application secret (copy the value for later)
    - Expose an API: set an Application ID URI and add a scope: (e.g. User.ReadWrite.All)
2. In your Azure Entra ID tenant set up a new application registration for the UI.
   This app registration should have the following settings:
   - Supported account types: "Accounts in this organizational directory only"
   - Redirect URI (SPA) = https://localhost:5001/authentication/login-callback
   - API permissions: give delegated access to the API scope created in step 1 and grant admin consent
3. In the Azure tenant which hosts the users that will be managed by the application (e.g. an Azure Entra ID tenant, or an Azure AD B2C tenant) create an app registration which will be used to obtain application Graph permissions to manage the users.
   This app registration should have the following settings:
   - Supported account types: "Accounts in this organizational directory only"
   - Create an application secret (copy the value for later)
   - API permissions: add the following Graph permissions and grant admin consent:
       - User.ReadWrite.All
       - User.ManageIdentities.All
4. Replace the values in the appSettings.json file in the GraphUserAdmin.API project with the values taken from the API app registration set up in step 1 and the Graph client app registration set up in step 3.
5. Replace the values in the appsettings.json files in the GraphUserAdmin.UI project with the values taken from the app registration set up in step 2.

## Run locally
1. Build solution
2. Run the GraphUserAdmin.API project. This will expose the API on https://localhost:5001/api and the UI on https://localhost:5001/. The web API application also creates a swagger endpoint on https://localhost:5001/swagger
3. Log in to the UI web app using your Azure Entra Id account. In a real scenario you would create an Enterprise application for the UI app registration and limit the access to specific users.

## Deploy to app service
All you will need to deploy is GraphUserAdmin.API. This will deploy the API on the root and the UI in a "wwwroot" folder. Make sure that the configuration keys are set correctly on both the API and the UI.
