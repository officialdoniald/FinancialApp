namespace FinancialApp.DAL.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Partner
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal PARTNER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PARTNER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal PARENT_PARTNER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal FEE_PERCENT { get; set; }
    }
}