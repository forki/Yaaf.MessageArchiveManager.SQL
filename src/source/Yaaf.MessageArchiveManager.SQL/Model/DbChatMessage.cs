// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Model {
	public enum MessageType {
		// Not using the default value 0 to detect errors..
		To = 1,
		From = 2,
		Note = 3
	}
	public enum MessageContentType {
		// Not using the default value 0 to detect errors..
		Body = 1,
		Message = 2
	}

    public enum MessageState
    {
        /// <summary>
        /// Message was processed on the server.
        /// </summary>
        OnServer = 1,
        /// <summary>
        /// Message was delivered to the recipients server.
        /// </summary>
        Delivered_Server = 2,
        /// <summary>
        /// Message was delivered to one or more of the recipients devices.
        /// </summary>
        Delivered_Target = 3,
        /// <summary>
        /// Message war read by the recipient (optional).
        /// </summary>
        Read = 4
    }

	[Table ("ChMsg")]
	public class DbChatMessage {

		[Key, Column (Order = 0)]
		[DatabaseGenerated (DatabaseGeneratedOption.Identity)]
		public int MessageId { get; set; }
		
        [ForeignKey ("ChatCollection"), Column ("UID", Order = 1), Required]
		public string CollectionArchivingUserId { get; set; }
		/// <summary>
		/// Note that this DateTime, can only have seconds as smallest unit (see xep 0136 for details)
		/// </summary>
		[ForeignKey ("ChatCollection"), Column ("Start", Order = 2), Required]
		public DateTime CollectionStartDate { get; set; }

		[ForeignKey ("ChatCollection"), Column ("WJid", Order = 3), Required]
		public string CollectionWithJid { get; set; }


		public virtual DbChatCollection ChatCollection { get; set; }

		[NotMapped]
		public MessageType MessageType
		{ get { return (MessageType) (DbMessageType); } set { DbMessageType = (int) value; } }

        [DefaultValue(1)]
		public int DbMessageType { get; set; }

		public DateTime MessageDateTime { get; set; }

		[NotMapped]
		public MessageContentType ContentType
		{ get { return (MessageContentType) (DbContentType); } set { DbContentType = (int) value; } }

        [DefaultValue(1)]
		public int DbContentType { get; set; }

        public string Content { get; set; }

        [NotMapped]
        public MessageState MessageState
        { get { return (MessageState)(DbMessageState); } set { DbMessageState = (int)value; } }

        [DefaultValue(1)]
        public int DbMessageState { get; set; }
	}
}
