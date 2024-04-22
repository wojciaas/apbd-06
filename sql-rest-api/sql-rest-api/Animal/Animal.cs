using System.ComponentModel.DataAnnotations;

namespace sql_rest_api.Animal;

public class Animal
{
    public int ID { get; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [Required]
    [EnumDataType(typeof(AnimalCategory))]
    public AnimalCategory Category { get; set; }
    [EnumDataType(typeof(AnimalArea))]
    public AnimalArea Area { get; set; }
    
    public Animal(
        int id,
        string name,
        string? description,
        AnimalCategory category,
        AnimalArea area)
    {
        ID = id;
        Name = name;
        Description = description;
        Category = category;
        Area = area;
    }
}