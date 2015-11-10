using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.Well.Completion;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject.ColorTables;

namespace ModifiedKh
{
    static class KandaIntersectionService
    {
        public static List<Index3> GetTheGridCellsIntersectedByWell(Grid gridInContext, Borehole bh, bool PerforatedZonesOnly)
        {


            IPillarGridIntersectionService pgiservice = CoreSystem.GetService<IPillarGridIntersectionService>();
            List<Index3> ListOfIntersectingGridCells = new List<Index3>();

            if (PerforatedZonesOnly)
            {
                if (bh.Completions.PerforationCount <= 0)
                {
                    IPolyline3 pline = bh.Trajectory.Polyline;
                    AddingWellGridIntersectionCellIndices(pgiservice, gridInContext, pline, ListOfIntersectingGridCells);
                }
                else
                {
                    List<IPolyline3> plineList = CreatingPolyLineListWhenWellHasPerforations(bh);
                    foreach (IPolyline3 pline in plineList)
                    {
                        AddingWellGridIntersectionCellIndices(pgiservice, gridInContext, pline, ListOfIntersectingGridCells);
                    }
                }
            }
            else 
            {
                IPolyline3 pline = bh.Trajectory.Polyline;
                AddingWellGridIntersectionCellIndices(pgiservice, gridInContext, pline, ListOfIntersectingGridCells);
            }

            return ListOfIntersectingGridCells.Distinct().ToList();
        }
        
        private static void AddingWellGridIntersectionCellIndices(IPillarGridIntersectionService pgiservice, Grid gridInContext, IPolyline3 pline, List<Index3> ListOfIntersectingGridCells)
        {            
            IEnumerable<SegmentCellIntersection> intersectionSegments;
            intersectionSegments = pgiservice.GetPillarGridPolylineIntersections(gridInContext, pline);

            if (intersectionSegments != null)
            {
                foreach (SegmentCellIntersection sci in intersectionSegments)
                {
                    Index3 indx3 = sci.EnteringCell;
                    if (indx3 != null)
                    {
                        ListOfIntersectingGridCells.Add(indx3);
                    }
                    else
                    {
                        indx3 = sci.LeavingCell;
                        ListOfIntersectingGridCells.Add(indx3);
                    }
                }
            }
            else
            {         IEnumerator<Point3> Point3Enumerator = pline.GetEnumerator();
               
                while (Point3Enumerator.MoveNext())
                {
                    Index3 indx3 = gridInContext.GetCellAtPoint(Point3Enumerator.Current);
                    break;
                }
                
            }
        }
  
        public static List<IPolyline3> CreatingPolyLineListWhenWellHasPerforations(Borehole bh)
        {
            List<IPolyline3> plineList = new List<IPolyline3>();

            foreach (Perforation prf in bh.Completions.Perforations)
            {
                double x1 = bh.Transform(Domain.MD, prf.StartMD, Domain.X);
                double y1 = bh.Transform(Domain.MD, prf.StartMD, Domain.Y);
                double z1 = bh.Transform(Domain.MD, prf.StartMD, Domain.ELEVATION_DEPTH);

                List<Point3> ptsList = new List<Point3>();
                Point3 pt3_1 = new Point3(x1, y1, z1);
                ptsList.Add(pt3_1);

                double x2 = bh.Transform(Domain.MD, prf.EndMD, Domain.X);
                double y2 = bh.Transform(Domain.MD, prf.EndMD, Domain.Y);
                double z2 = bh.Transform(Domain.MD, prf.EndMD, Domain.ELEVATION_DEPTH);

                Point3 pt3_2 = new Point3(x2, y2, z2);
                

                double xmiddle = (x1 + x2)/2;
                double ymiddle = (y1 + y2) / 2;
                double zmiddle = (z1 + z2) / 2;

                Point3 pt3_3 = new Point3(xmiddle, ymiddle, zmiddle);
                ptsList.Add(pt3_3);
                ptsList.Add(pt3_2);

                IPolyline3 pline = new Polyline3(ptsList);

                plineList.Add(pline);
            }

            return plineList;

        }


