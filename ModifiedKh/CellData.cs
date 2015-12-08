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
   public class CellData
    {
        public Index3 CellIndex;
        //public int ZoneIndex;
        public double Kh_wt;
        public double Perm;
        public double Height;
        public Borehole Well;
        public bool PerforatedZonesOnly;
    }
}
