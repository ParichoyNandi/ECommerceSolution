using Entities;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace ECommManagement
{
    public class ProductManagement: IProductManagement
    {
        private string _conn;
        private IDBAccess _data;

        public ProductManagement(IConfiguration config, IDBAccess data)
        {
            _conn = config.GetConnectionString("DevConn");
            _data = data;
        }

        public void SaveProductMappings(int ProductID, string ExamCategoryIDList, List<ProductCenterMap> centerMaps, string CreatedBy)
        {
            DataHelper dh = new(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                _data.SaveProductExamCategoriesMappings(ProductID, ExamCategoryIDList, CreatedBy, trans, dh);

                foreach (var c in centerMaps)
                {
                    _data.SaveProductCenterFeePlanMappings(ProductID, c.CenterID, c.FeePlans[0].CourseFeePlanID,
                                                            c.FeePlans[0].ValidFrom, c.FeePlans[0].ValidTo, CreatedBy, trans, dh);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }
        }

        public void SaveProductExamCategoryMappings(int ProductID, string ExamCategoryIDList, string CreatedBy)
        {
            DataHelper dh = new(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                _data.SaveProductExamCategoriesMappings(ProductID, ExamCategoryIDList,CreatedBy,trans,dh);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }
        }

        public void SaveProductCenterFeePlanMappings(int ProductID, List<ProductCenterMap> centerMaps, string CreatedBy)
        {
            DataHelper dh = new(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                foreach(var c in centerMaps)
                {
                    _data.SaveProductCenterFeePlanMappings(ProductID, c.CenterID, c.FeePlans[0].CourseFeePlanID,
                                                            c.FeePlans[0].ValidFrom, c.FeePlans[0].ValidTo, CreatedBy, trans, dh);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }
        }
    }
}
