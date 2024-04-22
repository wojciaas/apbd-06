namespace sql_rest_api.Animal;

public static class Configuration
{
    public static IEndpointRouteBuilder RegisterAnimalEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/animals", (HttpRequest request, IAnimalService service) =>
        {
            string orderBy = request.Query["orderBy"].FirstOrDefault() ?? "name";
            return TypedResults.Ok(service.GetAnimals(orderBy));
        });
        endpoints.MapPost("api/animals", (AnimalDTO animalDTO, IAnimalService service) => TypedResults.Created("", service.CreateAnimal(animalDTO)));
        endpoints.MapPut("api/animals/{id:int}", (int id, AnimalDTO animalDTO, IAnimalService service) =>
        {
            return service.UpdateAnimal(animalDTO, id) == 0 ? (IResult) TypedResults.NotFound() : TypedResults.NoContent();
        });
        endpoints.MapDelete("api/animals/{id:int}", (int id, IAnimalService service) =>
        {
            return service.DeleteAnimal(id) == 0 ? (IResult) TypedResults.NotFound() : TypedResults.NoContent();
        });
        
        return endpoints;
    }
}