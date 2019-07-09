using System;

namespace FinancialApp.DAL.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class FinancialItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal PARTNER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal AMOUNT { get; set; }
    }
}