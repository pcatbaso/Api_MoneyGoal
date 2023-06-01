using Api_MoneyGoal.Models;
using Api_MoneyGoal.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apuestaUserController : Controller
    {
        [HttpPost]
        [Route("registrarApuestaUsuario")]
        public async Task<Object> RegistrarTicket(ticketModel apuesta)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                apuestaUserData ticketUserData = new apuestaUserData();

                bool resultado = await ticketUserData.InsertarApuesta(apuesta);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se registro correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo registar el ticket correctmente");
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
