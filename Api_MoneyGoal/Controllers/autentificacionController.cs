using Microsoft.AspNetCore.Mvc;
using Api_MoneyGoal.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class autentificacionController : Controller
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        private readonly string secretKey;

        public autentificacionController(IConfiguration config)
        {
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpGet]
        public IActionResult Validar(string email, string password)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);

            MySqlCommand cmd = null;

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_authentication", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("email_param", email));
                cmd.Parameters.Add(new MySqlParameter("password_param", password));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;
                var reader1 = cmd.ExecuteNonQuery();

                conn.Close();                

                if (Convert.ToInt16(cmd.Parameters["@resultado"].Value.ToString()) == 1)
                {
                    var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                    var claims = new ClaimsIdentity();

                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, email));

                    var tokenDescriptior = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddMinutes(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenConfig = tokenHandler.CreateToken(tokenDescriptior);

                    string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                    return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            //if (usuario.Email == "pcat@baso.com.mx" && usuario.Password == "root09")
            //{
            //    var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            //    var claims = new ClaimsIdentity();

            //    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Email));

            //    var tokenDescriptior = new SecurityTokenDescriptor
            //    {
            //        Subject = claims,
            //        Expires = DateTime.UtcNow.AddMinutes(1),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            //    };

            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var tokenConfig = tokenHandler.CreateToken(tokenDescriptior);

            //    string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            //    return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
            //}
            //else
            //{
            //    return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            //}
        }
    }
}
