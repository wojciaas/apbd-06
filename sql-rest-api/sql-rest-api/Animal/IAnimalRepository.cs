namespace sql_rest_api.Animal;

public interface IAnimalRepository
{
    IEnumerable<Animal> GetAnimals(string orderBy);
    Animal CreateAnimal(AnimalDTO animalDTO);
    int  UpdateAnimal(AnimalDTO animalDTO, int id);
    int  DeleteAnimal(int id);
}