        //Method that returns a list of integers containing the dictionary property values corresponding to a list of cell indices. 
        //PLEASE NOTE: a PROPERTY is used to check (CheckProperty) if the cell should be used for processing or not. For example: If I am interested in
        // cells that have a defined value for porosity then my CheckProperty should be Porosity. If a value for porosity is not defined then 
        // the program returns a -1 for the DICTIONARY PROPERTY value entry (Zone Index for example) corresponding to that cell index.
        // public static List<int> GetThePropertyValueCorrespondingToTheCells(Property CheckProperty, List<Index3> CellIndeces, DictionaryProperty Property)
        //{
        //       NoBoundaryCheckDictionaryPropertyIndexer MyDictionaryPropertyIndexer = Property.SpecializedAccess.OpenNoBoundaryCheckDictionaryPropertyIndexer();
        //       NoBoundaryCheckPropertyIndexer CheckPropertyIndexer = CheckProperty.SpecializedAccess.OpenNoBoundaryCheckPropertyIndexer();
        //       List<int> ListOfPropertieValuesCorrespondingToCellIndices  = new List<int>();

        //       foreach (Index3 cell in CellIndeces) 
        //       {
        //           if (!double.IsNaN(CheckPropertyIndexer[cell])) //checking if it is Nan or not
        //           {
        //               ListOfPropertieValuesCorrespondingToCellIndices.Add(MyDictionaryPropertyIndexer[cell]);

        //           }
        //           else 
        //           {
        //               ListOfPropertieValuesCorrespondingToCellIndices.Add(-1);
        //               PetrelLogger.InfoOutputWindow("One of the intersected cells has an undefined Check property");
        //           }

        //       }
        //       MyDictionaryPropertyIndexer.Close();
        //       MyDictionaryPropertyIndexer.Dispose();
        //       CheckPropertyIndexer.Close();
        //       CheckPropertyIndexer.Dispose(); 
        //       return ListOfPropertieValuesCorrespondingToCellIndices;

               
        //}

        public static List<int> GetThePropertyValueCorrespondingToTheCells(Property CheckProperty, List<Index3> CellIndeces, DictionaryProperty Property)
        {
            NoBoundaryCheckDictionaryPropertyIndexer MyDictionaryPropertyIndexer = Property.SpecializedAccess.OpenNoBoundaryCheckDictionaryPropertyIndexer();
           NoBoundaryCheckPropertyIndexer CheckPropertyIndexer = CheckProperty.SpecializedAccess.OpenNoBoundaryCheckPropertyIndexer();
            List<int> ListOfPropertieValuesCorrespondingToCellIndices = new List<int>();

            foreach (Index3 cell in CellIndeces)
            {
                if (!double.IsNaN(CheckPropertyIndexer[cell])) //checking if it is Nan or not
                {
                    ListOfPropertieValuesCorrespondingToCellIndices.Add(MyDictionaryPropertyIndexer[cell]);

                }
                else
                {
                    ListOfPropertieValuesCorrespondingToCellIndices.Add(-1);
                    PetrelLogger.InfoOutputWindow("One of the intersected cells has an undefined Check property");
                }

            }
            MyDictionaryPropertyIndexer.Close();
            MyDictionaryPropertyIndexer.Dispose();
            CheckPropertyIndexer.Close();
            CheckPropertyIndexer.Dispose();
            return ListOfPropertieValuesCorrespondingToCellIndices;


        }
         //Method that returns a list of doubles containing the property values corresponding to a list of cell indices. 
         //PLEASE NOTE: The PROPERTY is used to check if the cell should be used for processing or not. For example: If I am interested in
         // cells that have a defined value for porosity then the program checks if porosity is defined in that cell. If a value for porosity 
         // is not defined then the program returns a  Nan corresponding to that cell index.
          
