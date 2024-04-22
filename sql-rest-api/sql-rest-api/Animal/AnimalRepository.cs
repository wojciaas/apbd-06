using System.Data.SqlClient;

namespace sql_rest_api.Animal;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        switch (orderBy)
        {
            case "name":                 
                cmd.CommandText = "SELECT IdAnimal, Name, Description, Category, Area FROM cw6.Animal ORDER BY Name ASC";                 
                break;             
            case "description":                 
                cmd.CommandText = "SELECT IdAnimal, Name, Description, Category, Area FROM cw6.Animal ORDER BY Description ASC";                 
                break;             
            case "category":                 
                cmd.CommandText = "SELECT IdAnimal, Name, Description, Category, Area FROM cw6.Animal ORDER BY Category ASC";                 
                break;             
            case "area":                 
                cmd.CommandText = "SELECT IdAnimal, Name, Description, Category, Area FROM cw6.Animal ORDER BY Area ASC";                 
                break;             
            default:                 
                throw new ArgumentException("Invalid orderBy parameter");
        }
        
        SqlDataReader dr = cmd.ExecuteReader();
        List<Animal> animals = new List<Animal>();
        while (dr.Read())
        {
            animals.Add(new Animal((int)dr["IdAnimal"],
                dr["Name"].ToString(),
                dr["Description"].ToString(),
                (AnimalCategory)Enum.Parse(typeof(AnimalCategory), dr["Category"].ToString()), 
                (AnimalArea)Enum.Parse(typeof(AnimalArea), dr["Area"].ToString())));
        }
        
        return animals;
    }

    public Animal CreateAnimal(AnimalDTO animalDTO)
    {
        (string name, string? description, AnimalCategory category, AnimalArea area) = animalDTO;
        
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO cw6.Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area);SELECT @@IDENTITY";
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@Category", category);
        cmd.Parameters.AddWithValue("@Area", area);
        
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            return new Animal((int)(decimal)dr[0], name, description, category, area);
        }
        
        throw new Exception("Could not create animal");
    }

    public int UpdateAnimal(AnimalDTO animalDTO, int id)
    {
        (string name, string? description, AnimalCategory category, AnimalArea area) = animalDTO;
        
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE cw6.Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal;SELECT @@ROWCOUNT";
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@Category", category);
        cmd.Parameters.AddWithValue("@Area", area);
        cmd.Parameters.AddWithValue("@IdAnimal", id);
        return cmd.ExecuteNonQuery();
    }

    public int DeleteAnimal(int id)
    {
        using SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM cw6.Animal WHERE IdAnimal = @IdAnimal;SELECT @@ROWCOUNT";
        cmd.Parameters.AddWithValue("@IdAnimal", id);
        
        return cmd.ExecuteNonQuery();
    }
}