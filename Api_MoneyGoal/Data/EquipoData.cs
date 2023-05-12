using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Transactions;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Data
{
    public class EquipoData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        public async Task<List<EquipoModel>> Consultar()
        {
            string cadenaConexion = conexion.CadenaConexion();

            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);

            var dtEquipo = new DataTable();
            var listaEquipo = new List<EquipoModel>();

            try {
                conn.Open();

                cmd = new MySqlCommand("sp_getTeams", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader();

                dtEquipo.Load(reader);

                if(dtEquipo.Rows.Count > 0)
                {
                    foreach(DataRow dr in dtEquipo.Rows)
                    {
                        EquipoModel equipo = new EquipoModel();

                        equipo.id = Convert.ToInt32(dr["id"]);
                        equipo.name = dr["name"].ToString();
                        equipo.active = Convert.ToInt32(dr["active"]) == 1 ? true : false;
                        equipo.fecha_creacion = DateTime.Parse(dr["createdDate"].ToString()).ToString("dd/MM/yyyy");
                        equipo.fecha_actualizacion = DateTime.Parse(dr["updateDate"].ToString()).ToString("dd/MM/yyyy");

                        listaEquipo.Add(equipo);
                    }                    
                }

                return listaEquipo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error catch: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<bool> Insertar(EquipoModel equipo)
        {
            string cadenaConexion = conexion.CadenaConexion();

            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_insertTeam", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("name_param", equipo.name));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                var t = cmd.Parameters["@resultado"].Value.ToString();

                if (t == "1")
                    return true;
                else
                    throw new Exception(t);
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

        public async Task<bool> Actualizar(EquipoModel equipo)
        {
            string cadenaConexion = conexion.CadenaConexion();

            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_updateTeam", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("id_param", equipo.id));
                cmd.Parameters.Add(new MySqlParameter("name_param", equipo.name));
                cmd.Parameters.Add(new MySqlParameter("active_param", 1));

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
