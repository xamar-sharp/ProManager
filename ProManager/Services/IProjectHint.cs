using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProManager.Models;
using Microsoft.AspNetCore.Mvc;
namespace ProManager.Services
{
    public interface IProjectHint
    {
        Task<IEnumerable<string>> DisplayAsync();
    }
}
