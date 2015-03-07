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

namespace Yaaf.Xmpp.MessageArchiveManager.Sql.Model {

	[Table ("ItemPref")]
	public class DbUserItemPreference : SavingModeEntity {

		[Key, Column (Order = 0)]
		public string ArchivingUserId { get; set; }
		[ForeignKey ("ArchivingUserId ")]
		public virtual DbArchivingUser ArchivingUser { get; set; }

		[Key, Column (Order = 1)]
		public string Jid { get; set; } 
	}
}
