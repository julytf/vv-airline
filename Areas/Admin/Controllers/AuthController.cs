using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Route("/admin/auth")]
public class AuthController : AppBaseController
{
    public AuthController()
    {
    }
}