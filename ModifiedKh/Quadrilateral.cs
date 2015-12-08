using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Geometry;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Modeling;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.DomainObject.ColorTables;

namespace ModifiedKh
{
    public class Quadrilateral//: INotifyPropertyChanged
    {
        public Quadrilateral(Point3 TopLeftIn, Point3 TopRightIn, Point3 BottomLeftIn, Point3 BottomRightIn)
        {
            this.TopLeft = TopLeftIn;
            this.TopRight = TopRightIn;
            this.BottomLeft = BottomLeftIn;
            this.BottomRight = BottomRightIn;

            Plane3 plane = new Plane3(this.TopLeft, this.TopRight, this.BottomLeft);
            //Vector3 ab = new Vector3(new Segment3(this.BottomLeft, this.TopLeft));
            //Vector3 ac = new Vector3(new Segment3(this.BottomLeft, this.BottomRight));
            //Vector3 normal = Vector3.Cross(ab, ac);
            //Vector3 ad = new Vector3(new Segment3(this.BottomLeft, this.TopRight));
            //double dotp = Vector3.Dot(ad, normal);

            //PetrelLogger.InfoOutputWindow("The dot product of this cell's face is " + System.Convert.ToString(dotp));

            if (Plane3Extensions.Contains(plane,this.BottomRight, 1E-7))
            {
                LeftSegment = new Segment3(this.TopLeft, this.BottomLeft);
                RightSegment = new Segment3(this.TopRight, this.BottomRight);
                BottomSegment = new Segment3(this.BottomLeft, this.BottomRight);
                TopSegment = new Segment3(this.TopLeft, this.TopRight);
                

                CalculateCentroid();
            }
            else 
            {   double x = (this.TopLeft.X + this.BottomLeft.X + this.TopRight.X + this.BottomRight.X) / 4.0;
                double y = (this.TopLeft.Y + this.BottomLeft.Y + this.TopRight.Y + this.BottomRight.Y) / 4.0;
                double z = (this.TopLeft.Z + this.BottomLeft.Z + this.TopRight.Z + this.BottomRight.Z) / 4.0;

                this.Centroid = new Point3(x,y,z);
            }

          }

        public Point3 TopLeft;
        public Point3 TopRight;
        public Point3 BottomLeft;
        public Point3 BottomRight;

        // public event PropertyChangedEventHandler PropertyChanged;

        private Segment3 leftSegment;
        private Segment3 rightSegment;
        private Segment3 topSegment;
        private Segment3 bottomSegment;

        private Point3 centroid;

        public Segment3 LeftSegment
        {
            get { return this.leftSegment; }
            internal set { this.leftSegment = value; }
        }

        public Segment3 RightSegment
        {
            get { return this.rightSegment; }
            internal set { this.rightSegment = value; }
        }

        public Segment3 BottomSegment
        {
            get { return this.bottomSegment; }
            internal set { this.bottomSegment = value; }
        }

        public Segment3 TopSegment
        {
            get { return this.topSegment; }
            internal set { this.topSegment = value; }
        }

        public Point3 Centroid
        {
            get { return this.centroid; }
            internal set { this.centroid = value; }
        }

        public void CalculateCentroid()
        {
            Segment3 LeftRightBimedian = new Segment3(this.LeftSegment.MidPoint, this.RightSegment.MidPoint);
            Segment3 TopBottomBimedian = new Segment3(this.BottomSegment.MidPoint, this.TopSegment.MidPoint);

            this.Centroid = LeftRightBimedian.Intersect(TopBottomBimedian,1E-4);
        }

    }
}
