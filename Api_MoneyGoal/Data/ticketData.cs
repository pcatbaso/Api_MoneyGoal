using Api_MoneyGoal.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
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
                cmd.Parameters.Add(new MySqlParameter("dateActive_param", DateTime.Parse(ticket.dateActive).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new MySqlParameter("dateDeactive_param", DateTime.Parse(ticket.dateDeactive).ToString("yyyy-MM-dd HH:mm:ss")));

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
                        cmdD.Parameters.Add(new MySqlParameter("startDate_param", DateTime.Parse(ticketItem.startDate).ToString("yyyy-MM-dd HH:mm:ss")));

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
            
            try
            {
                conn.Open();

                cmd = new MySqlCommand("sp_getBetAvailable", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader();

                dtTicket.Load(reader);

                if (dtTicket.Rows.Count > 0)
                {
                    var groupByIdTicket = dtTicket.AsEnumerable().GroupBy(row => row.Field<int>("idTicketBet")).ToList();
                    int item = 0;
                    foreach (var itemTicket in groupByIdTicket)
                    {
                        ticketModel ticket = new ticketModel();
                        ticket.idTicketBet = Convert.ToInt16(groupByIdTicket[item].FirstOrDefault().ItemArray[0]);
                        ticket.dateActive = DateTime.Parse(groupByIdTicket[item].FirstOrDefault().ItemArray[6].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        ticket.dateDeactive = DateTime.Parse(groupByIdTicket[item].FirstOrDefault().ItemArray[7].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        ticket.listTicketDetail = new List<ticketDetailModel>();


                        foreach (var itemTicketDetail in itemTicket)
                        {
                            ticketDetailModel ticketDetail = new ticketDetailModel();

                            ticketDetail.numGame = Convert.ToInt16(itemTicketDetail.ItemArray[1].ToString());
                            ticketDetail.idLocalTeam = Convert.ToInt16(itemTicketDetail.ItemArray[2].ToString());
                            ticketDetail.nameLocal = itemTicketDetail.ItemArray[3].ToString();
                            ticketDetail.idVisitingTeam = Convert.ToInt16(itemTicketDetail.ItemArray[4].ToString());
                            ticketDetail.nameVisitante = itemTicketDetail.ItemArray[5].ToString();
                            ticketDetail.startDate = DateTime.Parse(itemTicketDetail.ItemArray[8].ToString()).ToString("dd/MM/yyyy HH:mm:ss");

                            ticket.listTicketDetail.Add(ticketDetail);
                        }

                        item++;
                        listaTicket.Add(ticket);
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
