using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Data
{
    public class usersData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        public async Task <List<usersModel>> ConsultarUsuarios()
        {
            string cadenaConexion = conexion.CadenaConexion();
            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);
            var dtUsers = new DataTable();

            var listaUsuarios = new List<usersModel>();

            try
            {
                conn.Open();
                cmd = new MySqlCommand("sp_getUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader();

                dtUsers.Load(reader);

                if(dtUsers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        usersModel user = new usersModel();
                        user.nombre_usuario = dr["name"].ToString();
                        user.apellidoPaterno_usuario = dr["lastNameP"].ToString();
                        user.apellidoMaterno_usuario = dr["lastNameM"].ToString();
                        user.direccion_usuario = dr["address"].ToString();
                        user.telefono_usuario = dr["phoneNumber"].ToString();
                        user.email_usuario = dr["email"].ToString();
                        user.contrasenia_usuario = dr["password"].ToString();
                        user.rol = Convert.ToInt32(dr["rol"]);
                        user.activo = Convert.ToInt32(dr["active"]) == 1 ? true : false;

                        listaUsuarios.Add(user);
                    }
                }

                return listaUsuarios;
            }
            catch(Exception ex)
            {
                throw new Exception("Error catch: " + ex.Message);
            }            
        }
    }
}
