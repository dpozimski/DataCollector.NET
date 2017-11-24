using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Device.Models
{
    /// <summary>
    /// Represents a point in 3D.
    /// </summary>
    public sealed class SpherePoint
    {
        #region Public Properties
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        /// <param name="z">z value</param>
        public SpherePoint(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        /// <summary>
        /// The default constructor.
        /// </summary>
        public SpherePoint()
        {

        }
        #endregion
    }
}
