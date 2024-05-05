using Microsoft.AspNetCore.Mvc;
using rest_api_warehouse.Interfaces;

namespace rest_api_warehouse.Controllers;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController : ControllerBase
{
    private readonly IWarhouseService _warehouseService;

    public WarehouseController(IWarhouseService service)
    {
        _warehouseService = service;
    }

    [HttpPost("api/warehouse")]
    public IActionResult AddGoods()
    {
        
    }  
}