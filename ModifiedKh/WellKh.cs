using System;
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
        }

        public WellKh(Borehole SelectedWell, Property SelectedPermeability, DictionaryProperty SelectedZoneIndex)
        {
            this.permeability = SelectedPermeability;
            this.well = SelectedWell;
            this.ZoneIndex = SelectedZoneIndex;
            this.root = ColorTableRoot.Get(PetrelProject.PrimaryProject);
            this.ListOfNamesOfIntersectedZones = new List<string>();
        }


        private Property permeability;
        private Borehole well;
        private DictionaryProperty zoneIndex;
        private ColorTableRoot root;
        private double top;
        private double bottom;
        private double khWellTesting;
        private List<string> listOfNamesOfIntersectedZones = new List<string>();
        

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
            set { this.root = value; }
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
            set { this.khWellTesting= value; }
        }

        [Description("listOfNamesOfIntersectedZones", "The list of names of the zones intersected by the well")]
        public List<string> ListOfNamesOfIntersectedZones
        {
            get { return this.listOfNamesOfIntersectedZones; }
            set { this.listOfNamesOfIntersectedZones = value; }
        }


        public bool GetListOfNamesOfIntersectedZones(bool PerforatedZonesOnly)
        {
            if (this.zoneIndex != null && this.permeability != null && this.well != null)
            {
                //Getting the indeces of the cells that are intersected by the selected borehole.
                List<Index3> ListOfIntersectedGridCells = KandaIntersectionService.GetTheGridCellsIntersectedByWell(this.permeability.Grid, this.well, PerforatedZonesOnly);

                //Getting the Zone Indeces corresponding to the cells that are intersected by the borehole.
                List<int> ListOfZoneIndexCorrespondingToIntersectedCells = KandaIntersectionService.GetThePropertyValueCorrespondingToTheCells(this.permeability,
                                                                                     ListOfIntersectedGridCells, this.zoneIndex);
               
                DictionaryColorTableAccess TableAccess = Root.GetDictionaryColorTableAccess(this.ZoneIndex);
                DictionaryColorTableEntry ColorTableEntry;

                foreach (int i in ListOfZoneIndexCorrespondingToIntersectedCells)
                {
                    if (i > -1)
                    {
                        ColorTableEntry = TableAccess.GetEntryAt(i);
                        this.ListOfNamesOfIntersectedZones.Add(ColorTableEntry.Name);
                        PetrelLogger.InfoOutputWindow(ColorTableEntry.Name);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
    
        }

    
    }

}
