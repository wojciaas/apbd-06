using rest_api_warehouse.Interfaces;

namespace rest_api_warehouse.Services;

public class WarehouseService : IWarhouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository repository)
    {
        _warehouseRepository = repository;
    }
    
    
}