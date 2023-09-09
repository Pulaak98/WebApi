using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApi.DatabaseContext;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebApiDbContext webApiDbcontext;

        public UsersController(WebApiDbContext webApiDbcontext)
        {
            this.webApiDbcontext = webApiDbcontext;
        }

        [HttpPost]
        public async Task<IActionResult>RegisterUser(User user)
        {

            var userCreate = new User()
            {
                Id = Guid.NewGuid(),
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
            };
            // await webApiDbcontext.UserInfo.AddAsync(//userCreate);
            //await webApiDbcontext.SaveChangesAsync();
            await webApiDbcontext.Database.ExecuteSqlInterpolatedAsync($@"EXEC InsertUser @UserId={userCreate.Id},@UserName={userCreate.UserName},@UserEmail={userCreate.Email},@UserPassword={userCreate.Password}");
            return Ok(userCreate);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser logInUser)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            else
            {
                var loginUser=await webApiDbcontext.UserInfo.
                   FirstOrDefaultAsync(u=>(u.Email==logInUser.Email)&&(u.Password==logInUser.Password));

                // var loginUser = await webApiDbcontext.UserInfo.FromSqlRaw("EXEC FindUserByEmailPassword @Email,@Password",
                    //new SqlParameter("@Email",logInUser.Email),new SqlParameter("@Password",logInUser.Password)).
                    //FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return NotFound();
                }else return Ok(loginUser);
            }
        }
    }
}