        public static List<double> GetThePropertyValueCorrespondingToTheCells(Property Property, List<Index3> CellIndeces)
        {
            NoBoundaryCheckPropertyIndexer MyPropertyIndexer = Property.SpecializedAccess.OpenNoBoundaryCheckPropertyIndexer();
            List<double> ListOfPropertieValuesCorrespondingToCellIndices  = new List<double>();

            foreach (Index3 cell in CellIndeces)
            {
                if (!double.IsNaN(MyPropertyIndexer[cell])) //checking if it is Nan or not
                {
                    ListOfPropertieValuesCorrespondingToCellIndices.Add(MyPropertyIndexer[cell]);

                }
                else
                {
                    ListOfPropertieValuesCorrespondingToCellIndices.Add(0.0/0.0);
                    PetrelLogger.InfoOutputWindow("One of the intersected cells has an undefined Check property");
                }
            }
            MyPropertyIndexer.Close();
            MyPropertyIndexer.Dispose();
            return ListOfPropertieValuesCorrespondingToCellIndices;
        }

        //Method that finds the distance from Face1 to Face2 of a cell passing through the center if both faces are directly opposite
        //public static double Plane2Plane_AvgDist(Plane3 Face1, Plane3 Face2, Point3 center)
        //{
        //     double dist;
        //     dist = Face1.AbsoluteDistance(center);
        //     dist = Face2.AbsoluteDistance(center) + dist;
        //     return dist;
        //}

