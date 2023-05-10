using Api_MoneyGoal.Data;
using Api_MoneyGoal.Models;
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

        [HttpPut]
        [Route("actualizarRol")]
        public async Task<Object> Actualizar(rolesModel rol)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                rolesData rolData = new rolesData();

                bool resultado = await rolData.Actualizar(rol);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se actualizo correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo actualizar el rol correctamente");
                }
            }
            catch (Exception ex)
            {
                listaResponse.Add("Error: " + ex.Message);
            }

            return listaResponse;
        }
    }
}
