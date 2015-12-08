using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Modeling;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.DomainObject.ColorTables;

namespace ModifiedKh
{
    class WellKh
    {
        public WellKh() 
        {
            this.ListOfNamesOfIntersectedZones = new List<string>();
            this.root = ColorTableRoot.Get(PetrelProject.PrimaryProject);
            this.KhWellTesting = -1;
        }
         
        public WellKh(Borehole SelectedWell, Property SelectedPermeability, double SelectedTop, double SelectedBottom ) 
        {
            this.permeability = SelectedPermeability;
            this.well = SelectedWell;
            this.top = SelectedTop;
            this.bottom = SelectedBottom;
            this.ZoneIndex = null;
            this.root = null;
            this.ListOfNamesOfIntersectedZones = null;
            this.KhWellTesting = -1;
        }

        public WellKh(Borehole SelectedWell, Property SelectedPermeability, DictionaryProperty SelectedZoneIndex)
        {
            this.permeability = SelectedPermeability;
            this.well = SelectedWell;
            this.ZoneIndex = SelectedZoneIndex;
            this.root = ColorTableRoot.Get(PetrelProject.PrimaryProject);
            this.ListOfNamesOfIntersectedZones = new List<string>();
            this.KhWellTesting = -1;
        }


        private Property permeability;
        private Borehole well;
        private DictionaryProperty zoneIndex;
        private ColorTableRoot root;
        private double top;
        private double bottom;
        private double khWellTesting;
        private List<string> listOfNamesOfIntersectedZones = new List<string>();
        private List<Index3> listOfSelectedGridCells = new List<Index3>();
        public List<int> listOfSelectedZoneIndeces = new List<int>();
        private List<Index3> listOfIntersectedGridCells = new List<Index3>();
        private List<int> listOfZoneIndexCorrespondingToIntersectedCells = new List<int>();
        //private Dictionary<Index3, double> DictionaryOfCellData = new Dictionary<Index3, double>();
       // private Dictionary<Index3, double> DictionaryOfSelectedCells = new Dictionary<Index3, double>();

        public static double FactorToConvert_mdft_To_m3 = (9.869233E-16) * (0.3048);
        public static double FactorToConvert_mdm_To_m3 = 9.869233E-16;
        

        [Description("permeability", "The permeability given by the user")]
        public Property Permeability
        {
            get { return this.permeability; }
            set { this.permeability = value; }
        }


        [Description("well", "The selected well")]
        public Borehole Well
        {
             get { return this.well; }
            set { this.well = value; }
        }

        [Description("zoneIndex", "The zoneIndex given by the user")]
        public DictionaryProperty ZoneIndex
        {
            get { return this.zoneIndex; }
            set { this.zoneIndex = value; }
        }

        [Description("root", "The color table root of the project")]
        public ColorTableRoot Root
        {
            get { return this.root; }
            internal set { this.root = value; }
        }

        [Description("top", "Depth (ft) corresponding to the top of the section of KhWellTesting")]
        public double Top
        {
            get { return this.top; }
            set { this.top = value; }
        }

        [Description("bottom", "Depth (ft) corresponding to the bottom of the section of KhWellTesting")]
        public double Bottom
        {
            get { return this.bottom; }
            set { this.bottom = value; }
        }

        [Description("khWellTesting", "The well testing Kh given by the user")]
        public double KhWellTesting
        {
            get { return this.khWellTesting; }
            set { this.khWellTesting = value; }
        }

        [Description("listOfNamesOfIntersectedZones", "The list of names of the zones intersected by the well")]
        public List<string> ListOfNamesOfIntersectedZones
        {
            get { return this.listOfNamesOfIntersectedZones; }
           internal set { this.listOfNamesOfIntersectedZones = value; }
        }

