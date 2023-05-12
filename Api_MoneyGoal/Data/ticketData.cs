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

        public async Task<bool> Insertar(ticketModel ticket)
        {
            string cadenaConexion = conexion.CadenaConexion();
            conn = new MySqlConnection(cadenaConexion);

            MySqlCommand cmd = null;
            MySqlCommand cmdD = null;

            conn.Open();

            var transaction = conn.BeginTransaction();

            try
            {
                cmd = new MySqlCommand("sp_insertTicketBet", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("idTicketBet_param", ticket.idTicketBet));
                cmd.Parameters.Add(new MySqlParameter("active_param", ticket.active));
                cmd.Parameters.Add(new MySqlParameter("dateActive_param", ticket.dateDeactive));
                cmd.Parameters.Add(new MySqlParameter("dateDeactive_param", ticket.dateDeactive));

                cmd.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                cmd.Parameters["@resultado"].Direction = ParameterDirection.Output;

                var reader = cmd.ExecuteNonQuery();

                var t = cmd.Parameters["@resultado"].Value.ToString();

                if (Convert.ToInt16(t) > 0)
                {
                    cmdD = new MySqlCommand("sp_insertTicketBet_Detail", conn);
                    cmdD.CommandType = CommandType.StoredProcedure;

                    foreach (var ticketItem in ticket.listTicketDetail)
                    {
                        cmdD.Parameters.Clear();
                        cmdD.Parameters.Add(new MySqlParameter("idTicketBet_param", ticketItem.idTicketBet));
                        cmdD.Parameters.Add(new MySqlParameter("numGame_param", ticketItem.numGame));
                        cmdD.Parameters.Add(new MySqlParameter("idLocalTeam_param", ticketItem.idLocalTeam));
                        cmdD.Parameters.Add(new MySqlParameter("idVisitingTeam_param", ticketItem.idVisitingTeam));

                        cmdD.Parameters.Add(new MySqlParameter("@resultado", MySqlDbType.VarChar));
                        cmdD.Parameters["@resultado"].Direction = ParameterDirection.Output;

                        cmdD.ExecuteNonQuery();

                        var t1 = cmdD.Parameters["@resultado"].Value.ToString();

                        if (t1 != "Éxito")
                            throw new Exception("Ocurrio un error al insertar el ticket");
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

        public async Task<List<ticketModel>> ConsultarTicketDisponibles()
        {
            string cadenaConexion = conexion.CadenaConexion();

            MySqlCommand cmd = null;
            conn = new MySqlConnection(cadenaConexion);

            DataTable dtTicket = new DataTable();
            List<ticketModel> listaTicket = new List<ticketModel>();
            List<ticketDetailModel> listaTicketDetail = new List<ticketDetailModel>();

            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_getBetAvailable", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader();

                dtTicket.Load(reader);



                if (dtTicket.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTicket.Rows)
                    {
                        ticketModel ticket = new ticketModel();

                       // listaTicketDetail = dr.

                        //ticket.idTicketBet = Convert.ToInt32(dr["id"]);
                        //ticket.name = dr["name"].ToString();
                        //ticket.description = dr["description"].ToString();
                        //ticket.active = Convert.ToInt32(dr["active"]) == 1 ? true : false;
                        //ticket.createdDate = DateTime.Parse(dr["createdDate"].ToString()).ToString("dd/MM/yyyy");
                        //ticket.updatedDate = DateTime.Parse(dr["updateDate"].ToString()).ToString("dd/MM/yyyy");

                        //listaTicket.Add(ticket);
                    }
                }

                return listaTicket;
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
