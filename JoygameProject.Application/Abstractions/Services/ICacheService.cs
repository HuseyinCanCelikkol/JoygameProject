﻿namespace JoygameProject.Application.Abstractions.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expireTime = null);
        Task RemoveAsync(string key);
    }
}
