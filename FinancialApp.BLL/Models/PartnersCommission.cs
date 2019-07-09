using FinancialApp.DAL.Model;
using System.Collections.Generic;

namespace FinancialApp.BLL.Models
{
    /// <summary>
    /// Partnerek jutalékai és vásárlásai.
    /// </summary>
    public class PartnersCommission
    {
        /// <summary>
        /// A Partner csapata.
        /// </summary>
        public List<Partner> Team { get; set; }

        /// <summary>
        /// Az adott Partner ID-ja.
        /// </summary>
        public decimal PARTNER_ID { get; set; }

        /// <summary>
        /// Csapat vásárlása.
        /// </summary>
        public decimal TEAM_PURCHASE { get; set; }

        /// <summary>
        /// Össz. vásárlás(csapat + saját).
        /// </summary>
        public decimal ALL_PURCHASE { get; set; }

        /// <summary>
        /// Az adott Partner jutaléka.
        /// </summary>
        public decimal COMMISSION { get; set; }

        /// <summary>
        /// Saját bónusza.
        /// </summary>
        public decimal OWN_PURCHASE { get; set; }

        /// <summary>
        /// Közvetlen gyermeke után járó bónusz.
        /// </summary>
        public decimal CHILDREN_PURCHASE { get; set; }

        /// <summary>
        /// Az adott Partner, mint szülő.
        /// </summary>
        public Partner PARENT { get; set; }
    }
}