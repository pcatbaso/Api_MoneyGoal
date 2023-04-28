using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
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
    }
}
