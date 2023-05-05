using Api_MoneyGoal.Data;
using Api_MoneyGoal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApuestaController
    {
        [HttpPost]
        [Route("registrarApuesta")]
        public async Task<Object> RegistrarApuesta(List<apuestaModel> apuestaUser)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                var apuesta = new apuestaData();

                bool resultado = await apuesta.Insertar(apuestaUser);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se registro correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo registar la apuesta correctmente");
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
