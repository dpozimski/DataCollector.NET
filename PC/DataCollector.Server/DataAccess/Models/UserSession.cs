using DataCollector.Server.DataAccess.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Class defines an user session.
    /// </summary>
    /// <CreatedOn>01.11.2017 15:53</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    [DataContract]
    public class UserSession
    {
        /// <summary>
        /// Gets or sets the session usser.
        /// </summary>
        /// <value>
        /// The session usser.
        /// </value>
        /// <CreatedOn>01.11.2017 15:53</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        [DataMember]
        public User SessionUser { get; set; }
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        /// <CreatedOn>01.11.2017 15:53</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        [DataMember]
        public int SessionId { get; set; }
    }
}
