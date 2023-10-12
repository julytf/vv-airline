using Microsoft.AspNetCore.Mvc;
using vv_airline.Controllers;
using vv_airline.Models.Data;

namespace vv_airline.Areas.SearchAndBooking.Controllers;

[Area("API")]
[Route("/api/address")]
public class AddressController : AppBaseController
{
    private readonly AppDBContext _appDBContext;
    public AddressController(
        AppDBContext appDBContext
        )
    {
        _appDBContext = appDBContext;
    }

    [HttpGet("provinces")]
    public async Task<IActionResult> Provinces()
    {
        List<Province> provinces = _appDBContext.Provinces.ToList();
        return Json(new { provinces });
    }

    [HttpGet("districts")]
    public async Task<IActionResult> Districts()
    {
        List<District> districts = _appDBContext.Districts.ToList();
        return Json(new { districts });
    }

    [HttpGet("districts/{provinceCode}")]
    public async Task<IActionResult> Districts(string provinceCode)
    {
        List<District> districts = _appDBContext.Districts.Where(d => d.ProvinceCode == provinceCode).ToList();
        return Json(new { districts });
    }

    [HttpGet("wards")]
    public async Task<IActionResult> Wards()
    {
        List<Ward> wards = _appDBContext.Wards.ToList();
        return Json(new { wards });
    }

    [HttpGet("wards/{districtCode}")]
    public async Task<IActionResult> Wards(string districtCode)
    {
        List<Ward> wards = _appDBContext.Wards.Where(d => d.DistrictCode == districtCode).ToList();
        return Json(new { wards });
    }
}