        //Method used to set the ListOfNamesOfIntersectedZones property. This is used to see which zones are intersected. 
        public bool SetListOfNamesOfIntersectedZones(bool PerforatedZonesOnly)
        {
            this.ListOfNamesOfIntersectedZones.Clear();
            if (this.zoneIndex != null && this.permeability != null && this.well != null)
            {
                //Getting the indeces of the cells that are intersected by the selected borehole.If PerforatedZonesOnly is set to true then it gets only
                //those cells which are perforated.
                listOfIntersectedGridCells = KandaIntersectionService.GetTheGridCellsIntersectedByWell(this.permeability.Grid, this.well, PerforatedZonesOnly);

                //Getting the Zone Indeces corresponding to the cells that are intersected by the borehole.
                listOfZoneIndexCorrespondingToIntersectedCells = KandaIntersectionService.GetThePropertyValueCorrespondingToTheCells(this.permeability,
                                                                                     listOfIntersectedGridCells, this.zoneIndex);
                //listOfZoneIndexCorrespondingToIntersectedCells = KandaIntersectionService.GetThePropertyValueCorrespondingToTheCells(
                //                                                                    listOfIntersectedGridCells, this.zoneIndex);
                DictionaryColorTableEntry ColorTableEntry;
               
                DictionaryColorTableAccess TableAccess = Root.GetDictionaryColorTableAccess(this.ZoneIndex);
                

                foreach (int i in listOfZoneIndexCorrespondingToIntersectedCells)
                {
                    if (i > -1)
                    {
                       ColorTableEntry = TableAccess.GetEntryAt(i);
                        this.ListOfNamesOfIntersectedZones.Add(ColorTableEntry.Name);
                        //PetrelLogger.InfoOutputWindow(ColorTableEntry.Name);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
    
        }


        //Method used to check wether there is any in-between Zones in the ListOfSelectedZoneNames that is not listed. If that is the case then it returns
        //false so that the user knows that some zones are missing. It also returns false in other cases such as when the ZoneIndex property has not been set
        //or when one of the names in the ListOfSelectedNames does not match any zone in the Zonde Index table.
        public bool VerticalContinuity(List<string> ListOfSelectedZoneNames)
        {   if(this.ZoneIndex == null) {MessageBox.Show("Please define a zone index property"); return false;}
            DictionaryColorTableAccess TableAccess = Root.GetDictionaryColorTableAccess(this.ZoneIndex);
            DictionaryColorTableEntry ColorTableEntry;

            if (this.listOfSelectedZoneIndeces != null)
            {
                this.listOfSelectedZoneIndeces.Clear();
            }
            else 
            {
                this.listOfSelectedZoneIndeces = new List<int>();
            }
            int count = 0;
            //Getting the zone indices corresponding to the selected zone names
            foreach(string NameOfZone in ListOfSelectedZoneNames)
            {
                count = count + 1;
                //Looping through all the names of the zones
                for (int i = 0; i < TableAccess.Size; i++) 
                {   ColorTableEntry = TableAccess.GetEntryAt(i);

                    //Checking if one the names matches the selected zone name
                    if(NameOfZone.Equals(ColorTableEntry.Name)) 
                    {
                        this.listOfSelectedZoneIndeces.Add(i);
                        break;
                    }
                    
                }
                if (this.listOfSelectedZoneIndeces.Count != count)
                {
                    MessageBox.Show(NameOfZone +
                        " zone does not correspond to any item in the Zone Index"); return false;
                }
               

            }

            //Sorting the indices in ascending order
            this.listOfSelectedZoneIndeces.Sort();

            //Checking if the indices follow a consecutive order (e.g 1 2 3 4 5 and not 1 3 4 5). If they are not consecutive then there is a gap
            // between the selected zones and this is not accepted by the program. It returns a false.
            for (int i = 0; i < this.listOfSelectedZoneIndeces.Count -1 ; i++)
            {
                if (this.listOfSelectedZoneIndeces[i] + 1 != this.listOfSelectedZoneIndeces[i+1] )
                {
                   // this.listOfSelectedZoneIndeces = null;
                    return false;
                    
                }
                
            }

            //If everything has proceeded as expected then return true
            return true;
        }

        //Method to get all the grid cells that are either in the zones selected by the user (if Depth_or_Zones is set to false) or between the two depth values 
        //(Top and Bottom) given by the user (if Depth_or_Zones is set to true). The information of the grid cells is contained in a CellData object which has to be 
        //associated to a grid (as there is no grid property in a CellData object).
        public Dictionary<int, List<CellData>> GetKhDictionaryOfSelectedGridCells(bool Depth_or_Zones, bool PerforatedZonesOnly, bool Vertical_only)
        {  //True for Depth
           //False for Zones

            if (this.permeability != null && this.well != null && this.listOfIntersectedGridCells != null && this.listOfZoneIndexCorrespondingToIntersectedCells != null)
            {
                if (Depth_or_Zones)
                {
                    Dictionary<int, List<CellData>> DictionaryOfCellData = new Dictionary<int, List<CellData>>();
                    return DictionaryOfCellData;
                }

                else
                {
                    //Dictionary<int, List<CellData>> DictionaryOfCellData = new Dictionary<int, List<CellData>>(this.listOfSelectedZoneIndeces.Count);
                    Dictionary<int, List<CellData>> DictionaryOfCellData = new Dictionary<int, List<CellData>>();
                    List<double> Heights = KandaIntersectionService.GetListOfPenetratedCellDistances(this.permeability.Grid, this.well, this.listOfIntersectedGridCells,
                                                                                                      PerforatedZonesOnly, Vertical_only);


                    foreach (int index in this.listOfSelectedZoneIndeces)
                    {
                        for (int i = 0; i < this.listOfIntersectedGridCells.Count; i++)
                        {
                            if (this.listOfZoneIndexCorrespondingToIntersectedCells[i] == index)
                            {
                                if (!DictionaryOfCellData.ContainsKey(index)) { DictionaryOfCellData.Add(index, new List<CellData>()); }
                                CellData CellDataObj = new CellData();
                                CellDataObj.CellIndex = this.listOfIntersectedGridCells[i];
                                CellDataObj.Perm = this.permeability[listOfIntersectedGridCells[i]];
                                CellDataObj.Height = Heights[i];
                                CellDataObj.Kh_wt = this.khWellTesting;
                                CellDataObj.Well = this.Well;
                                CellDataObj.PerforatedZonesOnly = PerforatedZonesOnly;
                                
                                if (CellDataObj.Height >= 0 && !Double.IsNaN(CellDataObj.Perm))
                                {
                                    DictionaryOfCellData[index].Add(CellDataObj);   
                                }
                                

                                //  this.listOfSelectedGridCells.Add(this.listOfIntersectedGridCells[i]);
                            }

                        }

                    }
                    return DictionaryOfCellData;
                }
            }
            else 
            {
                return null;
            }
            
        }

        //Returns a dictionary containing the Index3 of the selected cell as the key of the dictionary. It also returns the "height" property
        //corresponding to that cell as the first element of the list corresponding to the given key. The second property is the permeability value and the
        //third property corresponds to the ratio Kh_wt/Kh_average. 
        //public bool SetKhDictionaryValuesOfSelectedCells(bool Depth_orZones)
        //{ 
            
        //}


    
    }

}
