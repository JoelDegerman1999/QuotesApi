using System;
using System.Collections.Generic;

namespace QuotesApi.Models.Auth
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public AppUserViewModel AppUser { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpireDate { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}
