using FinancialApp.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FinancialApp.DAL
{
    /// <summary>
    /// Adatbázis műveleteket megvalósító osztály.
    /// </summary>
    public class DBContext
    {
        #region Properties

        private readonly string _connectionString = string.Empty;


        private const string INSERT_PARTNER = "INSERT INTO [dbo].[PARTNER] ([PARTNER_NAME], [PARENT_PARTNER_ID], [FEE_PERCENT]) VALUES(@PARTNER_NAME,@PARENT_PARTNER_ID,@FEE_PERCENT);";

        private const string GET_PARTNERS = "SELECT * FROM [dbo].[PARTNER];";


        private const string INSERT_FINANCIALITEM = "INSERT INTO [dbo].[FINANCIAL_ITEM] ([PARTNER_ID], [DATE], [AMOUNT]) VALUES(@PARTNER_ID,@DATE,@AMOUNT);";

        private const string GET_FINANCIALITEMS = "SELECT * FROM [dbo].[FINANCIAL_ITEM];";

        private const string UPDATE_FINANCIALITEM = "UPDATE [dbo].[FINANCIAL_ITEM] SET DATE=@DATE,AMOUNT=@AMOUNT,PARTNER_ID=@PARTNER_ID WHERE ID=@ID;";

        private const string DELETE_FINANCIALITEM = "DELETE FROM [dbo].[FINANCIAL_ITEM] WHERE ID=@ID;";

        private const string DELETE_FINANCIALITEMS = "DELETE FROM [dbo].[FINANCIAL_ITEM];";

        private const string GET_FINANCIALITEM_BY_ID = "SELECT * FROM [dbo].[FINANCIAL_ITEM] WHERE ID=@ID;";

        private const string GET_FINANCIALITEM_BY_PARTNERID = "SELECT * FROM [dbo].[FINANCIAL_ITEM] WHERE PARTNER_ID=@PARTNER_ID;";


        private const string DELETE_DBCONTENT = "DELETE FROM [dbo].[FINANCIAL_ITEM];DELETE FROM [dbo].[PARTNER];DBCC CHECKIDENT ('PARTNER', RESEED, 0);";

        #endregion

        public DBContext()
        {
            _connectionString = Properties.Resources.ConnectionString;
        }

        /// <summary>
        /// Beszúr egy elemet a PARTNER táblába.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public int InsertPartner(Partner partner)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(INSERT_PARTNER, conn))
                    {
                        cmd.Parameters.Add(
                            new SqlParameter("@PARTNER_NAME", partner.PARTNER_NAME)
                            {
                                SqlDbType = System.Data.SqlDbType.NVarChar
                            }
                         );
                        cmd.Parameters.Add(
                            new SqlParameter("@PARENT_PARTNER_ID", partner.PARENT_PARTNER_ID)
                            {
                                SqlDbType = System.Data.SqlDbType.Decimal
                            }
                         );
                        cmd.Parameters.Add(
                            new SqlParameter("@FEE_PERCENT", partner.FEE_PERCENT)
                            {
                                SqlDbType = System.Data.SqlDbType.Decimal
                            }
                         );

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at InsertPartner(): {0}, {1}", ex.StackTrace, ex.Message);

                return -1;
            }
        }

        /// <summary>
        /// Kilistázza a PARTNER táblát.
        /// </summary>
        /// <returns></returns>
        public List<Partner> GetPartners()
        {
            List<Partner> partners = new List<Partner>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(GET_PARTNERS, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                Partner partner = new Partner
                                {
                                    PARTNER_ID = reader.GetDecimal(reader.GetOrdinal("PARTNER_ID")),
                                    PARTNER_NAME = reader.GetString(reader.GetOrdinal("PARTNER_NAME")),
                                    FEE_PERCENT = reader.GetDecimal(reader.GetOrdinal("FEE_PERCENT")),
                                    PARENT_PARTNER_ID = reader.GetDecimal(reader.GetOrdinal("PARENT_PARTNER_ID"))
                                };

                                partners.Add(partner);
                            }
                        }
                    }
                }
                return partners;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetPartners(): {0}, {1}", ex.StackTrace, ex.Message);

                return null;
            }
        }


        /// <summary>
        /// Beszúr egy elemet a FINANCIAL_ITEM táblába.
        /// </summary>
        /// <param name="financialItem">Ezt fogja beszúrni.</param>
        /// <returns>Mennyi mezőt szúrt be.</returns>
        public int InsertFinancialItem(FinancialItem financialItem)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(INSERT_FINANCIALITEM, conn))
                    {

                        cmd.Parameters.Add(
                            new SqlParameter("@DATE", financialItem.DATE)
                            {
                                SqlDbType = System.Data.SqlDbType.Date
                            }
                         );
                        cmd.Parameters.Add(
                            new SqlParameter("@AMOUNT", financialItem.AMOUNT)
                            {
                                SqlDbType = System.Data.SqlDbType.Decimal
                            }
                         );
                        cmd.Parameters.Add(
                            new SqlParameter("@PARTNER_ID", financialItem.PARTNER_ID)
                            {
                                SqlDbType = System.Data.SqlDbType.Decimal
                            }
                         );

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at InsertFinancialItem(): {0}, {1}", ex.StackTrace, ex.Message);

                return -1;
            }
        }

        /// <summary>
        /// Kilistázza a FINANCIAL_ITEM táblát.
        /// </summary>
        /// <returns></returns>
        public List<FinancialItem> GetFinancialItems()
        {
            List<FinancialItem> financialItems = new List<FinancialItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(GET_FINANCIALITEMS, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FinancialItem financialItem = new FinancialItem
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    PARTNER_ID = reader.GetDecimal(reader.GetOrdinal("PARTNER_ID")),
                                    DATE = reader.GetDateTime(reader.GetOrdinal("DATE")),
                                    AMOUNT = reader.GetDecimal(reader.GetOrdinal("AMOUNT"))
                                };

                                financialItems.Add(financialItem);
                            }
                        }
                    }
                }
                return financialItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetFinancialItems(): {0}, {1}", ex.StackTrace, ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Kivesz a FINANCIAL_ITEM táblából egy elemet.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FinancialItem GetFinancialItemByID(int id)
        {
            FinancialItem financialItem = new FinancialItem();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(GET_FINANCIALITEM_BY_ID, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                financialItem = new FinancialItem
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    PARTNER_ID = reader.GetDecimal(reader.GetOrdinal("PARTNER_ID")),
                                    DATE = reader.GetDateTime(reader.GetOrdinal("DATE")),
                                    AMOUNT = reader.GetDecimal(reader.GetOrdinal("AMOUNT"))
                                };
                            }
                        }
                    }
                }
                return financialItem;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetFinancialItemByID(): {0}, {1}", ex.StackTrace, ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Kivesz a FINANCIAL_ITEM táblából PARTNERID szerint az összes elemet.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FinancialItem> GetFinancialItemByPARTNERID(decimal id)
        {
            List<FinancialItem> financialItems = new List<FinancialItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(GET_FINANCIALITEM_BY_PARTNERID, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@PARTNER_ID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FinancialItem financialItem = new FinancialItem
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    PARTNER_ID = reader.GetDecimal(reader.GetOrdinal("PARTNER_ID")),
                                    DATE = reader.GetDateTime(reader.GetOrdinal("DATE")),
                                    AMOUNT = reader.GetDecimal(reader.GetOrdinal("AMOUNT"))
                                };

                                financialItems.Add(financialItem);
                            }
                        }
                    }
                }
                return financialItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetFinancialItemByPARTNERID(): {0}, {1}", ex.StackTrace, ex.Message);

                return null;
            }
        }


        /// <summary>
        /// Frissíti a FINANCIAL_ITEM tábla egy sorát.
        /// </summary>
        /// <param name="financialItem">Ezt fogja frissíteni.</param>
        /// <returns>Mennyi mezőt frissített.</returns>
        public int UpdateFinancialItem(FinancialItem financialItem)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(UPDATE_FINANCIALITEM, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", financialItem.ID);
                        cmd.Parameters.AddWithValue("@DATE", financialItem.DATE);
                        cmd.Parameters.AddWithValue("@AMOUNT", financialItem.AMOUNT);
                        cmd.Parameters.AddWithValue("@PARTNER_ID", financialItem.PARTNER_ID);

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at UpdateFinancialItem(): {0}, {1}", ex.StackTrace, ex.Message);

                return -1;
            }
        }

        /// <summary>
        /// Kitöröl egy sort a FINANCIAL_ITEM táblából.
        /// </summary>
        /// <param name="id">Ezzel az ID-val rendelkező sort fogja kitörölni.</param>
        /// <returns></returns>
        public int DeleteFinancialItem(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(DELETE_FINANCIALITEM, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at DeleteFinancialItem(): {0}, {1}", ex.StackTrace, ex.Message);

                return -1;
            }
        }

        /// <summary>
        /// Kitörli a FINANCIAL_ITEM táblát.
        /// </summary>
        /// <param name="id">Ezzel az ID-val rendelkező sort fogja kitörölni.</param>
        /// <returns></returns>
        public int DeleteFinancialItems()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(DELETE_FINANCIALITEMS, conn))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at DeleteFinancialItems(): {0}, {1}", ex.StackTrace, ex.Message);

                return -1;
            }
        }

        /// <summary>
        /// Törli a DB tartalmát.
        /// </summary>
        /// <returns></returns>
        public int DeleteDBContent()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(DELETE_DBCONTENT, conn))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at DeleteDBContent(): {0}, {1}", ex.StackTrace, ex.Message);

                return -1;
            }
        }
    }
}