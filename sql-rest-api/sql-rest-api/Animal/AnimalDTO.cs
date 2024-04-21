namespace sql_rest_api.Animal;

public record AnimalDTO(
    string Name,
    string? Description, 
    AnimalCategory Category,
    AnimalArea Area
    )
{
}