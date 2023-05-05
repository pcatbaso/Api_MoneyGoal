using webApi_MoneyGoal;
using MySql.Data.MySqlClient;
using Api_MoneyGoal.Models;
using System.Data;
using System.Linq;

namespace Api_MoneyGoal.Data
{
    public class apuestaData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;
        public async Task<bool> Insertar(List<apuestaModel> apuesta)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);

            MySqlCommand cmd = null;

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_insertBetUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var ap in apuesta)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new MySqlParameter("idUser_param", ap.idUser));
                    cmd.Parameters.Add(new MySqlParameter("idTicket_param", ap.idTicket));
                    cmd.Parameters.Add(new MySqlParameter("numGame_param", ap.numGame));
                    cmd.Parameters.Add(new MySqlParameter("local_param", ap.local));
                    cmd.Parameters.Add(new MySqlParameter("draw_param", ap.draw));
                    cmd.Parameters.Add(new MySqlParameter("visitor_param", ap.visitor));
                    cmd.Parameters.Add(new MySqlParameter("cost_param", ap.cost));
                    cmd.ExecuteNonQuery();                    
                }

                conn.Close();
                return true;

            }
            catch (Exception ex)
            {                
                throw new Exception("Error: " + ex.Message);
            }
           
        }

    }
}
