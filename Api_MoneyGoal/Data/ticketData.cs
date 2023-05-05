using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Data
{
    public class ticketData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        public async Task<bool> Insertar(List<ticketModel> ticket)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);

            MySqlCommand cmd = null;

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_insertTicketBet", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach(var t in ticket)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new MySqlParameter("idTicketBet_param", t.idTicketBet));
                    cmd.Parameters.Add(new MySqlParameter("numGame_param", t.numGame));
                    cmd.Parameters.Add(new MySqlParameter("idLocalTeam_param", t.idLocalTeam));
                    cmd.Parameters.Add(new MySqlParameter("idVisitingTeam_param", t.idVisitingTeam));
                    cmd.Parameters.Add(new MySqlParameter("active_param", t.active));

                    cmd.ExecuteNonQuery();
                }

                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
