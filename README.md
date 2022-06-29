# AAD B2C User Admin

## Technology
This is a web application built on .NET6 with a Blazor WASM UI which is hosted as a RCL (Razor Component Library) within a web API project. 
The application is protected by authentication based on AAD B2C and it uses Microsoft.Graph to interact with the AAD B2C directory. 

## Purpose
Managing the AAD B2C users in the directory

## Set up
1. Set up a B2C tenant
2. Create an app registration for the API
3. Set supported account types: "My organization only"
4. Set a redirect URI (web) = https://localhost:5001/swagger/oauth2-redirect.html
5. Expose Access and Id tokens (needed just for the Swagger UI)
6. Create a client secret (copy the value for later)
7. API permissions: give delegated access to Microsoft Graph: offline_access and openid. Give Application access to User.ManageIdentities.All and User.ReadWrite.All. Grant admin consent.
8. Expose an API: Set an application ID URI and add a scope: User.ReadWrite.All
9. Create an app registration for the UI
10. Set supported account types: "My organization only"
11. Set a redirect URI (SPA) = https://localhost:5001/authentication/login-callback
12. API permissions: give delegated access to Microsoft Graph: offline_access and openid. Give delegated access to the API scope User.ReadWrite.All created earlier in the API app registration (select an API, My APIs)
13. Replace the values in the appsettings.json files in the B2CUserAdmin.API and the B2CUserAdmin.UI project with the values taken from the B2C tenant.

## Run locally
1. Build solution
2. Run the B2CUserAdmin.API project. This will expose the API on https://localhost:5001/api and the UI on https://localhost:5001/. 
3. Log in to the UI using your B2C account. In a real scenario you would create an Enterprise application and limit the access to specific users.

## Deploy to app service
All you will need to deploy is B2CUserAdmin.API. This will deploy the API on the root and the UI in a "wwwroot" folder. Make sure that the configuration keys are set correctly on both files /appsettings.json and /wwwroot/appsettings.json.
