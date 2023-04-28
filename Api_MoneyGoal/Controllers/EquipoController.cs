using Api_MoneyGoal.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController
    {
        [HttpGet]
        [Route("obtenerEquipo")]
        public async Task<ActionResult<List<Object>>> Listar()
        {
            List<Object> listResponse = new List<Object>();

            try
            {
                EquipoData dataEquipo = new EquipoData();
                var lista = await dataEquipo.Consultar();

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
            catch(Exception ex)
            {
                listResponse.Add("Error: " + ex.Message);
            }

            return listResponse;
        }
    }
}
