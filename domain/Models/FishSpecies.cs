namespace Domain.Models;

public class FishSpecies
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ScientificName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}