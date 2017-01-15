using System.Runtime.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CaringCookieClub
{
	[DataContract]
	public class GoogleInfo
	{
		[DataMember]
		public string id { get; set; }
		[DataMember]
		public string name { get; set; }
		[DataMember]
		public string given_name { get; set; }
		[DataMember]
		public string family_name { get; set; }
		[DataMember]
		public string link { get; set; }
		[DataMember]
		public string picture { get; set; }
		[DataMember]
		public string locale { get; set; }

	}
}
