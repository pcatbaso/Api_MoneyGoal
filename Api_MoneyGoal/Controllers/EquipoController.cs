using Api_MoneyGoal.Data;
using Api_MoneyGoal.Models;
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

        [HttpPost]
        [Route("insertarEquipo")]
        public async Task<Object> InsertarUsuario(EquipoModel equipo)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                var equipoD = new EquipoData();

                bool resultado = await equipoD.Insertar(equipo);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se registro correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo registar el equipo correctmente");
                }
            }
            catch (Exception ex)
            {
                listaResponse.Add("Error: " + ex.Message);
            }

            return listaResponse;
        }

        [HttpPut]
        [Route("actualizarEquipo")]
        public async Task<Object> Actualizar(EquipoModel equipo)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                EquipoData equipoData = new EquipoData();

                bool resultado = await equipoData.Actualizar(equipo);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se actualizo correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo actualizar el usuario correctamente");
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
