using Api_MoneyGoal.Models;
using Microsoft.AspNetCore.Mvc;
using Api_MoneyGoal.Data;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class usersControllers
    {
        [HttpGet]
        [Route("obtenerUsuarios")]
        public async Task<ActionResult<List<Object>>> obtenerUsuarios()
        {
            List<Object> listResponse = new List<Object>();

            try
            {
                var dataUser = new usersData();
                var lista = await dataUser.ConsultarUsuarios();                

                if(lista.Count > 0)
                {
                    listResponse.Add("OK");
                    listResponse.Add(lista);
                }
                else
                {
                    listResponse.Add("Error");
                    listResponse.Add("No hay usuarios registrados");
                }
            }
            catch (Exception ex)
            {
                listResponse.Add("Error: " + ex.Message);
            }

            return listResponse;
        }
    }
}
