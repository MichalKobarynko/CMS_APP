using CMS_APP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS_APP.FunctionalServices
{
    public class FunctionalSvc : IFunctionalSvc
    {
        private readonly AdminUserOptions _adminUserOptions;
        private readonly AppUserOptions _appUserOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public FunctionalSvc(
            IOptions<AdminUserOptions> adminUserOptions,
            IOptions<AppUserOptions> appUserOptions,
            IOptions<UserManager<ApplicationUser>> userManager)
        {
            _adminUserOptions = adminUserOptions.Value;
            _appUserOptions = appUserOptions.Value;
            _userManager = userManager.Value;
        }

        public async Task CreateDefaultAdminUser()
        {
            try
            {
                var adminUser = new ApplicationUser
                {
                    Email = _adminUserOptions.Email,
                    UserName = _adminUserOptions.Username,
                    EmailConfirmed = true,
                    ProfilePic = GetDefaultProfilePic(), //TODO
                    PhoneNumber = "123456789",
                    PhoneNumberConfirmed = true,
                    FirstName = _adminUserOptions.Firstname,
                    Lastname = _adminUserOptions.Lastname,
                    UserRole = "Administrator",
                    IsActive = true,
                    UserAddresses = new List<AddressModel>
                    { 
                        new AddressModel { Country = _adminUserOptions.Country, Type = "Billing" },
                        new AddressModel { Country = _adminUserOptions.Country, Type = "Shipping" },
                    }
                };

                var result = await _userManager.CreateAsync(adminUser, _adminUserOptions.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Administrator");
                    Log.Information("Admin User Created {UserName}", adminUser.UserName);
                }
                else
                {
                    var errorString = string.Join(",", result.Errors);
                    Log.Error("Error while creating user {error}", errorString);
                }


            }
            catch(Exception ex)
            {
                Log.Error("Error while creating user {Error} {StackTrace} {InnerException} {Source}",
                    ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
            }
        }

        public async Task CreateDefaultAppUser()
        {
            var appUser = new ApplicationUser
            {
                Email = _appUserOptions.Email,
                UserName = _appUserOptions.Username,
                EmailConfirmed = true,
                ProfilePic = GetDefaultProfilePic(), //TODO
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = true,
                FirstName = _appUserOptions.Firstname,
                Lastname = _appUserOptions.Lastname,
                UserRole = "Administrator",
                IsActive = true,
                UserAddresses = new List<AddressModel>
                {
                    new AddressModel { Country = _appUserOptions.Country, Type = "Billing" },
                    new AddressModel { Country = _appUserOptions.Country, Type = "Shipping" },
                }
            };

            var result = await _userManager.CreateAsync(appUser, _adminUserOptions.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, "Administrator");
                Log.Information("Admin User Created {UserName}", appUser.UserName);
            }
            else
            {
                var errorString = string.Join(",", result.Errors);
                Log.Error("Error while creating user {error}", errorString);
            }
        }

        private string GetDefaultProfilePic()
        {
            return string.Empty;
        }
    }
}
