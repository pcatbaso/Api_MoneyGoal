using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Data
{
    public class rolesData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        public async Task<bool> Insertar(rolesModel rol)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);

            MySqlCommand cmd = null;

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_insertRol", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new MySqlParameter("name_param", rol.name));
                cmd.Parameters.Add(new MySqlParameter("description_param", rol.description));
                cmd.Parameters.Add(new MySqlParameter("active_param", 1));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                var resultado = cmd.Parameters["@resultado"].Value.ToString();

                if (resultado != "1")
                    throw new Exception("Ocurrio un error al insertar el ticket");

                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<rolesModel>> Consultar()
        {
            string cadenaConexion = conexion.CadenaConexion();

            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);

            var dtRoles = new DataTable();
            var listaRoles = new List<rolesModel>();

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_getRoles", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader();

                dtRoles.Load(reader);

                if (dtRoles.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoles.Rows)
                    {
                        rolesModel roles = new rolesModel();

                        roles.id = Convert.ToInt32(dr["id"]);
                        roles.name = dr["name"].ToString();
                        roles.description = dr["description"].ToString();
                        roles.active = Convert.ToInt32(dr["active"]) == 1 ? true : false;
                        roles.createdDate = DateTime.Parse(dr["createdDate"].ToString()).ToString("dd/MM/yyyy");
                        roles.updatedDate = DateTime.Parse(dr["updateDate"].ToString()).ToString("dd/MM/yyyy");

                        listaRoles.Add(roles);
                    }
                }

                return listaRoles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> Actualizar(rolesModel rol)
        {
            string cadenaConexion = conexion.CadenaConexion();
            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_updateRol", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("id_param", rol.id));
                cmd.Parameters.Add(new MySqlParameter("name_param", rol.name));
                cmd.Parameters.Add(new MySqlParameter("description_param", rol.description));
                cmd.Parameters.Add(new MySqlParameter("active_param", rol.active));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                var t = cmd.Parameters["@resultado"].Value.ToString();

                if (t == "1")
                    return true;
                else
                    throw new Exception(t);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
