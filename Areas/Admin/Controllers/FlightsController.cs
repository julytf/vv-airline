using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin/flights")]
public class FlightsController : AppBaseController
{
    public FlightsController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        throw new NotImplementedException();
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        throw new NotImplementedException();
    }

    // [HttpPost("create")]
    // public async Task<IActionResult> Create()
    // {
    //     throw new NotImplementedException();
    // }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/update")]
    public async Task<IActionResult> Update(int id)
    {
        throw new NotImplementedException();
    }

    // [HttpPut("{id}/update")]
    // public async Task<IActionResult> Update(int id)
    // {
    //     throw new NotImplementedException();
    // }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}