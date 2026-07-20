using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces
{
    public interface ISmsSender
    {
        Task SendAsync(string phoneNumber, string message);
    }
}
