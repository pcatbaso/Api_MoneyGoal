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
                        roles.createdDate = dr["createdDate"].ToString();
                        roles.updatedDate = dr["updateDate"].ToString();

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
    }
}
