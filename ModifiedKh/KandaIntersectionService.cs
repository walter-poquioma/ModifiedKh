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

            return ListOfIntersectingGridCells;
        }
        
        private static void AddingWellGridIntersectionCellIndices(IPillarGridIntersectionService pgiservice, Grid gridInContext, IPolyline3 pline, List<Index3> ListOfIntersectingGridCells)
        {            
            IEnumerable<SegmentCellIntersection> intersectionSegments;
            intersectionSegments = pgiservice.GetPillarGridPolylineIntersections(gridInContext, pline);
            foreach (SegmentCellIntersection sci in intersectionSegments)
            {
                Index3 indx3 = sci.EnteringCell;
                if (indx3 != null)
                {
                    ListOfIntersectingGridCells.Add(indx3);
                }
            }
        }
  
        private static List<IPolyline3> CreatingPolyLineListWhenWellHasPerforations(Borehole bh)
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
         public static List<int> GetThePropertyValueCorrespondingToTheCells(Property CheckProperty, List<Index3> CellIndeces, DictionaryProperty Property)
        {
               NoBoundaryCheckDictionaryPropertyIndexer MyDictionaryPropertyIndexer = Property.SpecializedAccess.OpenNoBoundaryCheckDictionaryPropertyIndexer();
               NoBoundaryCheckPropertyIndexer CheckPropertyIndexer = CheckProperty.SpecializedAccess.OpenNoBoundaryCheckPropertyIndexer();
               List<int> ListOfPropertieValuesCorrespondingToCellIndices  = new List<int>();

               foreach (Index3 cell in CellIndeces) 
               {
                   if (!double.IsNaN(CheckPropertyIndexer[cell])) //checking if it is Nan or not
                   {
                       ListOfPropertieValuesCorrespondingToCellIndices.Add(MyDictionaryPropertyIndexer[cell]);

                   }
                   else 
                   {
                       ListOfPropertieValuesCorrespondingToCellIndices.Add(-1);
                       PetrelLogger.InfoOutputWindow("One of the intersected cells has an undefined permeability property");
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
                    PetrelLogger.InfoOutputWindow("One of the intersected cells has an undefined permeability property");
                }
            }
            MyPropertyIndexer.Close();
            MyPropertyIndexer.Dispose();
            return ListOfPropertieValuesCorrespondingToCellIndices;
        }

        //Recursive Loop to get all the low level zones
        //public static List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> GetAllLowLevelZones(IEnumerable<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> TopZones)
        //{
        //    List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfLowestLevelZones = new  List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone>(); 
        //    foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in TopZones) 
        //    {
        //        if (zone.ZoneCount > 0)
        //        {
        //            ListOfLowestLevelZones.AddRange(GetAllLowLevelZones(zone.Zones));
        //        }
        //        else 
        //        {
        //            ListOfLowestLevelZones.Add(zone);
        //        }

        //    }
        //    return ListOfLowestLevelZones;
        //}

        //public static Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone GetZone(Index3 cellIndex, List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> Zones)//List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> TopZone)
        //{
        //    foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in Zones) 
        //    {
        //        if (cellIndex.K > zone.BaseK && cellIndex.K)
        //       {
               
        //       }
        //    }
 
        //}
    }
}
