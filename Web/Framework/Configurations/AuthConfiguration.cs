﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;

namespace Web.Framework.Configurations
{
    public class AuthConfiguration
    {
        public static void Init(IConfiguration configuration, IServiceCollection services)
        {
            var oauthEvents = new OAuthEvents
            {
                OnTicketReceived = context =>
                {
                    //TODO
                    //Look for the user in the database
                    //If the user doesn't exist create it
                    //if it exists put the userid in memory
                    return Task.CompletedTask;
                },
            };

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.SlidingExpiration = true;
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                googleOptions.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                googleOptions.ClaimActions.Clear();
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                googleOptions.ClaimActions.MapJsonKey("urn:google:profile", "link");
                googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                googleOptions.SaveTokens = true;

                googleOptions.Events = oauthEvents;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.Events = oauthEvents;
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                microsoftOptions.Events = oauthEvents;
            })
            .AddLinkedIn(linkedinOptions =>
            {
                linkedinOptions.ClientId = configuration["Authentication:LinkedIn:ClientId"];
                linkedinOptions.ClientSecret = configuration["Authentication:LinkedIn:ClientSecret"];
                linkedinOptions.Events = oauthEvents;
            });

            //TODO Include issue for github
            /*.AddGitHub(githubOptions => {
                githubOptions.ClientId = configuration["Authentication:Github:ClientId"];
                githubOptions.ClientSecret = configuration["Authentication:Github:ClientSecret"];
                githubOptions.Scope.Add("user:email");
                githubOptions.CallbackPath = "/account/HandleExternalLogin";
            });*/
        }
    }
}
