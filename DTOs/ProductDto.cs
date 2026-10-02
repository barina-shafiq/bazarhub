namespace WebProject.DTOs;

public record ProductDto(int Id, string Name, string Brand, decimal Price, decimal EffectivePrice, string ImagePath, double AverageRating);