        //Method to get all the CellSides from which the well entered the selected grid cells. The selected grid cells must intersect the well in order for the method to work.
        public static List<CellSide> GetListOfEnteringSidesOfIntersectedCells(IPillarGridIntersectionService pgiservice, Grid gridInContext, Borehole bh, List<Index3> ListOfSelectedIntersectingGridCells, bool PerforatedZonesOnly)
        {
            bool indx3_LeavingCell;
            List<CellSide> ListOfEnteringSides = new List<CellSide>();

            if (PerforatedZonesOnly) //If the user wants only perforated zones
            {
                if (bh.Completions.PerforationCount <= 0)
                {
                    IPolyline3 pline = bh.Trajectory.Polyline;

                    IEnumerable<SegmentCellIntersection> intersectionSegments;
                    intersectionSegments = pgiservice.GetPillarGridPolylineIntersections(gridInContext, pline);
                    foreach (SegmentCellIntersection sci in intersectionSegments)
                    {
                        Index3 indx3 = sci.EnteringCell;
                        if (indx3 != null)
                        {
                            indx3_LeavingCell = false;
                        }
                        else 
                        {
                            indx3 = sci.LeavingCell;
                            indx3_LeavingCell = true;
                        }

                        foreach (Index3 cell in ListOfSelectedIntersectingGridCells)
                        {
                            if (cell == indx3)
                            {
                                if (!indx3_LeavingCell)
                                {
                                    ListOfEnteringSides.Add(sci.EnteringCellSide);
                                }
                                else
                                {
                                    ListOfEnteringSides.Add(sci.LeavingCellSide);
                                }
                            }
                        }
                    }

                }
                else
                {  
                    List<IPolyline3> plineList = CreatingPolyLineListWhenWellHasPerforations(bh);

                    foreach (IPolyline3 poly in plineList)
                    {
                        IEnumerable<SegmentCellIntersection> intersectionSegments;
                        intersectionSegments = pgiservice.GetPillarGridPolylineIntersections(gridInContext, poly);

                        foreach (SegmentCellIntersection sci in intersectionSegments)
                        {
                            Index3 indx3 = sci.EnteringCell;
                            if (indx3 != null)
                            {
                                indx3_LeavingCell = false;
                            }
                            else
                            {
                                indx3 = sci.LeavingCell;
                                indx3_LeavingCell = true;
                            }

                            foreach (Index3 cell in ListOfSelectedIntersectingGridCells)
                            {
                                if (cell == indx3)
                                {
                                    if (!indx3_LeavingCell)
                                    {
                                        ListOfEnteringSides.Add(sci.EnteringCellSide);
                                    }
                                    else
                                    {
                                        ListOfEnteringSides.Add(sci.LeavingCellSide);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else 
            {
                IPolyline3 pline = bh.Trajectory.Polyline;

                IEnumerable<SegmentCellIntersection> intersectionSegments;
                intersectionSegments = pgiservice.GetPillarGridPolylineIntersections(gridInContext, pline);
                foreach (SegmentCellIntersection sci in intersectionSegments)
                {
                    Index3 indx3 = sci.EnteringCell;
                    if (indx3 != null)
                    {
                        indx3_LeavingCell = false;
                    }
                    else
                    {
                        indx3 = sci.LeavingCell;
                        indx3_LeavingCell = true;
                    }

                    foreach (Index3 cell in ListOfSelectedIntersectingGridCells)
                    {
                        if (cell == indx3)
                        {
                            if (!indx3_LeavingCell)
                            {
                                ListOfEnteringSides.Add(sci.EnteringCellSide);
                            }
                            else
                            {
                                ListOfEnteringSides.Add(sci.LeavingCellSide);
                            }
                        }
                    }
                }
 
            }
            if (ListOfEnteringSides.Count != ListOfSelectedIntersectingGridCells.Count) 
            { MessageBox.Show("One or more of the cells are not intersected by the borehole");
            return ListOfEnteringSides = null;
            }
            return ListOfEnteringSides;


          }


        public static Point3 GetIntersectingPoint(IPillarGridIntersectionService pgiservice, Grid GridInContext,Borehole bh, Index3 CellIndex, bool PerforatedZonesOnly)
        {
            Point3 IntersectionPoint;

            if (PerforatedZonesOnly) //if the user wants only perforated zones
            {
                if (bh.Completions.PerforationCount <= 0)
                {
                    IPolyline3 pline = bh.Trajectory.Polyline;

                    IEnumerable<SegmentCellIntersection> intersectionsegments;
                    intersectionsegments = pgiservice.GetPillarGridPolylineIntersections(GridInContext, pline);
                    foreach (SegmentCellIntersection sci in intersectionsegments)
                    {
                        if (CellIndex == sci.EnteringCell || CellIndex == sci.LeavingCell)
                        {
                            IntersectionPoint = sci.IntersectionPoint;
                            return IntersectionPoint;
                        }
                      
                    }

                }
                else
                {
                    List<IPolyline3> plinelist = CreatingPolyLineListWhenWellHasPerforations(bh);

                    foreach (IPolyline3 poly in plinelist)
                    {
                        IEnumerable<SegmentCellIntersection> intersectionsegments;
                        intersectionsegments = pgiservice.GetPillarGridPolylineIntersections(GridInContext, poly);

                        foreach (SegmentCellIntersection sci in intersectionsegments)
                        {
                            if (CellIndex == sci.EnteringCell || CellIndex == sci.LeavingCell)
                            {
                                IntersectionPoint = sci.IntersectionPoint;
                                return IntersectionPoint;
                            }
                        }
                    }

                }
            }
            else
            {
                IPolyline3 pline = bh.Trajectory.Polyline;

                IEnumerable<SegmentCellIntersection> IntersectionSegments;
                IntersectionSegments = pgiservice.GetPillarGridPolylineIntersections(GridInContext, pline);
                foreach (SegmentCellIntersection sci in IntersectionSegments)
                {
                    if (CellIndex == sci.EnteringCell || CellIndex == sci.LeavingCell)
                    {
                        IntersectionPoint = sci.IntersectionPoint;
                        return IntersectionPoint;
                    }
                }

            }


            return null;
        }



        public static List<double> GetListOfPenetratedCellDistances(Grid gridInContext, Borehole bh, List<Index3> ListOfSelectedIntersectingGridCells, bool PerforatedZonesOnly, bool Vertical_only) 
        {
            IPillarGridIntersectionService pgiservice = CoreSystem.GetService<IPillarGridIntersectionService>();
            Quadrilateral Face1;
            Quadrilateral Face2;
            CellCorner[] CellCorners = new CellCorner[4];
            Point3[] CellCornerPoints1 = new Point3[4];
            Point3[] CellCornerPoints2 = new Point3[4];
            CellSide Side = new CellSide() ;
            //Dictionary<Index3,List<double>> DictionaryOfSelectedCells = new Dictionary<Index3,List<double>>(ListOfSelectedIntersectingGridCells.Count);
           // List<double> Distance = new List<double>(3); //This array will contain the Height of the cell for the kh calculation and two extra elements that will be empty;
            List<double> Distance = new List<double>();

            if (!Vertical_only)
            {
                List<CellSide> ListOfEnteringSides = GetListOfEnteringSidesOfIntersectedCells(pgiservice, gridInContext, bh, ListOfSelectedIntersectingGridCells, PerforatedZonesOnly);
                for (int i = 0; i < ListOfEnteringSides.Count; i++)
                {
                    switch (ListOfEnteringSides[i])
                    {
                     case CellSide.Up:
                        CellCornerPoints1 = KandaIntersectionService.GetCornerSet(ListOfEnteringSides[i], gridInContext, ListOfSelectedIntersectingGridCells[i]);
                          

                        Side = CellSide.Down;
                        CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                            break;

                     case CellSide.East:
                        CellCornerPoints1 = KandaIntersectionService.GetCornerSet(ListOfEnteringSides[i], gridInContext, ListOfSelectedIntersectingGridCells[i]);
                          

                        Side = CellSide.West;
                        CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                            break;

                      case CellSide.West:
                           CellCornerPoints1 = KandaIntersectionService.GetCornerSet(ListOfEnteringSides[i], gridInContext, ListOfSelectedIntersectingGridCells[i]);
                          

                        Side = CellSide.East;
                        CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                            break;

                      case CellSide.South:
                           CellCornerPoints1 = KandaIntersectionService.GetCornerSet(ListOfEnteringSides[i], gridInContext, ListOfSelectedIntersectingGridCells[i]);
                          

                            Side = CellSide.North;
                            CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                            break;

                      case CellSide.North:
                           CellCornerPoints1 = KandaIntersectionService.GetCornerSet(ListOfEnteringSides[i], gridInContext, ListOfSelectedIntersectingGridCells[i]);
                          

                        Side = CellSide.South;
                        CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                            break;

                        case CellSide.Down:
                            CellCornerPoints1 = KandaIntersectionService.GetCornerSet(ListOfEnteringSides[i], gridInContext, ListOfSelectedIntersectingGridCells[i]);
                          

                        Side = CellSide.Up;
                        CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);
 
                            break;
                        default:

                            CellCornerPoints1 = null;
                            CellCornerPoints2= null;
                            break;
                    }
                   

                    try
                    {
                        Face1 = new Quadrilateral(CellCornerPoints1[0], CellCornerPoints1[1], CellCornerPoints1[2], CellCornerPoints1[3]);
                    }
                    catch
                    {
                        Face1 = null;
                    }

                    try
                    {
                        Face2 = new Quadrilateral(CellCornerPoints2[0], CellCornerPoints2[1], CellCornerPoints2[2], CellCornerPoints2[3]);
                    }
                    catch
                    {
                        Face2 = null;
                    }

                    try
                    {
                        // Distance[0] = Face1.Centroid.Distance(Face2.Centroid);
                        //DictionaryOfSelectedCells.Add(ListOfSelectedIntersectingGridCells[i], Distance);
                        Distance.Add(Face1.Centroid.Distance(Face2.Centroid));
                    }
                    catch
                    {
                        Distance.Add(-1);
                        //DictionaryOfSelectedCells.Add(ListOfSelectedIntersectingGridCells[i], null);
                    }

                }
            }
            else //If only the vertical distance from top to base face of cell is required.
            {
                for (int i = 0; i < ListOfSelectedIntersectingGridCells.Count; i++)
                {
                    //CellCorners[0] = CellCorner.TopNorthWest; CellCorners[1] = CellCorner.TopNorthEast; CellCorners[2] = CellCorner.TopSouthWest;
                    //CellCorners[3] = CellCorner.TopSouthEast;
                    //CellCornerPoints = gridInContext.GetCellCorners(ListOfSelectedIntersectingGridCells[i], CellCorners);
                    Side = CellSide.Up;
                    CellCornerPoints1 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                    Side = CellSide.Down;
                    CellCornerPoints2 = KandaIntersectionService.GetCornerSet(Side, gridInContext, ListOfSelectedIntersectingGridCells[i]);

                    try
                    {
                        Face1 = new Quadrilateral(CellCornerPoints1[0], CellCornerPoints1[1], CellCornerPoints1[2], CellCornerPoints1[3]);
                    }
                    catch
                    {
                        Face1 = null;
                    }

                    try
                    {
                        Face2 = new Quadrilateral(CellCornerPoints2[0], CellCornerPoints2[1], CellCornerPoints2[2], CellCornerPoints2[3]);
                    }
                    catch
                    {
                        Face2 = null;
                    }


                    try
                    {
                        // Distance[0] = Face1.Centroid.Distance(Face2.Centroid);
                        //DictionaryOfSelectedCells.Add(ListOfSelectedIntersectingGridCells[i], Distance);
                        Distance.Add(Face1.Centroid.Distance(Face2.Centroid));
                    }
                    catch
                    {
                        Distance.Add(-1);
                        //DictionaryOfSelectedCells.Add(ListOfSelectedIntersectingGridCells[i], null);
                    }
                  
                }
            }

            

            return Distance;
            //return DictionaryOfSelectedCells;

        }



       

        //public static Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone GetZone(Index3 cellIndex, List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> Zones)//List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> TopZone)
        //{
        //    foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in Zones) 
        //    {
        //        if (cellIndex.K > zone.BaseK && cellIndex.K)
        //       {
               
        //       }
        //    }
 
        //}

        public static Point3[] GetCornerSet(CellSide SideOfCell, Grid gridInContext, Index3 CellIndex)
        {
            CellCorner[] CellCorners = new CellCorner[4];
            Point3[] CellCornerPoints = new Point3[4];
            
            switch (SideOfCell)
            {
                case CellSide.Up:
                    CellCorners[0] = CellCorner.TopNorthWest; CellCorners[1] = CellCorner.TopNorthEast; CellCorners[2] = CellCorner.TopSouthWest;
                    CellCorners[3] = CellCorner.TopSouthEast;
                    CellCornerPoints = gridInContext.GetCellCorners(CellIndex, CellCorners);

                    break;

                case CellSide.East:
                    CellCorners[0] = CellCorner.TopSouthEast; CellCorners[1] = CellCorner.TopNorthEast; CellCorners[2] = CellCorner.BaseSouthEast;
                    CellCorners[3] = CellCorner.BaseNorthEast;
                    CellCornerPoints = gridInContext.GetCellCorners(CellIndex, CellCorners);

                    break;

                case CellSide.West:

                    CellCorners[0] = CellCorner.TopSouthWest; CellCorners[1] = CellCorner.TopNorthWest; CellCorners[2] = CellCorner.BaseSouthWest;
                    CellCorners[3] = CellCorner.BaseNorthWest;
                    CellCornerPoints = gridInContext.GetCellCorners(CellIndex, CellCorners);


                    break;

                case CellSide.South:
                    CellCorners[0] = CellCorner.TopSouthWest; CellCorners[1] = CellCorner.TopSouthEast; CellCorners[2] = CellCorner.BaseSouthWest;
                    CellCorners[3] = CellCorner.BaseSouthEast;
                    CellCornerPoints = gridInContext.GetCellCorners(CellIndex, CellCorners);
              
                    break;

                case CellSide.North:

                    CellCorners[0] = CellCorner.TopNorthWest; CellCorners[1] = CellCorner.TopNorthEast; CellCorners[2] = CellCorner.BaseNorthWest;
                    CellCorners[3] = CellCorner.BaseNorthEast;
                    CellCornerPoints = gridInContext.GetCellCorners(CellIndex, CellCorners);

                    break;

                case CellSide.Down:
                  
                    CellCorners[0] = CellCorner.BaseNorthWest; CellCorners[1] = CellCorner.BaseNorthEast; CellCorners[2] = CellCorner.BaseSouthWest;
                    CellCorners[3] = CellCorner.BaseSouthEast;
                    CellCornerPoints = gridInContext.GetCellCorners(CellIndex, CellCorners);

                    break;
                default:

                    CellCornerPoints = null;
                    break;
            }

            return CellCornerPoints;
        }

    }

   
}
