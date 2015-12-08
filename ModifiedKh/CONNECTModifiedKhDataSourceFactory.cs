using Slb.Ocean.Core;
using Slb.Ocean.Petrel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModifiedKh
{
    class CONNECTModifiedKhDataSourceFactory: DataSourceFactory
    {
        
        public override Slb.Ocean.Core.IDataSource GetDataSource()
        {
            StructuredArchiveDataSource SADS = new StructuredArchiveDataSource(DataSourceId, new [] {typeof(SaveableArguments)});
            return SADS;

        }

        public static string DataSourceId = "ModifiedKh.CONNECTModifiedKhDataSourceFactory.DataSourceId";

        public static StructuredArchiveDataSource Get(IDataSourceManager DSM) 
        {
            return DSM.GetSource(DataSourceId) as StructuredArchiveDataSource;
        }
    }
}
