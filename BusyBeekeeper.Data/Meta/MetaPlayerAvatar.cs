using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persited resource information about what makes an avatar.
    /// </summary>
    public class MetaPlayerAvatar
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this Avatar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the path of the texture used for this avatar.
        /// </summary>
        public string PortraitTexturePath { get; set; }

        public PlayerAvatar ToPlayerAvatar()
        {
            return new PlayerAvatar { ResourceId = this.Id };
        }
    }
}
