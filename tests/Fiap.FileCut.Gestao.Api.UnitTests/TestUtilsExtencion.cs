using Fiap.FileCut.Gestao.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.FileCut.Gestao.Api.UnitTests
{
    internal static class TestUtilsExtencion
    {
        public static void SetUserAuth(this ControllerBase controller, Guid userId, string email)
        {
            var claims = new List<Claim>
        {
            new ("preferred_username", email),
            new (ClaimTypes.NameIdentifier, userId.ToString())
        };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }
    }
}
