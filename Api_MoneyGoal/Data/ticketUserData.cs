using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using System.Data;
using webApi_MoneyGoal;

namespace Api_MoneyGoal.Data
{
    public class ticketUserData
    {
        Conexion conexion = new Conexion();
        MySqlConnection conn;

        public async Task<bool> InsertarApuesta(ticketModel ticketApuesta)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);

            MySqlCommand cmd = null;
            MySqlCommand cmdD = null;

            conn.Open();

            var transaction = conn.BeginTransaction();

            try
            {
                cmd = new MySqlCommand("sp_insertBetUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("idUser_param", ticketApuesta.idUser));
                cmd.Parameters.Add(new MySqlParameter("idTicket_param", ticketApuesta.idTicketBet));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                var t = cmd.Parameters["@resultado"].Value.ToString();

                if (Convert.ToInt16(t) > 0)
                {
                    cmdD = new MySqlCommand("sp_insertBetUserDetail", conn);
                    cmdD.CommandType = CommandType.StoredProcedure;

                    foreach (var ticketItem in ticketApuesta.listTicketDetail)
                    {
                        cmdD.Parameters.Clear();
                        cmdD.Parameters.Add(new MySqlParameter("idTicket_param", ticketItem.idTicketBet));
                        cmdD.Parameters.Add(new MySqlParameter("numGame_param", ticketItem.numGame));
                        cmdD.Parameters.Add(new MySqlParameter("local_param", ticketItem.localApuesta));
                        cmdD.Parameters.Add(new MySqlParameter("draw_param", ticketItem.drawApuesta));
                        cmdD.Parameters.Add(new MySqlParameter("visitor_param", ticketItem.visitApuesta));
                        cmdD.Parameters.Add(new MySqlParameter("cost_param", ticketItem.costoApostado));

                        cmdD.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                        cmdD.Parameters["@resultado"].Direction = ParameterDirection.Output;

                        cmdD.ExecuteNonQuery();

                        var resultado = cmdD.Parameters["@resultado"].Value.ToString();

                        if (Convert.ToInt16(resultado) != 1)
                            throw new Exception("Ocurrio un error al insertar la apuesta del usuario");
                    }

                    transaction.Commit();
                    return true;
                }
                else
                {
                    throw new Exception("Ocurrio un error");
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
