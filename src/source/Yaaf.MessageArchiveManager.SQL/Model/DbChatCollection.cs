// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Model
{

    [Table("ChatCol")]
    public class DbChatCollection
    {
        [Key, Column("UID", Order = 0)]
        //[ForeignKey ("NextCollection")]
        //[ForeignKey ("PreviousCollection")]
        public string ArchivingUserId { get; set; }
        [ForeignKey("ArchivingUserId ")]
        public virtual DbArchivingUser ArchivingUser { get; set; }

        /// <summary>
        /// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
        /// </summary>
        [Key, Column("Start", Order = 1)]
        public DateTime StartDate { get; set; }

        [Key, Column("WJid", Order = 2)]
        public string WithJid { get; set; }

        [Column(Order = 3)]
        public string Subject { get; set; }

        [Column(Order = 4)]
        public string Thread { get; set; }

        /// <summary>
        /// This is for Replication: If this collection is in the list of changed elements the client will
        /// notice, that it has saved an older version locally and can request the updated version.
        /// </summary>
        [Column(Order = 5)]
        public int Version { get; set; }

        ///// <summary>
        ///// This is for replication, to request all collections changed since a specific date.
        ///// </summary>
        //public DateTime LastChanged { get; set; }

        /// <summary>
        /// We do not delete DbChatCollection entries, because we need the deletion info for replication!
        /// Instead we set the IsDeleted flag for deletion (and delete all data/messages within this collection).
        /// </summary>
        [Column(Order = 6)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
        /// </summary>
        //[ForeignKey ("NextCollection")]
        [Column("NStart", Order = 7)]
        public DateTime? NextStartDate { get; set; }

        //[ForeignKey ("NextCollection")]
        [Column("NWJid", Order = 8)]
        public string NextWithJid { get; set; }

        public virtual DbChatCollection NextCollection { get; set; }

        /// <summary>
        /// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
        /// </summary>
        //[ForeignKey ("PreviousCollection")]
        [Column("PStart", Order = 9)]
        public DateTime? PreviousStartDate { get; set; }

        //[ForeignKey ("PreviousCollection")]
        [Column("PWJid", Order = 10)]
        public string PreviousWithJid { get; set; }
        public virtual DbChatCollection PreviousCollection { get; set; }

        /// <summary>
        /// See '5.7 Associating Attributes with a Collection'
        /// </summary>
        // MySql has no support for 'xml'
        //[Column ("XData", TypeName = "xml")]
        [Column(Order = 11)]
        public string XData { get; set; }


        public virtual ICollection<DbChatMessage> ChatMessages { get; set; }
    }
}
