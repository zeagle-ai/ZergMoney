using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using ZergMoney.Models;

namespace ZergMoney.Helpers
{
    public static class HelperExtensions
    {
        public static string GetFullName(this IIdentity user)
        {
            var ClaimsUser = (ClaimsIdentity)user;
            var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == "FullName");
            return claim?.Value;
        }

        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseholdClaim = claimsIdentity.Claims
                .FirstOrDefault(c => c.Type == "HouseholdId");
            if (HouseholdClaim != null)
                return int.Parse(HouseholdClaim.Value);
            else
                return null;
        }

        public static bool IsInHousehold(this IIdentity user)
        {
            var cUser = (ClaimsIdentity)user;
            var hid = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (hid != null && !string.IsNullOrWhiteSpace(hid.Value));
        }

        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            context.GetOwinContext().Authentication.SignOut();
            await context.GetOwinContext().Get<ApplicationSignInManager>()
                .SignInAsync(user, isPersistent: false, rememberBrowser: false);
        }
    }
}