using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Slb.Ocean.Core;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.Workflow.ProcessGroups;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.ColorTables;
using Slb.Ocean.Petrel.UI;

namespace ModifiedKh
{
    static class KandaPropertyCreator
    {
        
        public static void Create(double[] arrayOfProperty, Template templateOfPropery,string nameOfProperty, Grid gridInContext)
        {

            try
            {
                if (arrayOfProperty == null || templateOfPropery==null || nameOfProperty==null || gridInContext==null)
                {
                    throw new NullReferenceException("One of the arguments supplied is null");
                }


                PropertyCollection pc = gridInContext.PropertyCollection;

                using (ITransaction trans = DataManager.NewTransaction())
                {
                    trans.Lock(pc);
                    Property p = pc.CreateProperty(templateOfPropery);
                    p.Name = nameOfProperty;

                    int max_i = gridInContext.NumCellsIJK.I;
                    int max_j = gridInContext.NumCellsIJK.J;
                    int max_k = gridInContext.NumCellsIJK.K;

                    if (arrayOfProperty.Length!=max_i*max_j*max_k)
                    {
                        throw new IndexOutOfRangeException("The property array and grid do not match..");
                    }

                    for (int i = 0; i < max_i; i++)
                    {
                        for (int j = 0; j < max_j; j++)
                        {
                            for (int k = 0; k < max_k; k++)
                            {
                                p[i, j, k] = (float)arrayOfProperty[i + j * max_i + k * max_i * max_j];
                            }
                        }
                    }

                    trans.Commit();
                }

                
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("The argument is null..");
                Console.WriteLine("{0}  The argument is null", ex);
                throw;
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("The property array and grid do not match");
                Console.WriteLine("{0}  The property array and grid do not match", ex);
                throw;
            }
            
        }
        
        public static void Create(List<double[]> listOfArrayOfProperty, Template templateOfPropery, string prefixNameOfProperties, Grid gridInContext)
        {
            try
            {
                if (listOfArrayOfProperty == null || templateOfPropery==null || prefixNameOfProperties==null || gridInContext==null)
                {
                    throw new NullReferenceException("One of the arguments supplied is null");
                }

                int suffix = 1;
                foreach (double[] arrayproperty in listOfArrayOfProperty)
                {
                    Create(arrayproperty, templateOfPropery, prefixNameOfProperties + suffix.ToString(), gridInContext);
                    suffix = suffix + 1;
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("The argument is null..");
                Console.WriteLine("{0}  The argument is null", ex);
                throw;                
            }

            
        }

        public static DictionaryProperty CreateZoneIndex(Grid grid)
        {
            List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfZones = KandaPropertyCreator.GetAllLowLevelZones(grid.Zones);
            PropertyCollection pc = grid.PropertyCollection;
            Index3 CellIndex = new Index3();

             using (ITransaction trans = DataManager.NewTransaction())
             {
                 trans.Lock(pc);

                 DictionaryTemplate TemplateOfProperty = PetrelProject.WellKnownTemplates.FaciesGroup.ZonesMain;
                 DictionaryProperty ZoneIndex = pc.CreateDictionaryProperty(TemplateOfProperty);
                 ColorTableRoot Root = ColorTableRoot.Get(PetrelProject.PrimaryProject);
                 DictionaryColorTableAccess TableAccess = Root.GetDictionaryColorTableAccess(ZoneIndex);
                 Slb.Ocean.Petrel.DomainObject.ColorTables.DictionaryColorTableEntry ColorTableEntry = new Slb.Ocean.Petrel.DomainObject.ColorTables.DictionaryColorTableEntry();

              

                 int max_i = grid.NumCellsIJK.I;
                 int max_j = grid.NumCellsIJK.J;
                 int max_k = grid.NumCellsIJK.K;
                 int begin; int end;
                  
                 int code = 0;
                  
                 foreach(Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in ListOfZones)
                 {
                     // code = ZoneIndex.AddFaciesCode(zone.Name);
                     ColorTableEntry.CodeValue = code;
                     ColorTableEntry.Name = zone.Name;
                        using (ITransaction trans2 = DataManager.NewTransaction())
                       {
                           trans2.Lock(TableAccess);

                           TableAccess.InsertEntry(ColorTableEntry);

                           trans2.Commit();
                       }

                        if (zone.BaseK > zone.TopK) { begin = zone.TopK; end = zone.BaseK; }
                        else { begin = zone.BaseK; end = zone.TopK; }

                       for (int k = begin; k <= end; k++)
                       {
 
                           for (int i = 0; i < max_i; i++)
                           {
                               for (int j = 0; j < max_j; j++)
                               {   CellIndex.I = i; CellIndex.J = j;  CellIndex.K = k;
                                   
                                   if(grid.IsCellDefined(CellIndex))
                                   {
                                       
                                       ZoneIndex[CellIndex] = code;
                                   }
                               }
                           }
                       }
                       code = code + 1;
                 }
                 
                 trans.Commit();
                 return ZoneIndex;
             }

             
    
        }

        //Recursive Loop to get all the low level zones
        public static List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> GetAllLowLevelZones(IEnumerable<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> TopZones)
        {
            List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfLowestLevelZones = new List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone>();
            foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in TopZones)
            {
                if (zone.ZoneCount > 0)
                {
                    ListOfLowestLevelZones.AddRange(GetAllLowLevelZones(zone.Zones));
                }
                else
                {
                    ListOfLowestLevelZones.Add(zone);
                }

            }
            return ListOfLowestLevelZones;
        }



    }

   
}
