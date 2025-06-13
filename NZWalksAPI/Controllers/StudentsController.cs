using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    //https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] students =
            [
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown"
            ];

            return Ok(students);
        }
    }
}
