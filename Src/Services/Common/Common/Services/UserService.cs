﻿using Microsoft.AspNetCore.Http;

namespace Common.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public int GetId()
        {
            if (_httpContextAccessor.HttpContext?.User?.Identity is null)
            {
                throw new InvalidOperationException("User is not authenticated");
            }
            return int.Parse(_httpContextAccessor.HttpContext!.User!.Identity!.Name!);
        }
    }
}