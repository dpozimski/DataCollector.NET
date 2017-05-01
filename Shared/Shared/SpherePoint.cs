using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Device.Models
{
    /// <summary>
    /// Klasa reprezentująca Punkt w przestrzeni.
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
        /// Konstruktor klasy Point3D.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public SpherePoint(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        /// <summary>
        /// Domyślny konstruktor klasy Point3D.
        /// </summary>
        public SpherePoint()
        {

        }
        #endregion
    }
}
