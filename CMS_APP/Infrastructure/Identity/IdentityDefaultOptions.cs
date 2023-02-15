﻿namespace CMS_APP.Infrastructure.Identity
{
    public class IdentityDefaultOptions
    {
        /*---------------------------------------------------------------------*/
        /*               Password Properties                                   */
        /*---------------------------------------------------------------------*/
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequireLength { get; set; }
        public bool PasswordRequireAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public int PasswordRequireUniqueChars { get; set; }
        /*---------------------------------------------------------------------*/
        /*               Lockout Properties                                    */
        /*---------------------------------------------------------------------*/
        public double LockoutDefaultLockoutTimeSpanInMinutes { get; set; }
        public int LockoutMaxFailedAccessAttempts { get; set; }
        public bool LockoutAllowedForNewUsers { get; set; }
        /*---------------------------------------------------------------------*/
        /*                  User Properties                                    */
        /*---------------------------------------------------------------------*/
        public bool UserRequireUniqueEmail { get; set; }
        public bool SignInRequireConfirmedEmail { get; set; }
        public string AccessDeniedPath { get; set; }

    }
}
