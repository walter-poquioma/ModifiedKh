using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;



using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Basics;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Modeling;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.PropertyModeling;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.Data.Persistence;
using Slb.Ocean.Petrel.Data;

namespace ModifiedKh
{
   [Archivable(Version = 1, FromRelease = "2014.1")]
   public class KhTableRowInfoContainer : INotifyPropertyChanged
    {

     //[Archived (FromVersion=1)]
     //public Droid Droid
     //{
     //    get;
     //  internal  set;

     //}

     //private void SetDataSourceToDirty()
     //{
     //    if (sADS != null)
     //    {
     //        sADS.IsDirty = true;
     //    }
     //    else
     //    {
     //        sADS = CONNECTModifiedKhDataSourceFactory.Get(DataManager.DataSourceManager);
     //        sADS.IsDirty = true;
     //   }

     //}
    
     //[ArchivableContextInject]
     //private StructuredArchiveDataSource sADS;
       public KhTableRowInfoContainer()
       { 
       
       }

        public KhTableRowInfoContainer(Dictionary<int, List<CellData>> DictOfCellData, int ZoneIndexKey, Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone ZoneIn)
        {
            ZoneName = ZoneIn.Name;
            ZoneIndex = ZoneIndexKey;
            WellName = DictOfCellData[DictOfCellData.Keys.First()][0].Well.Name;
            this.PropertyChanged += HandlePropertyChanged;
            Global = false;
            include = true;

            Kh_wt = -1.0;
            
            this.SetListOfCellsAndAvgIJK(DictOfCellData);
            this.SetKh_sim(DictOfCellData);

            //sADS = CONNECTModifiedKhDataSourceFactory.Get(DataManager.DataSourceManager);

            //if (sADS != null)
            //{
            //   // Droid = ModifiedKh.ArgsDroid;
            //    sADS.AddItem(Droid, this);

            //}

       

        }

        public KhTableRowInfoContainer CreateCopy()
        {
            KhTableRowInfoContainer ri = new KhTableRowInfoContainer();
            ri.kh_sim = this.kh_sim;
            ri.kh_wt = this.kh_wt;

            try 
	       {
               foreach (Index3 ind in this.listOfCellInd)
               {
                   ri.listOfCellInd.Add(new Index3(ind.I,ind.J,ind.K));
               }
	       }
	       catch 
	       {
	       }

            ri.avgIJK = this.avgIJK;
            ri.wellName = this.wellName;
            ri.zoneName = this.zoneName;
            ri.zoneIndex = this.zoneIndex;
            ri.ratio = this.ratio;
            ri.global = this.global;
            ri.include = this.include;
            return ri;

        }

        //Used to set the ListOfCellIndices and AvgIJK properties
        private void SetListOfCellsAndAvgIJK(Dictionary<int, List<CellData>> DictOfCellData)
        {
            int avgI = 0;
            int avgJ= 0;

            ListOfCellInd = new List<Index3>();

            foreach (CellData CD in DictOfCellData[ZoneIndex]) 
            {
                ListOfCellInd.Add(CD.CellIndex);
                avgI = CD.CellIndex.I + avgI;
                avgJ = CD.CellIndex.J + avgJ;
            }

            AvgIJK = new Index3(avgI / ListOfCellInd.Count, avgJ / ListOfCellInd.Count, ZoneIndex);

        }

        //Used to set the Kh_sim Property
        private void SetKh_sim(Dictionary<int, List<CellData>> DictOfCellData)
        {   Kh_sim = 0.0; 

            foreach (CellData CD in DictOfCellData[ZoneIndex])
            {
                Kh_sim = Kh_sim + CD.Perm * CD.Height;
            }

        } 

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Kh_wt")
            {
                // update Ratio here
                if (kh_wt > 0)
                    this.ratio = Kh_wt / Kh_sim;
                else
                    this.ratio = 1.0;

            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //private Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone;
        private string zoneName; 
        private int zoneIndex;
        private string wellName;
        private double kh_sim;
        private double kh_wt;
        private double ratio;
        private List<Index3> listOfCellInd = new List<Index3>();
        private Index3 avgIJK;
        private bool global;
        private bool include; 

        [Archived(FromVersion = 1)]
        public Index3 AvgIJK
        {
            get { return avgIJK; }
            set
            {
                avgIJK = value;
                //SetDataSourceToDirty();
            }
        }

        [Archived(FromVersion = 1)]
        public bool Global
        {
            get { return global; }
            set
            {
                global = value;
                //SetDataSourceToDirty();
            }
        }

        [Archived(FromVersion = 1)]
        public bool Include
        {
            get { return include; }
            set
            {
                include = value;
                //SetDataSourceToDirty();
            }
        }

        [Archived(FromVersion = 1)]
        public List<Index3> ListOfCellInd
        {
            get { return listOfCellInd; }
            set
            {
                listOfCellInd = value;
               // SetDataSourceToDirty();
            }
        }

        [Archived(FromVersion = 1)]
        public double Kh_sim
        {
            get { return kh_sim; }
            set
            {
                kh_sim = value;
               // SetDataSourceToDirty();
            }
        }

        [Archived(FromVersion = 1)] 
        public string WellName
        {
            get { return wellName; }
            set
            {
                wellName = value;
               // SetDataSourceToDirty();
            }
        }

       [Archived(FromVersion = 1)] 
        public int ZoneIndex
        {
            get { return zoneIndex; }
            set 
            {
                zoneIndex = value;
               // SetDataSourceToDirty();
            }

        }

        [Archived(FromVersion = 1)] 
        public string ZoneName
        {
            get { return zoneName; }
            set 
            {
                zoneName = value;
              // SetDataSourceToDirty();
            }
        }

       [Archived(FromVersion = 1)] 
        public double Kh_wt
        {
            get { return kh_wt; }
            set
            {
                    this.kh_wt = value; //* WellKh.FactorToConvert_mdft_To_m3;
                    RaisePropertyChanged("Kh_wt");
                  //  SetDataSourceToDirty();
                // Call OnPropertyChanged whenever the property is updated    
            }
        }

        [Archived(FromVersion = 1)] 
        public double Ratio
        {
            get { return ratio; }
            set
            {
                    this.ratio = value;
                    RaisePropertyChanged("Ratio");
                  //  SetDataSourceToDirty();
                // Call OnPropertyChanged whenever the property is updated
            }
        }



    }
}
