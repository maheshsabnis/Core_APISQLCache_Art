using System.Text.Json;
using Core_APICache_Art.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Core_APICache_Art.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly April2025Context _context;
        private readonly IDistributedCache _cache;
        public DepartmentController(April2025Context context,IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> Get()
        {
            var cacheKey = "departments";
            var cachedDepartments = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedDepartments))
            {
                var departments = JsonSerializer.Deserialize<List<Department>>(cachedDepartments);
                var response = new
                {
                    Message = "Data read from cache",
                    Data = departments
                };
                return Ok(response);
            }
            // save to cache
            var departmentsList = await _context.Departments.ToListAsync();
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(departmentsList), options);
            var responseFromDb = new
            {
                Message = "Data read from database",
                Data = departmentsList
            };
            return Ok(responseFromDb);
        }
        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> Get(int id)
        {
            var cacheKey = $"department_{id}";
            var cachedDepartment = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedDepartment))
            {
                var dept = JsonSerializer.Deserialize<Department>(cachedDepartment);
                var response = new
                {
                    Message = "Data read from cache",
                    Data = dept
                };
                return Ok(response);
            }
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            // save to cache
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(department), options);
            var responseFromDb = new
            {
                Message = "Data read from database",
                Data = department
            };
            return Ok(responseFromDb);
        }
    }
}
