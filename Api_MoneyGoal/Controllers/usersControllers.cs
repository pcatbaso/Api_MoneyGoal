using Api_MoneyGoal.Models;
using Microsoft.AspNetCore.Mvc;
using Api_MoneyGoal.Data;

namespace Api_MoneyGoal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class usersControllers
    {
        [HttpPost]
        [Route("insertarUsuario")]
        public async Task<Object> InsertarUsuario(usersModel usuario)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                var user = new usersData();

                bool resultado = await user.Insertar(usuario);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se registro correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo registar el usuario correctmente");
                }
            }
            catch (Exception ex)
            {
                listaResponse.Add("Error: " + ex.Message);
            }

            return listaResponse;   
        }

        [HttpGet]
        [Route("obtenerUsuarios")]
        public async Task<ActionResult<List<Object>>> Listar(string search_param, string email_param = null)
        {
            List<Object> listResponse = new List<Object>();

            try
            {
                var dataUser = new usersData();
                var lista = await dataUser.Consultar(search_param, email_param);                

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

        [HttpPut]
        [Route("actualizarUsuario")]
        public async Task<Object> Actualizar(usersModel usuario)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                usersData user = new usersData();

                bool resultado = await user.Actualizar(usuario);

                if(resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se actualizo correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo registrar el usuario correctamente");
                }                
            }
            catch(Exception ex)
            {
                listaResponse.Add("Error: " + ex.Message);
            }

            return listaResponse;
        }

        [HttpDelete]
        [Route("eliminarUsuario")]
        public async Task<Object> Eliminar(string id)
        {
            List<Object> listaResponse = new List<Object>();

            try
            {
                usersData user = new usersData();

                bool resultado = await user.Eliminar(id);

                if (resultado)
                {
                    listaResponse.Add("OK");
                    listaResponse.Add("Se actualizo correctamente");
                }
                else
                {
                    listaResponse.Add("Error");
                    listaResponse.Add("No se pudo registrar el usuario correctamente");
                }
            }
            catch(Exception ex)
            {
                listaResponse.Add("Error: " + ex.Message);
            }

            return listaResponse;
        }
    }
}
