namespace sql_rest_api.Animal;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    
    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        return _animalRepository.GetAnimals(orderBy);
    }

    public Animal CreateAnimal(AnimalDTO animalDTO)
    {
        return _animalRepository.CreateAnimal(animalDTO);
    }

    public int UpdateAnimal(AnimalDTO animalDTO, int id)
    {
        return _animalRepository.UpdateAnimal(animalDTO, id);
    }

    public int DeleteAnimal(int id)
    {
        return _animalRepository.DeleteAnimal(id);
    }
}