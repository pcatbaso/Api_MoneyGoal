using Api_MoneyGoal.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class rolesController
    {
        [HttpGet]
        [Route("obtenerRoles")]
        public async Task<ActionResult<List<Object>>> Listar()
        {
            List<Object> listResponse = new List<Object>();

            try
            {
                rolesData dataRoles = new rolesData();
                var lista = await dataRoles.Consultar();

                if (lista.Count > 0)
                {
                    listResponse.Add("OK");
                    listResponse.Add(lista);
                }
                else
                {
                    listResponse.Add("Error");
                    listResponse.Add("No hay equipos registrados");
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
