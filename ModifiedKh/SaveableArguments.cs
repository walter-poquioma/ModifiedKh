using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.Data.Persistence;
using Slb.Ocean.Petrel.Data;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Slb.Ocean.Geometry;

namespace ModifiedKh
{
 [ Archivable (Version= 1, FromRelease= "2014.1") ]
  public  class SaveableArguments: IIdentifiable
    {

     
     [Archived (FromVersion=1)]
     public Droid Droid
     {
         get;
       internal  set;

     }

        public SaveableArguments() 
        {
            sADS = CONNECTModifiedKhDataSourceFactory.Get(DataManager.DataSourceManager);

            if (sADS != null)
            {
                Droid = PermMatching.ArgsDroid;
                sADS.AddItem(Droid, this);

            }

        }

        private void SetDataSourceToDirty()
        {
            if (sADS != null)
            {
                sADS.IsDirty = true;
            }
            else
            {
                sADS = CONNECTModifiedKhDataSourceFactory.Get(DataManager.DataSourceManager);
                sADS.IsDirty = true;
            }
        
        }
        


        [ArchivableContextInject]
        private StructuredArchiveDataSource sADS ;

        private bool truncate2NormalDist = true;
        
        [Archived (FromVersion= 1)] 
        public bool Truncate2NormalDist
        {
            get     
            {
               return truncate2NormalDist;
            }

            set
            {
                truncate2NormalDist = value;
                SetDataSourceToDirty();
            }
        }

        private bool useOriginalData= false;
        [Archived(FromVersion = 1)]
        public bool UseOriginalData
        {
            get
            {
                return useOriginalData;
            }

            set
            {
                useOriginalData = value;
                SetDataSourceToDirty();
            }
        }


        private bool firstTimeEditingRatio = true;
        [Archived(FromVersion = 1)]
        public bool FirstTimeEditingRatio
        {
            get
            {
                return firstTimeEditingRatio;
            }

            set
            {
                firstTimeEditingRatio = value;
                SetDataSourceToDirty();
            }
        }

        
        private Droid permDroid;

        [Archived(FromVersion = 1)] 
        public Droid PermDroid
        {
            get 
            {
                return permDroid;
            }
            set 
            {
                permDroid = value;
                SetDataSourceToDirty();
            }
        }

        private Droid gridDroid;

        [Archived(FromVersion = 1)]
        public Droid GridDroid
        {
            get
            {
                return gridDroid;
            }
            set
            {
                gridDroid = value;
                SetDataSourceToDirty();
            }
        }

        private bool selectedWellsCheck = false;

        [Archived(FromVersion = 1)]
        public bool SelectedWellsCheck
        {
            get
            {
                return selectedWellsCheck;
            }

            set
            {
                selectedWellsCheck = value;
                SetDataSourceToDirty();
            }
        }

        private bool keepMissingAs1 = false;
      
        [Archived(FromVersion = 1)]
        public bool KeepMissingAs1
        {
            get
            {
                return keepMissingAs1;
            }

            set
            {
                keepMissingAs1 = value;
                SetDataSourceToDirty();
            }
        }
       
        private double maxRatio;

        [Archived(FromVersion = 1)]
        public double MaxRatio
        {
            get
            {
                return maxRatio;
            }

            set
            {
                maxRatio = value;
                SetDataSourceToDirty();
            }
        }

        private double minRatio;

        [Archived(FromVersion = 1)]
        public double MinRatio
        {
            get
            {
                return minRatio;
            }

            set
            {
                minRatio = value;
                SetDataSourceToDirty();
            }
        }

        private List<KhTableRowInfoContainer> listOfRowInfo = new List<KhTableRowInfoContainer>() ;

        [Archived(FromVersion = 1)]
        public List<KhTableRowInfoContainer> ListOfRowInfo
        {
            get
            {
                return listOfRowInfo;
            }

            set
            {
                listOfRowInfo = value;
                SetDataSourceToDirty();
            }
        }


        private List<KhTableRowInfoContainer> listOfOriginalData = new List<KhTableRowInfoContainer>();

        [Archived(FromVersion = 1)]
        public List<KhTableRowInfoContainer> ListOfOriginalData
        {
            get
            {
                return listOfOriginalData;
            }

            set
            {
                listOfOriginalData = value;
                SetDataSourceToDirty();
            }
        }

        private ObservableCollection<int> listOfKhwtIndices = new ObservableCollection<int>();

        [Archived(FromVersion = 1)]
        public ObservableCollection<int> ListOfKhwtIndices
        {
            get
            {
                return listOfKhwtIndices;
            }

            set
            {
                listOfKhwtIndices = value;
                SetDataSourceToDirty();
            }
        }

        private List<int> listOfOriginalKhwtIndices = new List<int>();

        [Archived(FromVersion = 1)]
        public List<int> ListOfOriginalKhwtIndices
        {
            get
            {
                return listOfOriginalKhwtIndices;
            }

            set
            {
                listOfOriginalKhwtIndices = value;
                SetDataSourceToDirty();
            }
        }
      
        private DataGridView modelDataGridView ;

        [Archived(FromVersion = 1)]
        public DataGridView ModelDataGridView
        {
            get
            {
                return modelDataGridView;
            }

            set
            {
                modelDataGridView = value;
                SetDataSourceToDirty();
            }
        }


        private double majorRange = 0.0;

        [Archived(FromVersion = 1)]
        public double MajorRange
        {
            get
            {
                return majorRange;
            }

            set
            {
                majorRange = value;
                SetDataSourceToDirty();
            }
        }

        private double minorRange = 0.0;

        [Archived(FromVersion = 1)]
        public double MinorRange
        {
            get
            {
                return minorRange;
            }

            set
            {
                minorRange = value;
                SetDataSourceToDirty();
            }
        }

        private double nugget;

        [Archived(FromVersion = 1)]
        public double Nugget
        {
            get
            {
                return nugget;
            }

            set
            {
                nugget = value;
                SetDataSourceToDirty();
            }
        }


        private Angle majorDirection;

        [Archived(FromVersion = 1)]
        public Angle MajorDirection
        {
            get
            {
                return majorDirection;
            }

            set
            {
                majorDirection = value;
                SetDataSourceToDirty();
            }
        }

        private int krigingAlgIndex = -1;

        [Archived(FromVersion = 1)]
        public int KrigingAlgIndex
        {
            get
            {
                return krigingAlgIndex;
            }

            set
            {
                krigingAlgIndex = value;
                SetDataSourceToDirty();
            }
        }


        private int variogramTypeIndex = -1;

        [Archived(FromVersion = 1)]
        public int VariogramTypeIndex
        {
            get
            {
                return variogramTypeIndex;
            }

            set
            {
                variogramTypeIndex = value;
                SetDataSourceToDirty();
            }
        }
       
       
    }


}
