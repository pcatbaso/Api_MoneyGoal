using Api_MoneyGoal.Data;
using Api_MoneyGoal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ticketController
    {
        [HttpPost]
        [Route("regitrarTicket")]
        public async Task<Object> RegistrarTicket(ticketModel ticket)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                ticketData ticketData = new ticketData();

                bool resultado = await ticketData.Insertar(ticket);

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

        [HttpGet]
        [Route("ticketActivos")]
        public async Task<ActionResult<List<Object>>> ListarTicketActivos(int idTicket_param)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                ticketData ticketData = new ticketData();
                var lista = await ticketData.ConsultarTicketDisponibles(idTicket_param);

                if (lista.Count > 0)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add(lista);
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No hay tickets disponibles");
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
