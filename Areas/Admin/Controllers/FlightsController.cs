using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Route("/admin/flights")]
public class FlightsController : AppBaseController
{
    public FlightsController()
    {
    }
}