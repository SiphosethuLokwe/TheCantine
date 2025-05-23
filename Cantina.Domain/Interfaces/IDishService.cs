﻿using Cantina.Domain.Entities;

public interface IDishService
{
    Task CreateAsync(Dish dish, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Dish>> GetAllAsync(int page , int pageSize,CancellationToken cancellationToken);
    Task<Dish?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Dish?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task UpdateAsync(Dish dish, CancellationToken cancellationToken);
    Task<IEnumerable<Dish>> SearchAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken);

}