using Entities;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utilities;

namespace DataAccess
{
    public class DBAccess: IDBAccess
    {
        private string _conn;

        public DBAccess(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DevConn");
        }

        public int InsertStudentBatchInQueue(int StudentDetailID, int BatchID, string EntryType, SqlTransaction trans, DataHelper dh)
        {
            int i = 0;


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@StudentDetailID", StudentDetailID);
                sqlparams[1] = new SqlParameter("@BatchID", BatchID);
                sqlparams[2] = new SqlParameter("@EntryType", EntryType);

                i = dh.ExecuteNonQuery("LMS.uspInsertStudentBatchDetailsForInterface", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return i;
        }

        public List<Category> GetCategories()
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Category> categories = new List<Category>();


            try
            {
                ds = dh.ExecuteDataSet("SELECT * FROM ECOMMERCE.T_Category_Master", CommandType.Text);

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Category category = new Category();
                        category.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        category.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);

                        category.CategoryCode = ds.Tables[0].Rows[i]["CategoryCode"].ToString();
                        category.CategoryName = ds.Tables[0].Rows[i]["CategoryName"].ToString();
                        category.CategoryDesc = ds.Tables[0].Rows[i]["CategoryDesc"].ToString();

                        category.StatusID = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusID"]);
                        category.IsOnline = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsOnline"]);
                        category.IsOffline = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsOffline"]);

                        categories.Add(category);
                    }
                }
            }
            finally
            {
                if(dh!=null)
                {
                    if(dh.DataConn!=null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return categories;
        }

        public FAQ GetFAQForPlan(int PlanID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            FAQ fq = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@PlanID", PlanID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetFAQForPlan", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        fq.FAQID = Convert.ToInt32(ds.Tables[0].Rows[0]["FAQHeaderID"]);
                        fq.Name = ds.Tables[0].Rows[0]["FAQName"].ToString();

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            FAQDetail detail = new();

                            if (Convert.ToInt32(ds.Tables[1].Rows[i]["FAQHeaderID"]) == fq.FAQID)
                            {
                                detail.FAQDetailID = Convert.ToInt32(ds.Tables[1].Rows[i]["FAQdetailID"]);
                                detail.Question = ds.Tables[1].Rows[i]["Question"].ToString();
                                detail.Answer = ds.Tables[1].Rows[i]["Answer"].ToString();

                                fq.FAQDetails.Add(detail);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return fq;

        }

        public List<Product> GetProductList(int BrandID, int CategoryID = 0, string ExamGroups=null, int PlanID=0)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            List<Product> products = new List<Product>();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                if (CategoryID == 0)
                    sqlparams[1] = new SqlParameter("@CategoryID", DBNull.Value);
                else
                    sqlparams[1] = new SqlParameter("@CategoryID", CategoryID);

                if (ExamGroups == null || ExamGroups.Trim()=="")
                    sqlparams[2] = new SqlParameter("@sExamGroupList", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@sExamGroupList", ExamGroups);

                sqlparams[3] = new SqlParameter("@PlanID", PlanID);


                ds = dh.ExecuteDataSet("ECommerce.uspGetProductList", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Product product = new();

                        product.ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]);
                        product.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        //product.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                        product.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]);


                        product.ProductCode = ds.Tables[0].Rows[i]["ProductCode"].ToString();
                        product.ProductName = ds.Tables[0].Rows[i]["ProductName"].ToString();
                        product.ShortDesc = ds.Tables[0].Rows[i]["ProductShortDesc"].ToString();
                        product.LongDesc = ds.Tables[0].Rows[i]["ProductLongDesc"].ToString();
                        product.ProductImage = ds.Tables[0].Rows[i]["ProductImage"].ToString();
                        product.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);
                        product.CategoryDetails.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                        product.CategoryDetails.CategoryName = ds.Tables[0].Rows[i]["CategoryName"].ToString();

                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {
                            product.ProductLangaugeID = 0;
                            product.ProductLanguageName = null;
                        }
                        else
                        {
                            product.ProductLangaugeID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                            product.ProductLanguageName = ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }


                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (product.ProductID == Convert.ToInt32(ds.Tables[1].Rows[j]["ProductID"]))
                            {
                                ProductConfig config = new();

                                config.ConfigID = Convert.ToInt32(ds.Tables[1].Rows[j]["ConfigID"]);
                                config.ConfigCode = ds.Tables[1].Rows[j]["ConfigCode"].ToString();
                                config.ConfigValue= ds.Tables[1].Rows[j]["ConfigValue"].ToString();
                                config.ConfigDisplayName= ds.Tables[1].Rows[j]["ConfigName"].ToString();

                                product.ProductConfigList.Add(config);
                            }
                        }

                        products.Add(product);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return products;
        }

        public List<Product> GetProducts(int BrandID,int CenterID=0, int CategoryID=0, bool IsPublished=true)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            List<Product> products = new List<Product>();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                if (CenterID == 0)
                    sqlparams[1] = new SqlParameter("@CenterID", DBNull.Value);
                else
                    sqlparams[1] = new SqlParameter("@CenterID", CenterID);

                if (CategoryID == 0)
                    sqlparams[2] = new SqlParameter("@CategoryID", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@CategoryID", CategoryID);

                sqlparams[3] = new SqlParameter("@IsPublished", IsPublished);


                ds = dh.ExecuteDataSet("ECommerce.uspGetProductDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Product product = new();

                        product.ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]);
                        product.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        //product.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                        product.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]);


                        product.ProductCode = ds.Tables[0].Rows[i]["ProductCode"].ToString();
                        product.ProductName = ds.Tables[0].Rows[i]["ProductName"].ToString();
                        product.ShortDesc = ds.Tables[0].Rows[i]["ProductShortDesc"].ToString();
                        product.LongDesc = ds.Tables[0].Rows[i]["ProductLongDesc"].ToString();
                        product.ProductImage = ds.Tables[0].Rows[i]["ProductImage"].ToString();
                        product.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);


                        //susmita
                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {

                            product.ProductLangaugeID = 0;
                            product.ProductLanguageName = null;

                        }
                        else
                        {
                            product.ProductLangaugeID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                            product.ProductLanguageName = ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }

                        //susmita


                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (product.ProductID == Convert.ToInt32(ds.Tables[1].Rows[j]["ProductID"]))
                            {
                                ProductCenterMap centerMap = new();

                                centerMap.ProductCenterID = Convert.ToInt32(ds.Tables[1].Rows[j]["ProductCentreID"]);
                                centerMap.CenterID = Convert.ToInt32(ds.Tables[1].Rows[j]["CenterID"]);

                                centerMap.CenterName = ds.Tables[1].Rows[j]["S_Center_Name"].ToString();

                                for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                                {
                                    if (centerMap.ProductCenterID == Convert.ToInt32(ds.Tables[2].Rows[k]["ProductCentreID"]))
                                    {
                                        ProductFeePlan feePlan = new();

                                        feePlan.ProductFeePlanID = Convert.ToInt32(ds.Tables[2].Rows[k]["ProductFeePlanID"]);
                                        feePlan.CourseFeePlanID = Convert.ToInt32(ds.Tables[2].Rows[k]["CourseFeePlanID"]);
                                        feePlan.NumberOfInstalments = Convert.ToInt32(ds.Tables[2].Rows[k]["N_No_Of_Installments"]);
                                        feePlan.FeePlanAmount_Lumpsum = Convert.ToDecimal(ds.Tables[2].Rows[k]["N_TotalLumpSum"]);
                                        feePlan.FeePlanAmount_Instalment = Convert.ToDecimal(ds.Tables[2].Rows[k]["N_TotalInstallment"]);

                                        feePlan.FeePlan = ds.Tables[2].Rows[k]["S_Fee_Plan_Name"].ToString();
                                        feePlan.FeePlanDisplayName = ds.Tables[2].Rows[k]["ProductFeePlanDisplayName"].ToString();

                                        centerMap.FeePlans.Add(feePlan);
                                    }
                                }

                                product.CenterAvailability.Add(centerMap);
                            }
                        }

                        products.Add(product);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return products;
        }

        public List<Product> GetProductsForPublishing(int BrandID, int CenterID = 0, int CategoryID = 0)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            List<Product> products = new List<Product>();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                if (CenterID == 0)
                    sqlparams[1] = new SqlParameter("@CenterID", DBNull.Value);
                else
                    sqlparams[1] = new SqlParameter("@CenterID", CenterID);

                if (CategoryID == 0)
                    sqlparams[2] = new SqlParameter("@CategoryID", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@CategoryID", CategoryID);


                ds = dh.ExecuteDataSet("ECommerce.uspGetProductDetailsForPublishing", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Product product = new();

                        product.ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]);
                        product.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        //product.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                        product.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]);


                        product.ProductCode = ds.Tables[0].Rows[i]["ProductCode"].ToString();
                        product.ProductName = ds.Tables[0].Rows[i]["ProductName"].ToString();
                        product.ShortDesc = ds.Tables[0].Rows[i]["ProductShortDesc"].ToString();
                        product.LongDesc = ds.Tables[0].Rows[i]["ProductLongDesc"].ToString();
                        product.ProductImage = ds.Tables[0].Rows[i]["ProductImage"].ToString();
                        product.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);

                        //susmita
                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {
                            product.ProductLangaugeID = 0;
                            product.ProductLanguageName = null;
                        }
                        else
                        { 
                        product.ProductLangaugeID= Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                        product.ProductLanguageName= ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }
                        //susmita


                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (product.ProductID == Convert.ToInt32(ds.Tables[1].Rows[j]["ProductID"]))
                            {
                                ProductCenterMap centerMap = new();

                                centerMap.ProductCenterID = Convert.ToInt32(ds.Tables[1].Rows[j]["ProductCentreID"]);
                                centerMap.CenterID = Convert.ToInt32(ds.Tables[1].Rows[j]["CenterID"]);

                                centerMap.CenterName = ds.Tables[1].Rows[j]["S_Center_Name"].ToString();

                                for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                                {
                                    if (centerMap.ProductCenterID == Convert.ToInt32(ds.Tables[2].Rows[k]["ProductCentreID"]))
                                    {
                                        ProductFeePlan feePlan = new();

                                        feePlan.ProductFeePlanID = Convert.ToInt32(ds.Tables[2].Rows[k]["ProductFeePlanID"]);
                                        feePlan.CourseFeePlanID = Convert.ToInt32(ds.Tables[2].Rows[k]["CourseFeePlanID"]);
                                        feePlan.NumberOfInstalments = Convert.ToInt32(ds.Tables[2].Rows[k]["N_No_Of_Installments"]);
                                        feePlan.FeePlanAmount_Lumpsum = Convert.ToDecimal(ds.Tables[2].Rows[k]["N_TotalLumpSum"]);
                                        feePlan.FeePlanAmount_Instalment = Convert.ToDecimal(ds.Tables[2].Rows[k]["N_TotalInstallment"]);

                                        feePlan.FeePlan = ds.Tables[2].Rows[k]["S_Fee_Plan_Name"].ToString();
                                        feePlan.FeePlanDisplayName = ds.Tables[2].Rows[k]["ProductFeePlanDisplayName"].ToString();

                                        centerMap.FeePlans.Add(feePlan);
                                    }
                                }

                                product.CenterAvailability.Add(centerMap);
                            }
                        }

                        products.Add(product);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return products;
        }

        public List<Plan> GetPlanList(string BrandIDList,int CategoryID, string ExamCategoryIDs=null,int CouponID=0, string ExamGroupIDs = null, int ProductID=0, string CustomerID = null)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Plan> plans = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[7];
                sqlparams[0] = new SqlParameter("@sBrandID", BrandIDList);
                sqlparams[1] = new SqlParameter("@ExamCategoryIDs", ExamCategoryIDs);
                sqlparams[2] = new SqlParameter("@CategoryID", CategoryID);
                sqlparams[3] = new SqlParameter("@CouponID", CouponID);
                sqlparams[4] = new SqlParameter("@ExamGroupIDs", ExamGroupIDs);
                sqlparams[5] = new SqlParameter("@ProductID", ProductID);
                sqlparams[6] = new SqlParameter("@CustomerID", CustomerID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPlanList", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Plan plan = new();

                        plan.PlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["PlanID"]);
                        //plan.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);

                        plan.PlanCode = ds.Tables[0].Rows[i]["PlanCode"].ToString();
                        plan.PlanName = ds.Tables[0].Rows[i]["PlanName"].ToString();
                        plan.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);

                        //susmita : added
                        if ((ds.Tables[0].Rows[i]["I_Language_ID"]) != DBNull.Value)
                            plan.PlanLanguageID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                        else
                            plan.PlanLanguageID = 0;

                        if ((ds.Tables[0].Rows[i]["I_Language_Name"]) != DBNull.Value)
                            plan.PlanLanguageName = ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        else
                            plan.PlanLanguageName = null;

                       

                        //susmita :added


                        if (ds.Tables.Count > 1)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                PlanConfig config = new();

                                if (Convert.ToInt32(ds.Tables[1].Rows[j]["PlanID"]) == plan.PlanID)
                                {
                                    config.ConfigID = Convert.ToInt32(ds.Tables[1].Rows[j]["ConfigID"]);
                                    config.PlanConfigID = Convert.ToInt32(ds.Tables[1].Rows[j]["PlanConfigID"]);
                                    config.ConfigCode = ds.Tables[1].Rows[j]["ConfigCode"].ToString();
                                    config.DisplayName = ds.Tables[1].Rows[j]["DisplayName"].ToString();
                                    config.ConfigValue = ds.Tables[1].Rows[j]["ConfigValue"].ToString();

                                    plan.ConfigList.Add(config);
                                }
                            }
                        }

                        if(ds.Tables.Count>2)
                        {
                            for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                            {
                                if (Convert.ToInt32(ds.Tables[2].Rows[k]["PlanID"]) == plan.PlanID)
                                {
                                    Brand brand = new();

                                    brand.BrandID = Convert.ToInt32(ds.Tables[2].Rows[k]["I_Brand_ID"]);
                                    brand.BrandName = ds.Tables[2].Rows[k]["S_Brand_Name"].ToString();

                                    plan.BrandDetails.Add(brand);
                                }
                            }
                        }

                        if(ds.Tables.Count>3)
                        {
                            for(int l=0;l<ds.Tables[3].Rows.Count;l++)
                            {
                                if (Convert.ToInt32(ds.Tables[3].Rows[l]["PlanID"]) == plan.PlanID)
                                {
                                    plan.CategoryIDList.Add(Convert.ToInt32(ds.Tables[3].Rows[l]["CategoryID"]));
                                }
                            }
                        }

                        plans.Add(plan);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return plans;
        }

        public List<Plan> GetPlanListForCouponMapping(string BrandIDList, int CategoryID, string ExamCategoryIDs = null, int CouponID = 0)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Plan> plans = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@sBrandID", BrandIDList);
                sqlparams[1] = new SqlParameter("@ExamCategoryIDs", ExamCategoryIDs);
                sqlparams[2] = new SqlParameter("@CategoryID", CategoryID);
                sqlparams[3] = new SqlParameter("@CouponID", CouponID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPlanListForCouponMapping", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Plan plan = new();

                        plan.PlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["PlanID"]);
                        //plan.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);

                        plan.PlanCode = ds.Tables[0].Rows[i]["PlanCode"].ToString();
                        plan.PlanName = ds.Tables[0].Rows[i]["PlanName"].ToString();
                        plan.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);
                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {

                            plan.PlanLanguageID = 0;
                            plan.PlanLanguageName = null;

                        }
                        else
                        { 
                            plan.PlanLanguageID= Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                            plan.PlanLanguageName = ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }


                        //if (ds.Tables.Count > 1)
                        //{
                        //    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        //    {
                        //        PlanConfig config = new();

                        //if (Convert.ToInt32(ds.Tables[1].Rows[j]["PlanID"]) == plan.PlanID)
                        //{
                        //    config.ConfigID = Convert.ToInt32(ds.Tables[1].Rows[j]["ConfigID"]);
                        //    config.PlanConfigID = Convert.ToInt32(ds.Tables[1].Rows[j]["PlanConfigID"]);
                        //    config.ConfigCode = ds.Tables[1].Rows[j]["ConfigCode"].ToString();
                        //    config.DisplayName = ds.Tables[1].Rows[j]["DisplayName"].ToString();
                        //    config.ConfigValue = ds.Tables[1].Rows[j]["ConfigValue"].ToString();

                        //    plan.ConfigList.Add(config);
                        //}
                        //    }
                        //}

                        //if (ds.Tables.Count > 2)
                        //{
                        //    for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                        //    {
                        //        if (Convert.ToInt32(ds.Tables[2].Rows[k]["PlanID"]) == plan.PlanID)
                        //        {
                        //            Brand brand = new();

                        //            brand.BrandID = Convert.ToInt32(ds.Tables[2].Rows[k]["I_Brand_ID"]);
                        //            brand.BrandName = ds.Tables[2].Rows[k]["S_Brand_Name"].ToString();

                        //            plan.BrandDetails.Add(brand);
                        //        }
                        //    }
                        //}

                        plans.Add(plan);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return plans;
        }

        public List<Plan> GetPlanListForPublishing(string BrandIDList, bool CanBePublished=true)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Plan> plans = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@BrandIDList", BrandIDList);
                sqlparams[1] = new SqlParameter("@CanBePublished", CanBePublished);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPlanListForPublishing", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Plan plan = new();

                        plan.PlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["PlanID"]);

                        plan.PlanCode = ds.Tables[0].Rows[i]["PlanCode"].ToString();
                        plan.PlanName = ds.Tables[0].Rows[i]["PlanName"].ToString();
                        plan.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);

                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {
                            plan.PlanLanguageID = 0;
                            plan.PlanLanguageName = null;
                        }
                        else
                        {
                            plan.PlanLanguageID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                            plan.PlanLanguageName = ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }

                        plans.Add(plan);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return plans;
        }

        public List<PlanProduct> GetPlanProductDetails(int PlanID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            List<PlanProduct> planproducts = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@PlanID", PlanID);


                ds = dh.ExecuteDataSet("ECommerce.uspGetPlanProductDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                    {
                        PlanProduct planproduct = new();
                        planproduct.PlanID = Convert.ToInt32(ds.Tables[0].Rows[l]["PlanID"]);
                        planproduct.PlanProductID = Convert.ToInt32(ds.Tables[0].Rows[l]["PlanProductID"]);
                        planproduct.PlanName = ds.Tables[0].Rows[l]["PlanName"].ToString();
                        
                        //susmita
                        planproduct.PlanLanguageID = Convert.ToInt32(ds.Tables[0].Rows[l]["I_Language_ID"]);
                        planproduct.PlanLanguageName = ds.Tables[0].Rows[l]["I_Language_Name"].ToString();
                        

                        //susmita
           

                        for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                        {
                            PlanConfig config = new();

                            if (Convert.ToInt32(ds.Tables[6].Rows[i]["PlanID"]) == planproduct.PlanID)
                            {
                                config.ConfigID = Convert.ToInt32(ds.Tables[6].Rows[i]["ConfigID"]);
                                config.PlanConfigID = Convert.ToInt32(ds.Tables[6].Rows[i]["PlanConfigID"]);
                                config.ConfigCode = ds.Tables[6].Rows[i]["ConfigCode"].ToString();
                                config.DisplayName = ds.Tables[6].Rows[i]["DisplayName"].ToString();
                                config.ConfigValue = ds.Tables[6].Rows[i]["ConfigValue"].ToString();

                                planproduct.PlanConfigList.Add(config);
                            }
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Product product = new();

                            if (Convert.ToInt32(ds.Tables[1].Rows[i]["PlanProductID"]) == planproduct.PlanProductID)
                            {

                                product.ProductID = Convert.ToInt32(ds.Tables[1].Rows[i]["ProductID"]);
                                product.BrandID = Convert.ToInt32(ds.Tables[1].Rows[i]["BrandID"]);
                                //product.CategoryID = Convert.ToInt32(ds.Tables[1].Rows[i]["CategoryID"]);
                                product.CategoryDetails.CategoryID= Convert.ToInt32(ds.Tables[1].Rows[i]["CategoryID"]);
                                product.CategoryDetails.CategoryCode= ds.Tables[1].Rows[i]["CategoryCode"].ToString();
                                product.CategoryDetails.CategoryName= ds.Tables[1].Rows[i]["CategoryName"].ToString();
                                product.CategoryDetails.CategoryDesc= ds.Tables[1].Rows[i]["CategoryDesc"].ToString();
                                product.CategoryDetails.IsOnline= Convert.ToBoolean(ds.Tables[1].Rows[i]["IsOnline"]);
                                product.CategoryDetails.IsOffline = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsOffline"]);


                                product.CourseID = Convert.ToInt32(ds.Tables[1].Rows[i]["CourseID"]);


                                product.ProductCode = ds.Tables[1].Rows[i]["ProductCode"].ToString();
                                product.ProductName = ds.Tables[1].Rows[i]["ProductName"].ToString();
                                product.ShortDesc = ds.Tables[1].Rows[i]["ProductShortDesc"].ToString();
                                product.LongDesc = ds.Tables[1].Rows[i]["ProductLongDesc"].ToString();
                                product.ProductImage = ds.Tables[1].Rows[i]["ProductImage"].ToString();

                                for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                                {
                                    if (product.ProductID == Convert.ToInt32(ds.Tables[2].Rows[j]["ProductID"]))
                                    {
                                        ProductCenterMap centerMap = new();

                                        centerMap.ProductID = Convert.ToInt32(ds.Tables[2].Rows[j]["ProductID"]);
                                        centerMap.ProductCenterID = Convert.ToInt32(ds.Tables[2].Rows[j]["ProductCentreID"]);
                                        centerMap.CenterID = Convert.ToInt32(ds.Tables[2].Rows[j]["CenterID"]);
                                        centerMap.ProspectusAmount = Convert.ToDecimal(ds.Tables[2].Rows[j]["ProspectusAmount"]);

                                        centerMap.CenterName = ds.Tables[2].Rows[j]["S_Center_Name"].ToString();

                                        for (int k = 0; k < ds.Tables[3].Rows.Count; k++)
                                        {
                                            if (centerMap.ProductCenterID == Convert.ToInt32(ds.Tables[3].Rows[k]["ProductCentreID"]))
                                            {
                                                ProductFeePlan feePlan = new();

                                                feePlan.ProductFeePlanID = Convert.ToInt32(ds.Tables[3].Rows[k]["ProductFeePlanID"]);
                                                feePlan.CourseFeePlanID = Convert.ToInt32(ds.Tables[3].Rows[k]["CourseFeePlanID"]);
                                                feePlan.NumberOfInstalments = Convert.ToInt32(ds.Tables[3].Rows[k]["N_No_Of_Installments"]);
                                                feePlan.FeePlanAmount_Lumpsum = Convert.ToDecimal(ds.Tables[3].Rows[k]["N_TotalLumpSum"]);
                                                feePlan.FeePlanAmount_Instalment = Convert.ToDecimal(ds.Tables[3].Rows[k]["N_TotalInstallment"]);
                                                feePlan.FirstInstalmentAmount = Convert.ToDecimal(ds.Tables[3].Rows[k]["FirstInstAmount"]);
                                                feePlan.LumpsumTax = Convert.ToDecimal(ds.Tables[3].Rows[k]["LumpsumTax"]);
                                                feePlan.InstalmentTax = Convert.ToDecimal(ds.Tables[3].Rows[k]["InstalmentTax"]);
                                                feePlan.TotalInstalmentTax = Convert.ToDecimal(ds.Tables[3].Rows[k]["TotalInstalmentTax"]);

                                                feePlan.FeePlan = ds.Tables[3].Rows[k]["S_Fee_Plan_Name"].ToString();
                                                feePlan.FeePlanDisplayName = ds.Tables[3].Rows[k]["ProductFeePlanDisplayName"].ToString();

                                                feePlan.FeePlanDetails = GetFeePlanDetails(feePlan.CourseFeePlanID, 2, 0, DateTime.Now);

                                                feePlan.FeePlanDetails = feePlan.FeePlanDetails.Where(w => w.IsLumpsum == "N" && w.Amount > 0).ToList();

                                                for(int m=0;m<feePlan.FeePlanDetails.Count;m++)
                                                {
                                                    var f = feePlan.FeePlanDetails[m];
                                                    if(feePlan.InstalmentSummary.Where(w=>w.InstalmentNo==f.InstalmentNo).ToList().Count>0)
                                                    {
                                                        int index = feePlan.InstalmentSummary.FindIndex(x => x.InstalmentNo == f.InstalmentNo);

                                                        feePlan.InstalmentSummary[index].Amount += f.Amount;
                                                        feePlan.InstalmentSummary[index].Tax += f.TaxDetails.Sum(s=>s.TaxAmount);
                                                    }
                                                    else
                                                    {
                                                        feePlan.InstalmentSummary.Add(new InstalmentSummary()
                                                        {
                                                            InstalmentNo=f.InstalmentNo,
                                                            InstalmentDate=f.InstalmentDate,
                                                            Amount=f.Amount,
                                                            Tax=f.TaxDetails.Sum(s=>s.TaxAmount)
                                                        });
                                                    }
                                                }

                                                centerMap.FeePlans.Add(feePlan);
                                            }
                                        }

                                        product.CenterAvailability.Add(centerMap);
                                    }
                                }

                                for(int m=0;m<ds.Tables[4].Rows.Count;m++)
                                {
                                    if(Convert.ToInt32(ds.Tables[4].Rows[m]["ProductID"])==product.ProductID)
                                    {
                                        ProductConfig config = new();

                                        config.ProductConfigID = Convert.ToInt32(ds.Tables[4].Rows[m]["ProductConfigID"]);
                                        config.ProductID= Convert.ToInt32(ds.Tables[4].Rows[m]["ProductID"]);
                                        config.ConfigID= Convert.ToInt32(ds.Tables[4].Rows[m]["ConfigID"]);
                                        config.HeaderID= Convert.ToInt32(ds.Tables[4].Rows[m]["HeaderID"]);
                                        config.SubHeaderID= Convert.ToInt32(ds.Tables[4].Rows[m]["SubHeaderID"]);
                                        config.ConfigCode= ds.Tables[4].Rows[m]["ConfigCode"].ToString();

                                        if (ds.Tables[4].Rows[m]["ConfigValue"]!=DBNull.Value)
                                            config.ConfigValue = ds.Tables[4].Rows[m]["ConfigValue"].ToString();

                                        if(ds.Tables[4].Rows[m]["ConfigDisplayName"]!=DBNull.Value)
                                            config.ConfigDisplayName= ds.Tables[4].Rows[m]["ConfigDisplayName"].ToString();

                                        if(ds.Tables[4].Rows[m]["HeaderDisplayName"]!=DBNull.Value)
                                            config.HeaderDisplayName= ds.Tables[4].Rows[m]["HeaderDisplayName"].ToString();

                                        if (ds.Tables[4].Rows[m]["SubHeaderDisplayName"] != DBNull.Value)
                                            config.SubHeaderDisplayName= ds.Tables[4].Rows[m]["SubHeaderDisplayName"].ToString();


                                        product.ProductConfigList.Add(config);
                                    }
                                }

                                for (int m = 0; m < ds.Tables[5].Rows.Count; m++)
                                {
                                    if (Convert.ToInt32(ds.Tables[5].Rows[m]["ProductID"]) == product.ProductID)
                                    {
                                        ExamCategory exam = new();

                                        exam.ExamCategoryID = Convert.ToInt32(ds.Tables[5].Rows[m]["ExamCategoryID"]);
                                        exam.ExamCategoryName = ds.Tables[5].Rows[m]["ExamCategoryDesc"].ToString();


                                        product.ExamCategoryList.Add(exam);
                                    }
                                }



                                planproduct.ProductDetails = product;
                            }
                        }

                        planproducts.Add(planproduct);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return planproducts;
        }

        public List<string> GetReceiptInvoices(int ReceiptDetailID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            List<string> invoices = new();

            try
            {
                ds = dh.ExecuteDataSet("select DISTINCT B.S_Invoice_Number from T_Receipt_Component_Detail A inner join T_Invoice_Child_Detail B on A.I_Invoice_Detail_ID=B.I_Invoice_Detail_ID where A.I_Receipt_Detail_ID=" + ReceiptDetailID.ToString(), CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        invoices.Add(ds.Tables[0].Rows[i]["S_Invoice_Number"].ToString());
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return invoices;
        }

        public List<HighestEducationQualification> GetHighestEducationQualificationList(int BrandID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<HighestEducationQualification> qualifications = new();

            try
            {
                ds = dh.ExecuteDataSet("SELECT * FROM T_Education_CurrentStatus where I_Status=1 and I_Brand_ID=" + BrandID, CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        HighestEducationQualification qualification = new();

                        qualification.HighestQualificationID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Education_CurrentStatus_ID"]);
                        qualification.HighestQualificationDesc = ds.Tables[0].Rows[i]["S_Education_CurrentStatus_Description"].ToString();

                        qualifications.Add(qualification);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return qualifications;
        }

        public int SaveRegistration(Registration reg)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            int RegID = 0;
            string CourseList = "";

            try
            {
                if (reg.RegCourseList != null && reg.RegCourseList.Count > 0)
                {
                    foreach (var c in reg.RegCourseList)
                    {
                        CourseList += c.CourseName+",";
                    }
                }




                SqlParameter[] sqlparams = new SqlParameter[18];


                sqlparams[0] = new SqlParameter("@CustomerID", reg.CustomerID);

                sqlparams[1] = new SqlParameter("@FirstName", reg.FirstName);

                if (reg.MiddleName != null)
                    sqlparams[2] = new SqlParameter("@MiddleName", reg.MiddleName);
                else
                    sqlparams[2] = new SqlParameter("@MiddleName", DBNull.Value);

                sqlparams[3] = new SqlParameter("@EmailID", reg.EmailID);
                sqlparams[4] = new SqlParameter("@MobileNo", reg.MobileNo);
                sqlparams[5] = new SqlParameter("@HighestEducationQualification", reg.HighestEducationQualification);
                sqlparams[6] = new SqlParameter("@RegSource", reg.RegistrationSource);
                sqlparams[7] = new SqlParameter("@LastName", reg.LastName);
                sqlparams[8] = new SqlParameter("@Gender", reg.Gender);
                sqlparams[9] = new SqlParameter("@DoB", reg.DoB);
                sqlparams[10] = new SqlParameter("@CoursesList", CourseList);

                if (reg.Address != null)
                    sqlparams[11] = new SqlParameter("@Address", reg.Address);
                else
                    sqlparams[11] = new SqlParameter("@Address", DBNull.Value);

                if (reg.City != null)
                    sqlparams[12] = new SqlParameter("@City", reg.City);
                else
                    sqlparams[12] = new SqlParameter("@City", DBNull.Value);

                if(reg.State != null)
                    sqlparams[13] = new SqlParameter("@State", reg.State);
                else
                    sqlparams[13] = new SqlParameter("@State", DBNull.Value);

                if(reg.Country != null)
                    sqlparams[14] = new SqlParameter("@Country", reg.Country);
                else
                    sqlparams[14] = new SqlParameter("@Country", DBNull.Value);

                if (reg.Pincode!=null)
                    sqlparams[15] = new SqlParameter("@Pincode", reg.Pincode);
                else
                    sqlparams[15] = new SqlParameter("@Pincode", DBNull.Value);

                if (reg.LanguageID > 0)
                    sqlparams[16] = new SqlParameter("@LanguageID", reg.LanguageID);
                else
                    sqlparams[16] = new SqlParameter("@LanguageID", DBNull.Value);

                if (reg.LanguageName != null)
                    sqlparams[17] = new SqlParameter("@LanguageName", reg.LanguageName);
                else
                    sqlparams[17] = new SqlParameter("@LanguageName", DBNull.Value);


                RegID =Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSaveRegistration", CommandType.StoredProcedure, sqlparams));


            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return RegID;
        }

        public int UpdateRegistration(Registration reg)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            int RegID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[5];


                sqlparams[0] = new SqlParameter("@CustomerID", reg.CustomerID);

                if (reg.GuardianName != null)
                    sqlparams[1] = new SqlParameter("@GuardianName", reg.GuardianName);
                else
                    sqlparams[1] = new SqlParameter("@GuardianName", DBNull.Value);

                if (reg.GuardianMobileNo != null)
                    sqlparams[2] = new SqlParameter("@GuardianMobileNo", reg.GuardianMobileNo);
                else
                    sqlparams[2] = new SqlParameter("@GuardianMobileNo", DBNull.Value);

                if (reg.SecondLanguage != null)
                    sqlparams[3] = new SqlParameter("@SecondaryLanguage", reg.SecondLanguage);
                else
                    sqlparams[3] = new SqlParameter("@SecondaryLanguage", DBNull.Value);

                if (reg.SocialCategory != null)
                    sqlparams[4] = new SqlParameter("@SocialCategory", reg.SocialCategory);
                else
                    sqlparams[4] = new SqlParameter("@SocialCategory", DBNull.Value);


                RegID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspUpdateRegistration", CommandType.StoredProcedure, sqlparams));


            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return RegID;
        }

        public Registration GetRegistrationDetails(string CustomerID=null, int RegID=0)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            Registration reg = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];

                if(CustomerID!=null)
                    sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);
                else
                    sqlparams[0] = new SqlParameter("@CustomerID", DBNull.Value);

                if (RegID>0)
                    sqlparams[1] = new SqlParameter("@RegID", RegID);
                else
                    sqlparams[1] = new SqlParameter("@RegID", DBNull.Value);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetRegistrationDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    reg.CustomerID = ds.Tables[0].Rows[0]["CustomerID"].ToString(); ;

                    if (ds.Tables[0].Rows[0]["GuardianName"] != DBNull.Value)
                        reg.GuardianName = ds.Tables[0].Rows[0]["GuardianName"].ToString();

                    if (ds.Tables[0].Rows[0]["GuardianMobileNo"] != DBNull.Value)
                        reg.GuardianMobileNo = ds.Tables[0].Rows[0]["GuardianMobileNo"].ToString();

                    if (ds.Tables[0].Rows[0]["SecondLanguage"] != DBNull.Value)
                        reg.SecondLanguage = ds.Tables[0].Rows[0]["SecondLanguage"].ToString();

                    if (ds.Tables[0].Rows[0]["SocialCategory"] != DBNull.Value)
                        reg.SocialCategory = ds.Tables[0].Rows[0]["SocialCategory"].ToString();
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return reg;
        }

        public List<Config> GetConfigs()
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Config> configs = new();


            try
            {
                ds = dh.ExecuteDataSet("SELECT * FROM ECOMMERCE.T_Cofiguration_Property_Master WHERE StatusID=1", CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Config config = new Config();

                        config.ConfigID = Convert.ToInt32(ds.Tables[0].Rows[i]["ConfigID"]);
                        config.ConfigCode = ds.Tables[0].Rows[i]["ConfigCode"].ToString();
                        config.ConfigName = ds.Tables[0].Rows[i]["ConfigName"].ToString();
                        config.ConfigDefaultValue = ds.Tables[0].Rows[i]["ConfigDefaultValue"].ToString();


                        configs.Add(config);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return configs;
        }

        public ProductConfig SaveProductConfig(ProductConfig config)
        {
            DataHelper dh = new DataHelper(_conn);
            //DataSet ds = new DataSet();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[8];
                sqlparams[0] = new SqlParameter("@ProductID", config.ProductID);
                sqlparams[1] = new SqlParameter("@ConfigID", config.ConfigID);

                if (config.ConfigValue == null)
                    sqlparams[2] = new SqlParameter("@ConfigValue", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@ConfigValue", config.ConfigValue);

                if (config.ConfigDisplayName == null)
                    sqlparams[3] = new SqlParameter("@ConfigDisplayName", DBNull.Value);
                else
                    sqlparams[3] = new SqlParameter("@ConfigDisplayName", config.ConfigDisplayName);

                if (config.SubHeaderID <= 0)
                    sqlparams[4] = new SqlParameter("@SubHeaderID", DBNull.Value);
                else
                    sqlparams[4] = new SqlParameter("@SubHeaderID", config.SubHeaderID);

                if (config.HeaderID <= 0)
                    sqlparams[5] = new SqlParameter("@HeaderID", DBNull.Value);
                else
                    sqlparams[5] = new SqlParameter("@HeaderID", config.SubHeaderID);

                if (config.SubHeaderDisplayName == null)
                    sqlparams[6] = new SqlParameter("@SubHeaderDisplayName", DBNull.Value);
                else
                    sqlparams[6] = new SqlParameter("@SubHeaderDisplayName", config.SubHeaderDisplayName);

                if (config.HeaderDisplayName == null)
                    sqlparams[7] = new SqlParameter("@HeaderDisplayName", DBNull.Value);
                else
                    sqlparams[7] = new SqlParameter("@HeaderDisplayName", config.HeaderDisplayName);


                config.ProductConfigID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSaveProductConfigMap", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return config;
        }

        public PlanConfig SavePlanConfig(PlanConfig config)
        {
            DataHelper dh = new DataHelper(_conn);
            //DataSet ds = new DataSet();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@PlanID", config.PlanID);
                sqlparams[1] = new SqlParameter("@ConfigID", config.ConfigID);

                if (config.ConfigValue == null)
                    sqlparams[2] = new SqlParameter("@ConfigValue", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@ConfigValue", config.ConfigValue);

                if (config.DisplayName == null)
                    sqlparams[3] = new SqlParameter("@ConfigDisplayName", DBNull.Value);
                else
                    sqlparams[3] = new SqlParameter("@ConfigDisplayName", config.DisplayName);


                config.PlanConfigID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSavePlanConfigMap", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return config;
        }

        public int SaveConfigDetails(Config config)
        {
            DataHelper dh = new DataHelper(_conn);
            int c = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];

                sqlparams[0] = new SqlParameter("@ConfigCode", config.ConfigCode);
                sqlparams[1] = new SqlParameter("@ConfigName", config.ConfigName);

                if (config.ConfigDefaultValue == null)
                    sqlparams[2] = new SqlParameter("@ConfigDefaultValue", "");
                else
                    sqlparams[2] = new SqlParameter("@ConfigDefaultValue", config.ConfigDefaultValue);

                c = dh.ExecuteNonQuery("ECOMMERCE.uspSaveConfigDetails", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return c;
        }

        public List<Gender> GetGenders()
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Gender> genders = new();


            try
            {
                ds = dh.ExecuteDataSet("uspGetSexType", CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Gender gender = new();

                        gender.GenderID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Sex_ID"]);
                        gender.Desc = ds.Tables[0].Rows[i]["S_Sex_Name"].ToString();

                        genders.Add(gender);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return genders;
        }

        public List<ExamCategory> GetExamCategories(int BrandID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<ExamCategory> exams = new();


            try
            {
                ds = dh.ExecuteDataSet("SELECT * FROM ECOMMERCE.T_ExamCategory_Master WHERE StatusID=1 and BrandID="+BrandID.ToString(), CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ExamCategory exam = new();

                        exam.ExamCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["ExamCategoryID"]);
                        exam.ExamCategoryName = ds.Tables[0].Rows[i]["ExamCategoryDesc"].ToString();


                        exams.Add(exam);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return exams;
        }

        public List<Coupon> GetCouponsForPlan(int PlanID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new();

            List<Coupon> coupons = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];

                sqlparams[0] = new SqlParameter("@PlanID", PlanID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPlanCoupons", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count == 2)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Coupon coupon = new();

                        coupon.CouponID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponID"]);
                        coupon.CouponCode = ds.Tables[0].Rows[i]["CouponCode"].ToString();
                        coupon.CouponName = ds.Tables[0].Rows[i]["CouponName"].ToString();
                        coupon.CouponDesc = ds.Tables[0].Rows[i]["CouponDesc"].ToString();
                        coupon.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        coupon.CouponCount = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCount"]);
                        coupon.AssignedCount = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignedCount"]);

                        coupon.CouponTypeDetails.CouponTypeID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponType"]);
                        coupon.CouponTypeDetails.CouponTypeDesc = ds.Tables[0].Rows[i]["CouponTypeDesc"].ToString();

                        coupon.CategoryDetails.CouponCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);
                        coupon.CategoryDetails.CouponCategoryDesc = ds.Tables[0].Rows[i]["CouponCategoryDesc"].ToString();

                        coupon.DiscountDetails.DiscountSchemeID = Convert.ToInt32(ds.Tables[0].Rows[i]["DiscountSchemeID"]);
                        coupon.DiscountDetails.DiscountSchemeName = ds.Tables[0].Rows[i]["S_Discount_Scheme_Name"].ToString();

                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            DiscountSchemeDetail detail = new();

                            if (Convert.ToInt32(ds.Tables[1].Rows[j]["I_Discount_Scheme_ID"]) == coupon.DiscountDetails.DiscountSchemeID)
                            {
                                detail.DiscountSchemeDetailID = Convert.ToInt32(ds.Tables[1].Rows[j]["I_Discount_Scheme_Detail_ID"]);

                                if (ds.Tables[1].Rows[j]["N_Discount_Rate"] != DBNull.Value)
                                    detail.DiscountRate = Convert.ToDecimal(ds.Tables[1].Rows[j]["N_Discount_Rate"]);

                                if (ds.Tables[1].Rows[j]["N_Discount_Amount"] != DBNull.Value)
                                    detail.DiscountAmount = Convert.ToDecimal(ds.Tables[1].Rows[j]["N_Discount_Amount"]);


                                detail.IsApplicableOn = Convert.ToInt32(ds.Tables[1].Rows[j]["I_IsApplicableOn"]);

                                if (ds.Tables[1].Rows[j]["S_FromInstalment"] != DBNull.Value)
                                    detail.FromInstalment = ds.Tables[1].Rows[j]["S_FromInstalment"].ToString();

                                if (ds.Tables[1].Rows[j]["S_FeeComponents"] != DBNull.Value)
                                    detail.FeeComponentID = ds.Tables[1].Rows[j]["S_FeeComponents"].ToString();

                                coupon.DiscountDetails.DiscountSchemeDetails.Add(detail);
                            }
                        }

                        coupons.Add(coupon);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return coupons;
        }

        public ValidateCoupon ValidatePlanCoupon(int BrandID, string CouponCode, int PlanID, int ProductID, int PaymentMode, int CenterID, ValidateCoupon obj, 
                                                    decimal PurchaseAmount=0, string CustomerID=null)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new();
            

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[8];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);
                sqlparams[1] = new SqlParameter("@CouponCode", CouponCode);
                sqlparams[2] = new SqlParameter("@PlanID", PlanID);
                sqlparams[3] = new SqlParameter("@ProductID", ProductID);
                sqlparams[4] = new SqlParameter("@PaymentMode", PaymentMode);
                sqlparams[5] = new SqlParameter("@CenterID", CenterID);
                sqlparams[6] = new SqlParameter("@PurchaseAmount", PurchaseAmount);

                if(CustomerID!=null && CustomerID!="")
                    sqlparams[7] = new SqlParameter("@CustomerID", CustomerID);
                else
                    sqlparams[7] = new SqlParameter("@CustomerID", DBNull.Value);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspValidateCouponCodeForPlan", CommandType.StoredProcedure, sqlparams);

                if(ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
                {
                    obj.PayableAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["PayableAmount"]);
                    obj.PayableTax = Convert.ToDecimal(ds.Tables[0].Rows[0]["PayableTax"]);
                    obj.IsCouponApplicable = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCouponApplicable"]);
                    obj.TotalPayableInstalmentAmount= Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalInstalmentPayable"]);
                    obj.TotalPayableInstalmenTaxtAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalInstalmentPayableTax"]);
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return obj;
        }

        public int SaveDiscountScheme(DiscountScheme discountScheme, string BrandList,string CreatedBy)
        {
            DataHelper dh = new DataHelper(_conn);
            int DiscountSchemeID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[5];
                sqlparams[0] = new SqlParameter("@DiscountSchemeName", discountScheme.DiscountSchemeName);
                sqlparams[1] = new SqlParameter("@ValidFrom", discountScheme.ValidFrom);
                sqlparams[2] = new SqlParameter("@ValidTo", discountScheme.ValidTo);
                sqlparams[3] = new SqlParameter("@CreatedBy", CreatedBy);
                sqlparams[4] = new SqlParameter("@sBrands", BrandList);

                DiscountSchemeID = Convert.ToInt32(dh.ExecuteScalar("uspSaveDiscountScheme", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return DiscountSchemeID;
        }

        public int SaveDiscountSchemeDetails(int DiscountSchemeID, DiscountSchemeDetail discountScheme, string CreatedBy)
        {
            DataHelper dh = new DataHelper(_conn);
            int i = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[7];
                sqlparams[0] = new SqlParameter("@DiscountSchemeID",DiscountSchemeID);

                if(discountScheme.DiscountRate>0)
                    sqlparams[1] = new SqlParameter("@DiscountRate", discountScheme.DiscountRate);
                else
                    sqlparams[1] = new SqlParameter("@DiscountRate", DBNull.Value);

                if(discountScheme.DiscountAmount>0)
                    sqlparams[2] = new SqlParameter("@DiscountAmount", discountScheme.DiscountAmount);
                else
                    sqlparams[2] = new SqlParameter("@DiscountAmount", DBNull.Value);

                sqlparams[3] = new SqlParameter("@IsApplicableOn", discountScheme.IsApplicableOn);

                if(discountScheme.FromInstalment==null || discountScheme.FromInstalment=="")
                    sqlparams[4] = new SqlParameter("@FromInstalment", DBNull.Value);
                else
                    sqlparams[4] = new SqlParameter("@FromInstalment", discountScheme.FromInstalment);

                if (discountScheme.FeeComponentID == null || discountScheme.FeeComponentID == "")
                    sqlparams[5] = new SqlParameter("@FeeComponentID", DBNull.Value);
                else
                    sqlparams[5] = new SqlParameter("@FeeComponentID", discountScheme.FeeComponentID);

                sqlparams[6] = new SqlParameter("@CreatedBy", CreatedBy);

                i=dh.ExecuteNonQuery("uspSaveDiscountSchemeDetails", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return i;
        }

        public void SaveDiscountSchemeCenterCourseMap(int DiscountSchemeID, int CenterID, int CourseID, string CreatedBy)
        {
            DataHelper dh = new DataHelper(_conn);

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];

                sqlparams[0] = new SqlParameter("@DiscountSchemeID", DiscountSchemeID);
                sqlparams[1] = new SqlParameter("@CenterID", CenterID);
                sqlparams[2] = new SqlParameter("@CourseID", CourseID);
                sqlparams[3] = new SqlParameter("@CreatedBy", CreatedBy);


                dh.ExecuteNonQuery("uspSaveDiscountSchemeCenterCourseMap", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public List<DiscountSchemeCenterMap> GetDiscountSchemeCenterCourseMap(int DiscountSchemeID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new();
            List<DiscountSchemeCenterMap> centerMaps = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@DiscountSchemeID", DiscountSchemeID);

                ds = dh.ExecuteDataSet("uspGetDiscountSchemeCenterCourseMap", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DiscountSchemeCenterMap map = new();

                        map.DiscountCenterDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Discount_Center_Detail_ID"]);
                        map.CenterID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Centre_ID"]);

                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            DiscountSchemeCenterCourseMap courseMap = new();

                            if (Convert.ToInt32(ds.Tables[1].Rows[j]["I_Discount_Centre_Detail_ID"]) == map.DiscountCenterDetailID)
                            {
                                courseMap.DiscountCenterCourseDetailID = Convert.ToInt32(ds.Tables[1].Rows[j]["I_Discount_Course_Detail_ID"]);
                                courseMap.CourseID = Convert.ToInt32(ds.Tables[1].Rows[j]["I_Course_ID"]);

                                map.CenterCourseMaps.Add(courseMap);
                            }
                        }

                        centerMaps.Add(map);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return centerMaps;
        }

        public List<FeeComponent> GetFeeComponents(int BrandID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new();

            List<FeeComponent> fees = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetFeeComponentList", CommandType.StoredProcedure,sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FeeComponent fee = new();

                        fee.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Brand_ID"]);
                        fee.FeeComponentID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Fee_Component_ID"]);
                        fee.FeeComponentName = ds.Tables[0].Rows[i]["S_Component_Name"].ToString();

                        fees.Add(fee);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return fees;
        }

        public List<Brand> GetBrands()
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new();

            List<Brand> brands = new();

            try
            {
                ds = dh.ExecuteDataSet("SELECT * FROM T_Brand_Master where I_Brand_ID>106", CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Brand brand = new();

                        brand.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Brand_ID"]);
                        brand.BrandName = ds.Tables[0].Rows[i]["S_Brand_Name"].ToString();

                        brands.Add(brand);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return brands;
        }

        public int SaveCoupon(Coupon coupon, string BrandList)
        {
            DataHelper dh = new (_conn);
            DataSet ds = new();
            int couponid = 0;


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[14];

                sqlparams[0] = new SqlParameter("@CouponCode", coupon.CouponCode);
                sqlparams[1] = new SqlParameter("@CouponName", coupon.CouponName);
                sqlparams[2] = new SqlParameter("@CouponDesc", coupon.CouponDesc);
                sqlparams[3] = new SqlParameter("@sBrandID", BrandList);
                sqlparams[4] = new SqlParameter("@DiscountID", coupon.DiscountDetails.DiscountSchemeID);
                sqlparams[5] = new SqlParameter("@CouponCategoryID", coupon.CategoryDetails.CouponCategoryID);
                sqlparams[6] = new SqlParameter("@CouponTypeID", coupon.CouponTypeDetails.CouponTypeID);
                sqlparams[7] = new SqlParameter("@CouponCount", coupon.CouponCount);

                if(coupon.GreaterThanAmount>0)
                    sqlparams[8] = new SqlParameter("@GreaterThanAmount", coupon.GreaterThanAmount);
                else
                    sqlparams[8] = new SqlParameter("@GreaterThanAmount", DBNull.Value);

                if(coupon.CustomerCode==null || coupon.CustomerCode=="")
                    sqlparams[9] = new SqlParameter("@CustomerCode", DBNull.Value);
                else
                    sqlparams[9] = new SqlParameter("@CustomerCode", coupon.CustomerCode);

                sqlparams[10] = new SqlParameter("@ValidFrom", coupon.ValidFrom);
                sqlparams[11] = new SqlParameter("@ValidTo", coupon.ValidTo);
                sqlparams[12] = new SqlParameter("@PerStudentCount", coupon.PerStudentCount);

                sqlparams[13] = new SqlParameter("@IsPrivate", coupon.IsPrivate);//susmita: 2022-09-12


                couponid = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSaveCoupon", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return couponid;
        }

        public RegistrationCourseEnrolment GetRegistrationCourseEnrolment(string CustomerID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            RegistrationCourseEnrolment enrolment = new();
            //string courselist = "";

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetExistingEnrollmentsForCustomerID", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    enrolment.RegID = Convert.ToInt32(ds.Tables[0].Rows[0]["RegID"]);
                    enrolment.CustomerID = ds.Tables[0].Rows[0]["CustomerID"].ToString();
                    //courselist= ds.Tables[0].Rows[0]["CourseList"].ToString();

                    for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                    {
                        if (ds.Tables[0].Rows[i]["CourseID"] != DBNull.Value)
                        {
                            enrolment.CourseList.Add(new CourseEnrolment()
                            {
                                CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]),
                                CanBePurchased = Convert.ToBoolean(ds.Tables[0].Rows[i]["CanBePurchased"])
                            });
                        }
                    }

                    //if (courselist != null && courselist.Trim() != "")
                    //{
                    //    foreach (var c in courselist.Split(",").ToList())
                    //    {
                    //        enrolment.CourseIDList.Add(Convert.ToInt32(c));
                    //    }
                    //}

                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    enrolment.CourseIDList.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["I_Course_ID"]));
                    //}
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return enrolment;
        }

        public void CloseExistingSameCourseAccounts(string CustomerID, int ProductID, SqlTransaction trans, DataHelper dh)
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];

                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);
                sqlparams[1] = new SqlParameter("@ProductID", ProductID);

                dh.ExecuteNonQuery("ECOMMERCE.uspProcessExistingInvoiceHeaderListForProduct", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void SetAdmissionFeeSchedule(int FeeScheduleID, SqlTransaction trans, DataHelper dh)
        {
            try
            {
                dh.ExecuteNonQuery("Update T_Invoice_Parent set IsAdmissionInvoice=1 where I_Invoice_Header_ID=" + FeeScheduleID.ToString(), CommandType.Text, trans);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public List<Centre> GetCentres(int BrandID=109)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<Centre> centres = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                ds = dh.ExecuteDataSet("LMS.uspGetCentres", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Centre centre = new();

                        centre.CenterID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Centre_ID"]);
                        centre.CenterName = ds.Tables[0].Rows[i]["S_Center_Name"].ToString();

                        centres.Add(centre);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return centres;
        }

        public List<Course> GetCourses(int BrandID = 109)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<Course> courses = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetCourses", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Course course = new();

                        course.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Course_ID"]);
                        course.CourseName = ds.Tables[0].Rows[i]["S_Course_Name"].ToString();

                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {
                            course.CourseLanguageID = 0;
                            course.CourseLangaugeName = null;
                        }
                        else
                        { 
                            course.CourseLanguageID= Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                            course.CourseLangaugeName= ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }


                        courses.Add(course);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return courses;
        }

        public List<DiscountScheme> GetDiscountSchemes(string BrandIDList)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<DiscountScheme> schemes = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@sBrand", BrandIDList);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetDiscountSchemeList", CommandType.StoredProcedure,sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DiscountScheme scheme = new();

                        scheme.DiscountSchemeID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Discount_Scheme_ID"]);
                        scheme.DiscountSchemeName = ds.Tables[0].Rows[i]["S_Discount_Scheme_Name"].ToString();

                        schemes.Add(scheme);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return schemes;
        }

        public List<CouponCategory> GetCouponCategories()
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<CouponCategory> coupons = new();

            try
            {
                ds = dh.ExecuteDataSet("Select * from ECOMMERCE.T_Coupon_Category_Master where StatusID=1", CommandType.Text);


                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CouponCategory coupon = new();

                        coupon.CouponCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);
                        coupon.CouponCategoryDesc = ds.Tables[0].Rows[i]["CouponCategoryDesc"].ToString();

                        coupons.Add(coupon);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return coupons;
        }

        public bool ValidatePurchase(Payment payment)
        {
            DataHelper dh = new(_conn);
            //DataSet ds = new();
            Boolean flag = false;

            try
            {
                string PayXML = GeneratePaymentXML(payment.PlanPaymentDetails);

                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@PaymentXML", PayXML);
                sqlparams[1] = new SqlParameter("@TransactionAmount", payment.PaidAmount);
                sqlparams[2] = new SqlParameter("@TransactionTax", payment.PaidTax);
                sqlparams[3] = new SqlParameter("@CustomerID", payment.CustomerID);

                flag = Convert.ToBoolean(dh.ExecuteScalar("ECOMMERCE.uspValidatePurchase", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return flag;
        }

        public void SaveTransactionDetails(Payment payment)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            try
            {
                string PayXML = GeneratePaymentXML(payment.PlanPaymentDetails);

                SqlParameter[] sqlparams = new SqlParameter[9];
                sqlparams[0] = new SqlParameter("@PaymentXML", PayXML);
                sqlparams[1] = new SqlParameter("@TransactionNo", payment.TransactionNo);
                sqlparams[2] = new SqlParameter("@CustomerID", payment.CustomerID);
                sqlparams[3] = new SqlParameter("@TransactionDate", payment.TransactionDate);
                sqlparams[4] = new SqlParameter("@TransactionSource", payment.TransactionSource);
                sqlparams[5] = new SqlParameter("@TransactionMode", payment.TransactionMode);
                sqlparams[6] = new SqlParameter("@TransactionStatus", payment.TransactionStatus);
                sqlparams[7] = new SqlParameter("@TransactionAmount", payment.PaidAmount);
                sqlparams[8] = new SqlParameter("@TransactionTax", payment.PaidTax);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspSaveTransactionDetails", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public int SaveSubscriptionTransactionDetails(StudentSubscriptionTransaction payment)
        {
            DataHelper dh = new(_conn);
            int SubscriptionTransactionID = 0;

            try
            {

                SqlParameter[] sqlparams = new SqlParameter[10];
                sqlparams[0] = new SqlParameter("@SubscriptionID", payment.SubscriptionID);
                sqlparams[1] = new SqlParameter("@TransactionNo", payment.TransactionNo);
                sqlparams[2] = new SqlParameter("@TransactionDate", payment.TransactionDate);
                sqlparams[3] = new SqlParameter("@TransactionSource", payment.TransactionSource);
                sqlparams[4] = new SqlParameter("@TransactionMode", payment.TransactionMode);
                sqlparams[5] = new SqlParameter("@TransactionStatus", payment.TransactionStatus);
                sqlparams[6] = new SqlParameter("@TransactionAmount", payment.Amount);
                sqlparams[7] = new SqlParameter("@TransactionTax", payment.Tax);
                sqlparams[8] = new SqlParameter("@FeeScheduleID", payment.FeeScheduleID);
                sqlparams[9] = new SqlParameter("@CustomerID", payment.CustomerID);

                SubscriptionTransactionID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSaveSubscriptionTransaction", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return SubscriptionTransactionID;
        }

        public int SavePayoutTransactionDetails(StudentPayOutTransaction payment, int FeeScheduleID)
        {
            DataHelper dh = new(_conn);
            int PayoutTransactionID = 0;

            try
            {

                SqlParameter[] sqlparams = new SqlParameter[9];
                sqlparams[0] = new SqlParameter("@FeeScheduleID", FeeScheduleID);
                sqlparams[1] = new SqlParameter("@TransactionNo", payment.TransactionNo);
                sqlparams[2] = new SqlParameter("@TransactionDate", payment.TransactionDate);
                sqlparams[3] = new SqlParameter("@TransactionSource", payment.TransactionSource);
                sqlparams[4] = new SqlParameter("@TransactionMode", payment.TransactionMode);
                sqlparams[5] = new SqlParameter("@TransactionStatus", payment.TransactionStatus);
                sqlparams[6] = new SqlParameter("@TransactionAmount", payment.Amount);
                sqlparams[7] = new SqlParameter("@TransactionTax", payment.Tax);

                if(payment.CustomerID!=null)
                    sqlparams[8] = new SqlParameter("@CustomerID", payment.CustomerID);
                else
                    sqlparams[8] = new SqlParameter("@CustomerID", DBNull.Value);

                PayoutTransactionID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSavePayoutTransaction", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return PayoutTransactionID;
        }


        public List<Coupon> GetCoupons(int CouponCategoryID, int BrandID=109)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<Coupon> coupons = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@CouponCategoryID", CouponCategoryID);
                sqlparams[1] = new SqlParameter("@BrandID", BrandID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetCouponList", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Coupon coupon = new();

                        coupon.CouponID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponID"]);
                        coupon.CouponCode = ds.Tables[0].Rows[i]["CouponCode"].ToString();
                        coupon.CouponName = ds.Tables[0].Rows[i]["CouponName"].ToString();

                        coupons.Add(coupon);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return coupons;
        }

        public int SaveCouponPlanMap(int CouponID, int PlanID, DateTime ValidFrom, DateTime ValidTo)
        {
            DataHelper dh = new(_conn);
            int c = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@PlanID", PlanID);
                sqlparams[1] = new SqlParameter("@CouponID", CouponID);

                if (ValidFrom == DateTime.MinValue)
                    sqlparams[2] = new SqlParameter("@ValidFrom", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@ValidFrom", ValidFrom);

                if (ValidTo == DateTime.MinValue)
                    sqlparams[3] = new SqlParameter("@ValidTo", DBNull.Value);
                else
                    sqlparams[3] = new SqlParameter("@ValidTo", ValidTo);

                c=dh.ExecuteNonQuery("ECOMMERCE.uspSaveCouponPlanMap", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return c;
        }

        public Transaction GetTransactionDetails(string TransactionNo)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            Transaction transaction = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@TransactionNo", TransactionNo);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetTransactionDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    transaction.TransactionID = Convert.ToInt32(ds.Tables[0].Rows[0]["TransactionID"]);
                    transaction.TransactionNo = ds.Tables[0].Rows[0]["TransactionNo"].ToString();
                    transaction.CustomerID = ds.Tables[0].Rows[0]["CustomerID"].ToString();
                    transaction.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                    transaction.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                    transaction.EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString();
                    transaction.TransactionStatus = ds.Tables[0].Rows[0]["TransactionStatus"].ToString();
                    transaction.CanBeProcessed = Convert.ToBoolean(ds.Tables[0].Rows[0]["CanBeProcessed"]);
                    transaction.IsCompleted = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCompleted"]);

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        PlanTransaction plan = new();

                        plan.TransactionPlanDetailsID = Convert.ToInt32(ds.Tables[1].Rows[i]["TransactionPlanDetailID"]);
                        plan.PlanID = Convert.ToInt32(ds.Tables[1].Rows[i]["PlanID"]);
                        plan.CanBeProcessed = Convert.ToBoolean(ds.Tables[1].Rows[i]["CanBeProcessed"]);
                        plan.IsCompleted = Convert.ToBoolean(ds.Tables[1].Rows[i]["IsCompleted"]);

                        for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                        {
                            if (Convert.ToInt32(ds.Tables[2].Rows[j]["TransactionPlanDetailID"]) == plan.TransactionPlanDetailsID)
                            {
                                ProductTransaction product = new();

                                product.TransactionProductDetailID = Convert.ToInt32(ds.Tables[2].Rows[j]["TransactionProductDetailID"]);
                                product.ProductID = Convert.ToInt32(ds.Tables[2].Rows[j]["ProductID"]);
                                product.CenterID = Convert.ToInt32(ds.Tables[2].Rows[j]["CenterID"]);
                                product.AmountPaid = Convert.ToDecimal(ds.Tables[2].Rows[j]["AmountPaid"]);
                                product.TaxPaid = Convert.ToDecimal(ds.Tables[2].Rows[j]["TaxPaid"]);

                                if (ds.Tables[2].Rows[j]["DiscountSchemeID"] != DBNull.Value)
                                    product.DiscountSchemeID = Convert.ToInt32(ds.Tables[2].Rows[j]["DiscountSchemeID"]);

                                product.FeePlanID = Convert.ToInt32(ds.Tables[2].Rows[j]["FeePlanID"]);
                                product.PaymentModeID = Convert.ToInt32(ds.Tables[2].Rows[j]["PaymentModeID"]);

                                if (ds.Tables[2].Rows[j]["BatchID"] != DBNull.Value)
                                    product.BatchID = Convert.ToInt32(ds.Tables[2].Rows[j]["BatchID"]);

                                if (ds.Tables[2].Rows[j]["Dt_BatchStartDate"] != DBNull.Value)
                                    product.BatchStartDate = Convert.ToDateTime(ds.Tables[2].Rows[j]["Dt_BatchStartDate"]);

                                if (ds.Tables[2].Rows[j]["I_Brand_ID"] != DBNull.Value)
                                    product.BrandID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Brand_ID"]);

                                if (ds.Tables[2].Rows[j]["I_Course_ID"] != DBNull.Value)
                                    product.CourseID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Course_ID"]);

                                if (ds.Tables[2].Rows[j]["I_Delivery_Pattern_ID"] != DBNull.Value)
                                    product.DeliveryPatternID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Delivery_Pattern_ID"]);

                                if (ds.Tables[2].Rows[j]["FeeScheduleID"] != DBNull.Value)
                                    product.FeeScheduleID = Convert.ToInt32(ds.Tables[2].Rows[j]["FeeScheduleID"]);

                                if (ds.Tables[2].Rows[j]["ReceiptHeaderID"] != DBNull.Value)
                                    product.ReceiptHeaderID = Convert.ToInt32(ds.Tables[2].Rows[j]["ReceiptHeaderID"]);

                                if (ds.Tables[2].Rows[j]["StudentID"] != DBNull.Value)
                                    product.StudentID = ds.Tables[2].Rows[j]["StudentID"].ToString();

                                product.CanBeProcessed = Convert.ToBoolean(ds.Tables[2].Rows[j]["CanBeProcessed"]);
                                product.IsCompleted = Convert.ToBoolean(ds.Tables[2].Rows[j]["IsCompleted"]);

                                plan.ProductDetails.Add(product);
                            }
                        }


                        transaction.PlanDetails.Add(plan);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return transaction;
        }

        public StudentSubscriptionTransaction GetSubscriptionTransactionDetails(string TransactionNo, int FeeScheduleID=0)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            StudentSubscriptionTransaction transaction = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@TransactionNo", TransactionNo);
                sqlparams[1] = new SqlParameter("@FeeScheduleID", FeeScheduleID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetSubscriptionTransaction", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    transaction.SubscriptionTransactionID = Convert.ToInt32(ds.Tables[0].Rows[0]["SubscriptionTransactionID"]);
                    transaction.TransactionNo = ds.Tables[0].Rows[0]["TransactionNo"].ToString();
                    transaction.SubscriptionID = Convert.ToInt32(ds.Tables[0].Rows[0]["SubscriptionDetailID"]);
                    transaction.StudentID = ds.Tables[0].Rows[0]["StudentID"].ToString();
                    transaction.StudentDetailID = Convert.ToInt32(ds.Tables[0].Rows[0]["I_Student_Detail_ID"]);
                    transaction.CenterID = Convert.ToInt32(ds.Tables[0].Rows[0]["CenterID"]);
                    transaction.BrandID = Convert.ToInt32(ds.Tables[0].Rows[0]["BrandID"]);
                    transaction.TransactionMode = ds.Tables[0].Rows[0]["TransactionMode"].ToString();
                    transaction.TransactionSource = ds.Tables[0].Rows[0]["TransactionSource"].ToString();
                    transaction.TransactionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["TransactionDate"]);
                    transaction.TransactionStatus = ds.Tables[0].Rows[0]["TransactionStatus"].ToString();
                    transaction.Amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["TransactionAmount"]);
                    transaction.Tax = Convert.ToDecimal(ds.Tables[0].Rows[0]["TransactionTax"]);
                    transaction.IsCompleted = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCompleted"]);

                    if(ds.Tables[0].Rows[0]["ReceiptHeaderID"]!=DBNull.Value)
                        transaction.ReceiptHeaderID = Convert.ToInt32(ds.Tables[0].Rows[0]["ReceiptHeaderID"]);

                    transaction.FeeScheduleID = Convert.ToInt32(ds.Tables[0].Rows[0]["FeeScheduleID"]);
                }

            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return transaction;
        }

        public List<StudentPayOutTransaction> GetPayoutTransactionDetails(string TransactionNo, int FeeScheduleID=0)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            List<StudentPayOutTransaction> paymentlist = new();
            //StudentSubscriptionTransaction transaction = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@TransactionNo", TransactionNo);
                sqlparams[1] = new SqlParameter("@FeeScheduleID", FeeScheduleID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPayoutTransaction", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        StudentPayOutTransaction transaction = new();

                        transaction.PayoutTransactionID = Convert.ToInt32(ds.Tables[0].Rows[i]["PayoutTransactionID"]);
                        transaction.CustomerID= ds.Tables[0].Rows[i]["CustomerID"].ToString();
                        transaction.StudentID = ds.Tables[0].Rows[i]["StudentID"].ToString();
                        transaction.StudentDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Student_Detail_ID"]);
                        transaction.CenterID = Convert.ToInt32(ds.Tables[0].Rows[i]["CenterID"]);
                        transaction.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        transaction.TransactionMode = ds.Tables[0].Rows[i]["TransactionMode"].ToString();
                        transaction.TransactionSource = ds.Tables[0].Rows[i]["TransactionSource"].ToString();
                        transaction.TransactionDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TransactionDate"]);
                        transaction.TransactionStatus = ds.Tables[0].Rows[i]["TransactionStatus"].ToString();
                        transaction.Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransactionAmount"]);
                        transaction.Tax = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransactionTax"]);
                        transaction.IsCompleted = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCompleted"]);
                        transaction.TransactionNo= ds.Tables[0].Rows[i]["TransactionNo"].ToString();
                        transaction.FeeScheduleID= Convert.ToInt32(ds.Tables[0].Rows[i]["FeeScheduleID"]);

                        if (ds.Tables[0].Rows[i]["ReceiptHeaderID"] != DBNull.Value)
                            transaction.ReceiptHeaderID = Convert.ToInt32(ds.Tables[0].Rows[i]["ReceiptHeaderID"]);

                        paymentlist.Add(transaction);
                    }
                }

            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return paymentlist;
        }

        public int GetEnquiryDetails(int RegID, int CenterID, int BatchID, decimal ProspectusAmount)
        {
            DataHelper dh = new(_conn);
            int EnquiryID=0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@RegID", RegID);
                sqlparams[1] = new SqlParameter("@CenterID", CenterID);
                sqlparams[2] = new SqlParameter("@BatchID", BatchID);
                sqlparams[3] = new SqlParameter("@ProspectusAmount", ProspectusAmount);

                EnquiryID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspGetEnquiryDetails", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return EnquiryID;
        }

        public int uspInsertStudentDetailsFromEnquiry(int EnquiryID,string StudentCode,string CreatedBy, DataHelper dh, SqlTransaction trans)
        {
            DataSet ds = new();

            int StudentDetailID = 0;

            try
            {

                SqlParameter[] sqlparams = new SqlParameter[7];
                sqlparams[0] = new SqlParameter("@iEnquiryRegnID", EnquiryID);
                sqlparams[1] = new SqlParameter("@CrtdBy", CreatedBy);
                sqlparams[2] = new SqlParameter("@DtCrtdOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlparams[3] = new SqlParameter("sConductCode", DBNull.Value);
                sqlparams[4] = new SqlParameter("@sStudentCode", StudentCode);
                sqlparams[5] = new SqlParameter("@iRollNo", 0);
                sqlparams[6] = new SqlParameter("@sStudyMaterialNo", DBNull.Value);


                ds = dh.ExecuteDataSet("uspInsertStudentDetailsFromEnquiry", CommandType.StoredProcedure, trans, sqlparams);

                if (ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
                {
                    StudentDetailID = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentID"]);
                }
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return StudentDetailID;
        }

        public void InsertStudentLoginDetails(string LoginID, int UserID, SqlTransaction trans, DataHelper dh)
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@LoginID", LoginID);
                sqlparams[1] = new SqlParameter("@UserID", UserID);

                dh.ExecuteNonQuery("ECOMMERCE.uspInsertStudentLoginDetails", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void uspInsertStudentBatchDetails(string CourseXML,string CreatedBy, DataHelper dh, SqlTransaction trans)
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@sCoursesXMl", CourseXML);
                sqlparams[1] = new SqlParameter("@sCreatedBy", CreatedBy);
                sqlparams[2] = new SqlParameter("@dtCreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlparams[3] = new SqlParameter("@bIsFromPromoteBatch", 0);

                dh.ExecuteNonQuery("uspInsertStudentBatchDetails", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public int InsertInvoice(string InvoiceXML,int BrandID, DataHelper dh, SqlTransaction trans)
        {
            int FeeScheduleID = 0;
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@sInvoice", InvoiceXML);
                sqlparams[1] = new SqlParameter("@iBrandID", BrandID);
                sqlparams[2] = new SqlParameter("@iParentInvoiceID", DBNull.Value);

                FeeScheduleID=Convert.ToInt32(dh.ExecuteScalar("uspInsertInvoice", CommandType.StoredProcedure, trans, sqlparams));
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return FeeScheduleID;
        }

        public void GetInvoiceDetail(FeeSchedule objInvoice, DataHelper dh, SqlTransaction trans)
        {
            DataSet dsInvoice = null;
            try
            {
                SqlParameter[] arParameters = new SqlParameter[1];

                arParameters[0] = new SqlParameter("@iInvoiceHeaderID", objInvoice.InvoiceId);
                dsInvoice = dh.ExecuteDataSet("uspGetInvoiceDetail", CommandType.StoredProcedure, trans, arParameters);

                objInvoice.InvoiceChild = new List<InvoiceChild>();

                objInvoice.InvoiceNo = dsInvoice.Tables[0].Rows[0]["S_Invoice_No"].ToString();
                objInvoice.CenterID = Convert.ToInt32(dsInvoice.Tables[0].Rows[0]["I_Centre_Id"]);
                objInvoice.InvoiceAmount = Convert.ToDouble(dsInvoice.Tables[0].Rows[0]["N_Invoice_Amount"]);
                objInvoice.InvoiceDate = Convert.ToDateTime(dsInvoice.Tables[0].Rows[0]["Dt_Invoice_Date"]);
                objInvoice.TotalTaxAmount = Convert.ToDouble(dsInvoice.Tables[0].Rows[0]["N_Tax_Amount"]);

                //if (!UtilityFunctions.IsEmptyString(dsInvoice.Tables[0].Rows[0]["I_Coupon_Discount"]))
                //    objInvoice.CouponDiscount = Convert.ToInt32(dsInvoice.Tables[0].Rows[0]["I_Coupon_Discount"]);

                objInvoice.InvoiceNo = dsInvoice.Tables[0].Rows[0]["S_INVOICE_NO"].ToString();
                if (!String.IsNullOrEmpty(dsInvoice.Tables[0].Rows[0]["I_Discount_Scheme_ID"].ToString()) && dsInvoice.Tables[0].Rows[0]["I_Discount_Scheme_ID"].ToString() != "0")
                {
                    objInvoice.DiscountAmount = Convert.ToDouble(dsInvoice.Tables[0].Rows[0]["N_Discount_Amount"]);
                    objInvoice.DiscountSchemeId = Convert.ToInt32(dsInvoice.Tables[0].Rows[0]["I_Discount_Scheme_ID"].ToString());
                    objInvoice.DiscountAppliedAt = Convert.ToInt32(dsInvoice.Tables[0].Rows[0]["I_Discount_Applied_At"].ToString());
                }
                if (dsInvoice.Tables[0].Rows[0]["N_Discount_Amount"]!=DBNull.Value)
                {
                    objInvoice.DiscountAmount = Convert.ToDouble(dsInvoice.Tables[0].Rows[0]["N_Discount_Amount"]);
                }
                if (dsInvoice.Tables[0].Rows[0]["S_Parent_Invoice_No"] != DBNull.Value)
                    objInvoice.ParentInvoiceNumber = Convert.ToInt32(dsInvoice.Tables[0].Rows[0]["S_Parent_Invoice_No"].ToString());
                //if (dsInvoice.Tables[0].Rows[0]["I_Cancellation_Reason_ID"] != DBNull.Value)
                //    objInvoice.CancellationReason = (Enumerators.InvoiceCancellation)Convert.ToInt32(dsInvoice.Tables[0].Rows[0]["I_Cancellation_Reason_ID"].ToString());

                for (int i = 0; i < dsInvoice.Tables[1].Rows.Count; i++)
                {
                    InvoiceChild objInvoiceChild = new InvoiceChild();
                    //Set properties
                    objInvoiceChild.InvoiceChildID = Convert.ToInt32(dsInvoice.Tables[1].Rows[i]["I_Invoice_Child_Header_ID"]);
                    objInvoiceChild.InvoiceHeaderID = Convert.ToInt32(dsInvoice.Tables[1].Rows[i]["I_Invoice_Header_ID"]);

                    objInvoiceChild.CourseID = ((!string.IsNullOrEmpty(dsInvoice.Tables[1].Rows[i]["I_Course_ID"].ToString())) ? Convert.ToInt32(dsInvoice.Tables[1].Rows[i]["I_Course_ID"]) : 0);
                    //objInvoiceChild.Course.CourseName = dsInvoice.Tables[1].Rows[i]["S_Course_Name"].ToString();
                    //objInvoiceChild.Course.IsSTApplicable = dsInvoice.Tables[1].Rows[i]["I_Is_ST_Applicable"].ToString();
                    //objInvoiceChild.CourseFeePlan = new FeePlan();
                    objInvoiceChild.CourseFeePlanID= ((!string.IsNullOrEmpty(dsInvoice.Tables[1].Rows[i]["I_Course_FeePlan_ID"].ToString())) ? Convert.ToInt32(dsInvoice.Tables[1].Rows[i]["I_Course_FeePlan_ID"]) : 0);
                    objInvoiceChild.CourseAmount = ((!string.IsNullOrEmpty(dsInvoice.Tables[1].Rows[i]["N_Amount"].ToString())) ? Convert.ToDouble(dsInvoice.Tables[1].Rows[i]["N_Amount"]) : 0);
                    objInvoiceChild.CourseTaxAmount = ((!string.IsNullOrEmpty(dsInvoice.Tables[1].Rows[i]["N_Tax_Amount"].ToString())) ? Convert.ToDouble(dsInvoice.Tables[1].Rows[i]["N_Tax_Amount"]) : 0);
                    //objInvoiceChild.CourseAmount = Convert.ToDouble(dsInvoice.Tables[1].Rows[i]["N_Amount"]);
                    //objInvoiceChild.CourseTaxAmount = Convert.ToDouble(dsInvoice.Tables[1].Rows[i]["N_Tax_Amount"]);
                    objInvoiceChild.IsLumpSum = dsInvoice.Tables[1].Rows[i]["C_Is_lumpsum"].ToString();
                    objInvoiceChild.InvoiceChildDetails = new List<InvoiceChildDetails>();


                    if (!string.IsNullOrEmpty(dsInvoice.Tables[1].Rows[i]["I_Batch_ID"].ToString()))
                    {
                        //StudentBatch studentbatch = new StudentBatch();
                        //studentbatch.BatchID = Convert.ToInt32(dsInvoice.Tables[1].Rows[i]["I_Batch_ID"].ToString());
                        //studentbatch.BatchName = dsInvoice.Tables[1].Rows[i]["S_Batch_Name"].ToString();//akash
                        ////akash 18.12.2017
                        //studentbatch.BatchStartDate = Convert.ToDateTime(dsInvoice.Tables[1].Rows[i]["Dt_BatchStartDate"]);
                        //studentbatch.BatchIntroductionDate = Convert.ToDateTime(dsInvoice.Tables[1].Rows[i]["Dt_BatchIntroductionDate"]);
                        ////akash 18.12.2017
                        objInvoiceChild.BatchID = Convert.ToInt32(dsInvoice.Tables[1].Rows[i]["I_Batch_ID"]);
                    }

                    for (int j = 0; j < dsInvoice.Tables[2].Rows.Count; j++)
                    {
                        if (!dsInvoice.Tables[1].Rows[i]["I_Invoice_Child_Header_ID"].ToString().Equals(dsInvoice.Tables[2].Rows[j]["I_Invoice_Child_Header_ID"].ToString()))
                            continue;
                        InvoiceChildDetails objInvoiceChildDetails = new InvoiceChildDetails();
                        //Set properties
                        objInvoiceChildDetails.InvoiceChildDetailsId = Convert.ToInt32(dsInvoice.Tables[2].Rows[j]["I_Invoice_Detail_ID"]);
                        objInvoiceChildDetails.InvoiceChildHeaderId = Convert.ToInt32(dsInvoice.Tables[2].Rows[j]["I_Invoice_Child_Header_ID"]);
                        objInvoiceChildDetails.Sequence = Convert.ToInt32(dsInvoice.Tables[2].Rows[j]["I_Sequence"]);
                        if (dsInvoice.Tables[2].Rows[j]["N_Amount_Due"] != DBNull.Value)
                            objInvoiceChildDetails.AmountDue = Convert.ToDouble(dsInvoice.Tables[2].Rows[j]["N_Amount_Due"]);
                        if (dsInvoice.Tables[2].Rows[j]["N_Due"] != DBNull.Value)
                            objInvoiceChildDetails.PayableAmount = Convert.ToDouble(dsInvoice.Tables[2].Rows[j]["N_Due"]);
                        if (dsInvoice.Tables[2].Rows[j]["N_Discount_Amount"] != DBNull.Value)
                            objInvoiceChildDetails.DiscountAmount = Convert.ToDouble(dsInvoice.Tables[2].Rows[j]["N_Discount_Amount"]);
                        objInvoiceChildDetails.DueDate = Convert.ToDateTime(dsInvoice.Tables[2].Rows[j]["Dt_Installment_Date"]);
                        objInvoiceChildDetails.InstallmentNo = Convert.ToInt32(dsInvoice.Tables[2].Rows[j]["I_Installment_No"]);
                        objInvoiceChildDetails.FeeComponentId = Convert.ToInt32(dsInvoice.Tables[2].Rows[j]["I_Fee_Component_ID"]);
                        objInvoiceChildDetails.DisplayFeeComponentId = Convert.ToInt32(dsInvoice.Tables[2].Rows[j]["I_Display_Fee_Component_ID"]);
                        objInvoiceChildDetails.InvoiceTaxDetails = new List<InvoiceTaxDetails>();

                        for (int k = 0; k < dsInvoice.Tables[3].Rows.Count; k++)
                        {
                            if (dsInvoice.Tables[2].Rows[j]["I_Invoice_Detail_ID"].ToString().Equals(dsInvoice.Tables[3].Rows[k]["I_invoice_Detail_Id"].ToString()))
                            {
                                InvoiceTaxDetails objInvoiceTaxDetails = new InvoiceTaxDetails();

                                objInvoiceTaxDetails.TaxId = Convert.ToInt32(dsInvoice.Tables[3].Rows[k]["I_Tax_ID"]);
                                objInvoiceTaxDetails.InvoiceChildDetailId = Convert.ToInt32(dsInvoice.Tables[3].Rows[k]["I_invoice_Detail_Id"]);
                                objInvoiceTaxDetails.TaxValue = Convert.ToDouble(dsInvoice.Tables[3].Rows[k]["N_Tax_Value"]);
                                objInvoiceTaxDetails.TaxCode = dsInvoice.Tables[3].Rows[k]["TAX_CODE"].ToString();
                                if (!String.IsNullOrEmpty(dsInvoice.Tables[3].Rows[k]["TAX_DESC"].ToString()))
                                    objInvoiceTaxDetails.TaxDescription = dsInvoice.Tables[3].Rows[k]["TAX_DESC"].ToString();
                                objInvoiceTaxDetails.CGSTSGST = Convert.ToInt32(dsInvoice.Tables[3].Rows[k]["TAX_CHECK"].ToString());

                                objInvoiceChildDetails.InvoiceTaxDetails.Add((InvoiceTaxDetails)objInvoiceTaxDetails);
                            }
                        }
                        objInvoiceChild.InvoiceChildDetails.Add((InvoiceChildDetails)objInvoiceChildDetails);
                    }

                    objInvoice.InvoiceChild.Add((InvoiceChild)objInvoiceChild);
                }
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void GetReceipt(Receipt objReceipt, DataHelper dh, SqlTransaction trans)
        {
            if (dh == null)
                dh = new DataHelper(_conn);
            DataSet dsReceipt = null;

            try
            {
                SqlParameter[] arParameters = new SqlParameter[1];

                arParameters[0] = new SqlParameter("@iInvoiceHeaderID", objReceipt.Invoice.InvoiceId);
                dsReceipt = dh.ExecuteDataSet("uspGetReceipt", CommandType.StoredProcedure, trans, arParameters);


                objReceipt.ReceiptDetailList = new List<ReceiptDetails>();

                if (dsReceipt.Tables[0].Rows.Count > 0)
                {
                    objReceipt.ReceiptNo = dsReceipt.Tables[0].Rows[0]["S_Receipt_No"].ToString();
                }

                if (dsReceipt.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReceipt.Tables[1].Rows.Count; i++)
                    {
                        ReceiptDetails objReceiptDetails = new ReceiptDetails();
                        objReceiptDetails.ReceiptComponentDetailId = Convert.ToInt32(dsReceipt.Tables[1].Rows[i]["I_Receipt_Comp_Detail_ID"]);
                        objReceiptDetails.InvoiceDetailId = Convert.ToInt32(dsReceipt.Tables[1].Rows[i]["I_Invoice_Detail_ID"]);
                        objReceiptDetails.AmountPaid = Convert.ToDouble(dsReceipt.Tables[1].Rows[i]["N_Amount_Paid"]);
                        objReceiptDetails.ReceiptDetailsRff = Convert.ToDouble(dsReceipt.Tables[1].Rows[i]["N_Comp_Amount_Rff"]);
                        objReceiptDetails.ReceiptTaxDetails = new List<ReceiptTaxDetails>();

                        for (int j = 0; j < dsReceipt.Tables[2].Rows.Count; j++)
                        {
                            ReceiptTaxDetails objReceiptTaxDetails = new ReceiptTaxDetails();
                            if (dsReceipt.Tables[2].Rows[j]["I_Receipt_Comp_Detail_ID"].ToString().Equals(dsReceipt.Tables[1].Rows[i]["I_Receipt_Comp_Detail_ID"].ToString()))
                            {
                                objReceiptTaxDetails.TaxId = Convert.ToInt32(dsReceipt.Tables[2].Rows[j]["I_Tax_ID"]);
                                objReceiptTaxDetails.TaxPaid = Convert.ToDouble(dsReceipt.Tables[2].Rows[j]["N_Tax_paid"]);
                                objReceiptTaxDetails.ReceiptTaxRff = Convert.ToDouble(dsReceipt.Tables[2].Rows[j]["N_Tax_Rff"]);
                                objReceiptTaxDetails.ReceiptcomponentDetailId = Convert.ToInt32(dsReceipt.Tables[2].Rows[j]["I_Receipt_Comp_Detail_ID"]);

                                objReceiptDetails.ReceiptTaxDetails.Add((ReceiptTaxDetails)objReceiptTaxDetails);
                            }
                        }
                        objReceipt.ReceiptDetailList.Add((ReceiptDetails)objReceiptDetails);
                    }
                }
            }
            finally
            {
                if ((dh != null) && (trans == null))
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void InsertReceiptObject(Receipt objReceipt,int BrandID, DataHelper helper, SqlTransaction trans)
        {
            if (helper == null)
                helper = new DataHelper(_conn);
            SqlParameter[] arrParameters = new SqlParameter[21];

            try
            {
                if (objReceipt.ReceiptNo != null)
                    arrParameters[0] = new SqlParameter("@sReceiptNo", objReceipt.ReceiptNo);
                else
                    arrParameters[0] = new SqlParameter("@sReceiptNo", DBNull.Value);

                if (objReceipt.Invoice.InvoiceId != 0)
                    arrParameters[1] = new SqlParameter("@iInvoiceHeaderID", objReceipt.Invoice.InvoiceId);
                else
                    arrParameters[1] = new SqlParameter("@iInvoiceHeaderID", DBNull.Value);

                arrParameters[2] = new SqlParameter("@dReceiptDate", objReceipt.ReceiptDate);
                arrParameters[3] = new SqlParameter("@iStudentDetailID", objReceipt.StudentDetailID);
                arrParameters[4] = new SqlParameter("@iPaymentModeID", objReceipt.PaymentModeID);

                if (objReceipt.CenterId>0)
                    arrParameters[5] = new SqlParameter("@iCentreId", objReceipt.CenterId);
                else
                    arrParameters[5] = new SqlParameter("@iCentreId", DBNull.Value);

                arrParameters[6] = new SqlParameter("@nReceiptAmount", objReceipt.ReceiptAmount);

                if (objReceipt.FundTransferStatus != null)
                    arrParameters[7] = new SqlParameter("@sFundTransferStatus", objReceipt.FundTransferStatus);
                else
                    arrParameters[7] = new SqlParameter("@sFundTransferStatus", DBNull.Value);

                arrParameters[8] = new SqlParameter("@sCrtdBy", objReceipt.CreatedBy);
                arrParameters[9] = new SqlParameter("@dCreatedOn", DateTime.Now);

                arrParameters[10] = new SqlParameter("@nCreditCardNo", DBNull.Value);
                arrParameters[11] = new SqlParameter("@dCreditCardExpiry", DBNull.Value);
                arrParameters[12] = new SqlParameter("@sCreditCardIssuer", DBNull.Value);


                if (objReceipt.PaymentModeID == 2 || objReceipt.PaymentModeID == 3 || objReceipt.PaymentModeID == 27)
                {
                    arrParameters[13] = new SqlParameter("@sChequeDDNo", objReceipt.ChequeDDNo);
                    arrParameters[14] = new SqlParameter("@dChequeDDDate", objReceipt.ChequeDDDate);
                }
                else
                {
                    arrParameters[13] = new SqlParameter("@sChequeDDNo", DBNull.Value);
                    arrParameters[14] = new SqlParameter("@dChequeDDDate", DBNull.Value);
                }

                if (objReceipt.BankName != null)
                    arrParameters[15] = new SqlParameter("@sBankName", objReceipt.BankName);
                else
                    arrParameters[15] = new SqlParameter("@sBankName", DBNull.Value);

                if (objReceipt.BranchName != null)
                    arrParameters[16] = new SqlParameter("@sBranchName", objReceipt.BranchName);
                else
                    arrParameters[16] = new SqlParameter("@sBranchName", DBNull.Value);

                arrParameters[17] = new SqlParameter("@iReceiptType", Convert.ToInt32(objReceipt.ReceiptType));

                //Calculate the Total Tax for the Receipt             
                double dReceiptTax = 0.0;
                foreach (ReceiptDetails objReceiptDetails in objReceipt.ReceiptDetailList)
                {
                    // If Tax Details exists for the ReceiptDetails
                    if (objReceiptDetails.ReceiptTaxDetails != null && objReceiptDetails.IsModified == true)
                    {
                        foreach (ReceiptTaxDetails objReceiptTaxDetails in objReceiptDetails.ReceiptTaxDetails)
                        {
                            dReceiptTax += objReceiptTaxDetails.TaxPaid;
                        }
                    }
                }
                arrParameters[18] = new SqlParameter("@nReceiptTaxAmount", dReceiptTax);
                arrParameters[19] = new SqlParameter("@iBrandID", BrandID);
                arrParameters[20] = new SqlParameter("@sNarration", objReceipt.Narration);
                DataSet ds = helper.ExecuteDataSet("uspInsertReceiptHeader", CommandType.StoredProcedure, trans, arrParameters);
                objReceipt.ReceiptId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            finally
            {
                if (helper != null && trans == null)
                {
                    helper.DataConn.Close();
                }
            }
        }

        public void UpdateReceiptDetail(Receipt objReceipt, DataHelper dh, SqlTransaction trans)
        {
            if (dh == null)
                dh = new DataHelper(_conn);
            string sKeyValueXml = string.Empty;
            SqlParameter[] sqlparams = new SqlParameter[1];
            string sReceiptDetail = objReceipt.GenerateReceiptDetailXML();

            try
            {
                sqlparams[0] = new SqlParameter("@sReceiptDetail", sReceiptDetail);
                dh.ExecuteScalar("uspInsertReceiptDetails", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    dh.DataConn.Close();
                }
            }
        }

        public List<FeeStructure> GetFeePlanDetails(int FeePlanID, int PaymentModeID, int DiscountSchemeID, DateTime BatchStartDate)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<FeeStructure> fees = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@feeplanid", FeePlanID);
                sqlparams[1] = new SqlParameter("@paymentmode", PaymentModeID);
                sqlparams[2] = new SqlParameter("@discountschemeid", DiscountSchemeID);
                sqlparams[3] = new SqlParameter("@batchstartdate", BatchStartDate);

                ds = dh.ExecuteDataSet("uspGetFeePlanDiscountDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FeeStructure fee = new();

                        fee.CourseFeePlanDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Course_Fee_Plan_Detail_ID"]);
                        fee.FeeComponentID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Fee_Component_ID"]);
                        fee.FeePlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Course_Fee_Plan_ID"]);
                        fee.InstalmentNo = Convert.ToInt32(ds.Tables[0].Rows[i]["ActualInstalmentNo"]);
                        fee.InstalmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InstalmentDate"]);
                        fee.Sequence = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Sequence"]);
                        fee.DisplayFeeComponentID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Display_Fee_Component_ID"]);
                        fee.Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["I_Item_Value"]);
                        fee.DiscountAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["N_Discount"]);
                        fee.IsLumpsum = ds.Tables[0].Rows[i]["C_Is_LumpSum"].ToString();

                        for(int j=0;j<ds.Tables[1].Rows.Count;j++)
                        {
                            FeeStructureTax tax = null;

                            if(fee.CourseFeePlanDetailID== Convert.ToInt32(ds.Tables[1].Rows[j]["I_Course_Fee_Plan_Detail_ID"]))
                            {
                                tax = new();

                                tax.TaxID = Convert.ToInt32(ds.Tables[1].Rows[j]["TaxID"]);
                                tax.TaxAmount= Convert.ToDecimal(ds.Tables[1].Rows[j]["TaxAmount"]);

                                if (tax != null)
                                    fee.TaxDetails.Add(tax);
                            }
                        }

                        fees.Add(fee);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return fees;
        }

        public int GetRollNo(string format)
        {
            DataSet ds = null;
            int RollNo = 0;
            DataHelper dh = new DataHelper(_conn);
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@format", format);

                ds = dh.ExecuteDataSet("[dbo].[usGetRollNo]", CommandType.StoredProcedure, sqlparams);
                if (ds != null)
                {
                    RollNo = ds.Tables[0].Rows[0]["RollNo"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0][0]) : 0;
                }

            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
            return RollNo;
        }

        public int VerifyAdmissionType(int EnquiryID)
        {
            DataHelper dh = new(_conn);
            int StudentDetailID = 0;

            try
            {
                StudentDetailID = Convert.ToInt32(dh.ExecuteScalar("select ISNULL(I_Student_Detail_ID,0) from T_Student_Detail where I_Enquiry_Regn_ID=" + EnquiryID.ToString(), CommandType.Text));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return StudentDetailID;
        }

        public void UpdateTransactionDetails(int TransactionProductDetailID, int FeeScheduleID, int StudentID, int ReceiptID, DataHelper dh, SqlTransaction trans)
        {

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@TransactionProductDetailID", TransactionProductDetailID);
                sqlparams[1] = new SqlParameter("@FeeScheduleID", FeeScheduleID);

                if (StudentID == 0)
                    sqlparams[2] = new SqlParameter("@StudentDetailID", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@StudentDetailID", StudentID);

                sqlparams[3] = new SqlParameter("@ReceiptHeaderID", ReceiptID);

                dh.ExecuteNonQuery("ECOMMERCE.uspUpdateTransctionDetails", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void UpdateSubscriptionTransactionDetails(int SubscriptionTransactionDetailID, int ReceiptID,string TransactionStatus, DataHelper dh, SqlTransaction trans)
        {

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@SubscriptionTransactionDetailID", SubscriptionTransactionDetailID);
                sqlparams[1] = new SqlParameter("@ReceiptHeaderID", ReceiptID);
                sqlparams[2] = new SqlParameter("@TransactionStatus", TransactionStatus);

                dh.ExecuteNonQuery("ECOMMERCE.uspUpdateSubscriptionTransctionDetails", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void UpdatePayoutTransactionDetails(int PayoutTransactionID, int ReceiptID, string TransactionStatus, DataHelper dh, SqlTransaction trans)
        {

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@PayoutTransactionID", PayoutTransactionID);
                sqlparams[1] = new SqlParameter("@ReceiptHeaderID", ReceiptID);
                sqlparams[2] = new SqlParameter("@TransactionStatus", TransactionStatus);

                dh.ExecuteNonQuery("ECOMMERCE.uspUpdatePayoutTransctionDetails", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public List<Batch> GetBatches(int ProductID, int CenterID, DateTime AfterDate)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            List<Batch> batches = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@ProductID", ProductID);
                sqlparams[1] = new SqlParameter("@CenterID", CenterID);
                sqlparams[2] = new SqlParameter("@AfterDate", AfterDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetBatchListForProduct", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Batch batch = new();

                        batch.BatchID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Batch_ID"]);
                        batch.BatchName = ds.Tables[0].Rows[i]["S_Batch_Name"].ToString();

                        //if (Convert.ToBoolean(ds.Tables[0].Rows[i]["Admission_AfterStartDate"]) == false || DateTime.Compare(Convert.ToDateTime(ds.Tables[0].Rows[i]["Dt_BatchStartDate"]).Date, AfterDate.Date)  > 0)
                        //    {
                        //    batch.BatchStartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dt_BatchStartDate"]);
                        //    }
                        //else
                        //{
                        //    batch.BatchStartDate = DateTime.Now.AddDays(1);
                        //}
                        batch.BatchStartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dt_BatchStartDate"]);
                        batch.OriginalBatchStartDate= Convert.ToDateTime(ds.Tables[0].Rows[i]["Dt_BatchStartDate"]);
                        if (ds.Tables[0].Rows[i]["S_OnlineClassTime"] != DBNull.Value)
                            batch.OnlineTimeSlots = ds.Tables[0].Rows[i]["S_OnlineClassTime"].ToString();

                        if (ds.Tables[0].Rows[i]["S_BatchTime"] != DBNull.Value)
                            batch.BatchTime = ds.Tables[0].Rows[i]["S_BatchTime"].ToString();
                        else
                            batch.BatchTime = "";

                        batch.BatchCode= ds.Tables[0].Rows[i]["S_Batch_Code"].ToString(); //susmita 2022-09-21

                        batch.AdmissionAfterStartDate = Convert.ToBoolean(ds.Tables[0].Rows[i]["Admission_AfterStartDate"]);

                        batches.Add(batch);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return batches;
        }

        public DateTime FindNxtInstalmentDate(int receiptid)
        {
            DataHelper dh = null;
            DataSet ds = null;

            DateTime NxtInstalmentDate = DateTime.Now;

            try
            {
                if (dh == null)
                {
                    dh = new DataHelper(_conn);
                    SqlParameter[] sqlparams = new SqlParameter[1];
                    sqlparams[0] = new SqlParameter("@receiptid", receiptid);
                    ds = dh.ExecuteDataSet("uspGetNextInstalmentDateForReceipt", CommandType.StoredProcedure, sqlparams);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            NxtInstalmentDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["InstalmentDate"]);

                        }
                    }
                }
            }
            //catch (Exception ex)
            //{
            //}
            finally
            {
                if (dh != null)
                {
                    dh.DataConn.Close();
                }
            }
            return NxtInstalmentDate;
        }

        public List<StudentSubscription> GetStudentSubscriptionDues(DateTime DueDate)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<StudentSubscription> studentList = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@DueDate", DueDate);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetStudentSubscriptionPayableDetails", CommandType.StoredProcedure, sqlparams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        StudentSubscription student = new();

                        student.SubscriptionID = Convert.ToInt32(ds.Tables[0].Rows[k]["SubscriptionID"]);
                        student.CustomerID = ds.Tables[0].Rows[k]["CustomerID"].ToString();
                        student.AuthKey = ds.Tables[0].Rows[k]["AuthKey"].ToString();
                        student.StudentID = ds.Tables[0].Rows[k]["S_Student_ID"].ToString();
                        student.MobileNo = ds.Tables[0].Rows[k]["MobileNo"].ToString();

                        if(ds.Tables[0].Rows[k]["EmailID"]!=DBNull.Value)
                            student.EmailID = ds.Tables[0].Rows[k]["EmailID"].ToString();

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (
                                    (Convert.ToInt32(ds.Tables[1].Rows[i]["SubscriptionID"]) == student.SubscriptionID)
                                    &&
                                    (ds.Tables[1].Rows[i]["S_Student_ID"].ToString()==student.StudentID)
                               )
                            {
                                StudentSubscriptionFeeSchedule feeSchedule = new();

                                feeSchedule.FeeScheduleID = Convert.ToInt32(ds.Tables[1].Rows[i]["FeeScheduleID"]);
                                feeSchedule.FeeScheduleDate = Convert.ToDateTime(ds.Tables[1].Rows[i]["FeeScheduleCreatedOn"]);
                                feeSchedule.CourseName = ds.Tables[1].Rows[i]["S_Course_Name"].ToString();

                                for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(ds.Tables[2].Rows[j]["FeeScheduleID"]) == feeSchedule.FeeScheduleID)
                                    {
                                        StudentSubscriptionDue invoiceDetail = new();

                                        invoiceDetail.InvoiceDetailID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Invoice_Detail_ID"]);
                                        invoiceDetail.InvoiceNo = ds.Tables[2].Rows[j]["S_Invoice_No"].ToString();
                                        invoiceDetail.InstallmentNo = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Installment_No"]);
                                        invoiceDetail.InstallmentDate = Convert.ToDateTime(ds.Tables[2].Rows[j]["Dt_Installment_Date"]);
                                        invoiceDetail.Sequence = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Sequence"]);
                                        invoiceDetail.FeeComponentID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_FeeComponent_ID"]);
                                        invoiceDetail.FeeComponentName = ds.Tables[2].Rows[j]["S_Component_Name"].ToString();
                                        invoiceDetail.InitialAmountDue = Convert.ToDecimal(ds.Tables[2].Rows[j]["N_Amount_Due"]);
                                        invoiceDetail.InitialTaxDue = Convert.ToDecimal(ds.Tables[2].Rows[j]["TotalTax"]);
                                        invoiceDetail.AmountPaid = Convert.ToDecimal(ds.Tables[2].Rows[j]["BaseAmountPaid"]);
                                        invoiceDetail.TaxPaid = Convert.ToDecimal(ds.Tables[2].Rows[j]["TaxPaid"]);
                                        invoiceDetail.CreditNoteAmount = Convert.ToDecimal(ds.Tables[2].Rows[j]["CreditNoteAmt"]);
                                        invoiceDetail.CreditNoteTax = Convert.ToDecimal(ds.Tables[2].Rows[j]["CreditNoteTax"]);


                                        feeSchedule.DueDetails.Add(invoiceDetail);
                                    }
                                }

                                if(feeSchedule.DueDetails.Count>0)
                                    student.FeeSchedules.Add(feeSchedule);
                            }
                        }

                        studentList.Add(student);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return studentList;
        }

        public StudentPayOut GetStudentPayoutDues(string CustomerID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            StudentPayOut student = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetStudentPayoutPayableDetails", CommandType.StoredProcedure, sqlparams);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    student.CustomerID= ds.Tables[0].Rows[0]["CustomerID"].ToString();

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        if (student.CustomerID == ds.Tables[1].Rows[i]["CustomerID"].ToString())
                        {
                            StudentPayOutFeeSchedule feeSchedule = new();

                            feeSchedule.PlanID = Convert.ToInt32(ds.Tables[1].Rows[i]["PlanID"]);
                            feeSchedule.ProductID = Convert.ToInt32(ds.Tables[1].Rows[i]["ProductID"]);
                            feeSchedule.TransactionNo = ds.Tables[1].Rows[i]["TransactionNo"].ToString();
                            feeSchedule.StudentID = ds.Tables[1].Rows[i]["S_Student_ID"].ToString();
                            feeSchedule.FeeScheduleID = Convert.ToInt32(ds.Tables[1].Rows[i]["FeeScheduleID"]);
                            feeSchedule.FeeScheduleDate = Convert.ToDateTime(ds.Tables[1].Rows[i]["FeeScheduleCreatedOn"]);
                            feeSchedule.CourseName = ds.Tables[1].Rows[i]["S_Course_Name"].ToString();

                            for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            {
                                if (Convert.ToInt32(ds.Tables[2].Rows[j]["FeeScheduleID"]) == feeSchedule.FeeScheduleID)
                                {
                                    StudentSubscriptionDue invoiceDetail = new();

                                    invoiceDetail.InvoiceDetailID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Invoice_Detail_ID"]);
                                    invoiceDetail.InvoiceNo = ds.Tables[2].Rows[j]["S_Invoice_No"].ToString();
                                    invoiceDetail.InstallmentNo = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Installment_No"]);
                                    invoiceDetail.InstallmentDate = Convert.ToDateTime(ds.Tables[2].Rows[j]["Dt_Installment_Date"]);
                                    invoiceDetail.Sequence = Convert.ToInt32(ds.Tables[2].Rows[j]["I_Sequence"]);
                                    invoiceDetail.FeeComponentID = Convert.ToInt32(ds.Tables[2].Rows[j]["I_FeeComponent_ID"]);
                                    invoiceDetail.FeeComponentName = ds.Tables[2].Rows[j]["S_Component_Name"].ToString();
                                    invoiceDetail.InitialAmountDue = Convert.ToDecimal(ds.Tables[2].Rows[j]["N_Amount_Due"]);
                                    invoiceDetail.InitialTaxDue = Convert.ToDecimal(ds.Tables[2].Rows[j]["TotalTax"]);
                                    invoiceDetail.AmountPaid = Convert.ToDecimal(ds.Tables[2].Rows[j]["BaseAmountPaid"]);
                                    invoiceDetail.TaxPaid = Convert.ToDecimal(ds.Tables[2].Rows[j]["TaxPaid"]);
                                    invoiceDetail.CreditNoteAmount = Convert.ToDecimal(ds.Tables[2].Rows[j]["CreditNoteAmt"]);
                                    invoiceDetail.CreditNoteTax = Convert.ToDecimal(ds.Tables[2].Rows[j]["CreditNoteTax"]);


                                    feeSchedule.DueDetails.Add(invoiceDetail);
                                }
                            }

                            if(feeSchedule.DueDetails!=null && feeSchedule.DueDetails.Count>0)
                                student.FeeSchedules.Add(feeSchedule);
                        }
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return student;
        }

        public int ValidateSubscriptionPayment(int SubscriptionID, decimal PaidAmount, decimal PaidTax, DateTime DueDate, 
                                                    int FeeSchID,string TransactionNo=null)
        {
            DataHelper dh = new(_conn);
            int FeeScheduleID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[6];

                sqlparams[0] = new SqlParameter("@SubscriptionID", SubscriptionID);
                sqlparams[1] = new SqlParameter("@PaidAmount", PaidAmount);
                sqlparams[2] = new SqlParameter("@PaidTax", PaidTax);

                if (TransactionNo == null)
                    sqlparams[3] = new SqlParameter("@TransactionNo", DBNull.Value);
                else
                    sqlparams[3] = new SqlParameter("@TransactionNo", TransactionNo);

                if (DueDate == DateTime.MinValue)
                    sqlparams[4] = new SqlParameter("@DueDate", DBNull.Value);
                else
                    sqlparams[4] = new SqlParameter("@DueDate", DueDate);

                sqlparams[5] = new SqlParameter("@FeeSchID", FeeSchID);

                FeeScheduleID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspValidateSubscriptionPayment", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return FeeScheduleID;
        }

        public int ValidatePayOutPayment(int FeeScheduleID, decimal PaidAmount, decimal PaidTax)
        {
            DataHelper dh = new(_conn);
            int InvID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];

                sqlparams[0] = new SqlParameter("@FeeScheduleID", FeeScheduleID);
                sqlparams[1] = new SqlParameter("@PaidAmount", PaidAmount);
                sqlparams[2] = new SqlParameter("@PaidTax", PaidTax);

                InvID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspValidatePayOutPayment", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return InvID;
        }

        public int SaveNewSubscription(string TransactionNo,int PlanID,int ProductID, string SubscriptionPlanID, string AuthKey, int BillingPeriod,
                                        DateTime BillingStartDate, DateTime BillingEndDate, decimal BillingAmount,string SubscriptionStatus,
                                        string MandateLink)
        {
            DataHelper dh = new(_conn);
            int SubscriptionID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[11];


                sqlparams[0] = new SqlParameter("@TransactionNo", TransactionNo);
                sqlparams[1] = new SqlParameter("@PlanID", PlanID);
                sqlparams[2] = new SqlParameter("@ProductID", ProductID);

                if (SubscriptionPlanID!=null && SubscriptionPlanID!="")
                    sqlparams[3] = new SqlParameter("@SubscriptionPlanID", SubscriptionPlanID);
                else
                    sqlparams[3] = new SqlParameter("@SubscriptionPlanID", DBNull.Value);

                if(AuthKey!=null && AuthKey!="")
                    sqlparams[4] = new SqlParameter("@AuthKey", AuthKey);
                else
                    sqlparams[4] = new SqlParameter("@AuthKey", DBNull.Value);

                if(BillingPeriod>0)
                    sqlparams[5] = new SqlParameter("@BillingPeriod", BillingPeriod);
                else
                    sqlparams[5] = new SqlParameter("@BillingPeriod", DBNull.Value);

                if (BillingStartDate==DateTime.MinValue)
                    sqlparams[6] = new SqlParameter("@BillingStartDate", DBNull.Value);
                else
                    sqlparams[6] = new SqlParameter("@BillingStartDate", BillingStartDate);

                if (BillingEndDate == DateTime.MinValue)
                    sqlparams[7] = new SqlParameter("@BillingEndDate", DBNull.Value);
                else
                    sqlparams[7] = new SqlParameter("@BillingEndDate", BillingEndDate);

                if(BillingAmount>0)
                    sqlparams[8] = new SqlParameter("@BillingAmount", BillingAmount);
                else
                    sqlparams[8] = new SqlParameter("@BillingAmount", DBNull.Value);

                sqlparams[9] = new SqlParameter("@SubscriptionStatus", SubscriptionStatus);

                if(MandateLink!=null && MandateLink!="")
                    sqlparams[10] = new SqlParameter("@SubscriptionLink", MandateLink);
                else
                    sqlparams[10] = new SqlParameter("@SubscriptionLink", DBNull.Value);

                SubscriptionID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSaveNewSubscriptionDetails", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return SubscriptionID;
        }

        public PrintFeeSchedule GetPrintFeeScheduleDetails(int FeeScheduleID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            PrintFeeSchedule print = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@FeeScheduleID", FeeScheduleID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPrintFeeScheduleDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Brand brand = new();
                    Centre centre = new();

                    brand.BrandID = Convert.ToInt32(ds.Tables[0].Rows[0]["I_Brand_ID"]);
                    brand.BrandName = ds.Tables[0].Rows[0]["S_Brand_Name"].ToString();

                    centre.CenterID = Convert.ToInt32(ds.Tables[0].Rows[0]["I_Center_ID"]);
                    centre.CenterName = ds.Tables[0].Rows[0]["S_Center_Name"].ToString();
                    centre.Address = ds.Tables[0].Rows[0]["S_Center_Address1"].ToString();
                    centre.City = ds.Tables[0].Rows[0]["S_City_Name"].ToString();
                    centre.State = ds.Tables[0].Rows[0]["S_State_Name"].ToString();
                    centre.Country = ds.Tables[0].Rows[0]["S_Country_Name"].ToString();
                    centre.Pincode = ds.Tables[0].Rows[0]["S_Pin_Code"].ToString();
                    centre.EmailID = ds.Tables[0].Rows[0]["S_Email_ID"].ToString();
                    centre.ContactNo = ds.Tables[0].Rows[0]["S_Telephone_No"].ToString();
                    centre.GSTINNo = ds.Tables[0].Rows[0]["S_GST_Code"].ToString();
                    centre.StateCode = ds.Tables[0].Rows[0]["S_State_Code"].ToString();
                    centre.CIN= ds.Tables[0].Rows[0]["CIN"].ToString();
                    centre.PAN = ds.Tables[0].Rows[0]["PAN"].ToString();

                    print.BrandDetails = brand;
                    print.CenterDetails = centre;
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    Student student = new();

                    student.StudentDetailID = Convert.ToInt32(ds.Tables[1].Rows[0]["I_Student_Detail_ID"]);
                    student.StudentID = ds.Tables[1].Rows[0]["S_Student_ID"].ToString();
                    student.StudentName = ds.Tables[1].Rows[0]["StudentName"].ToString();
                    student.Address = ds.Tables[1].Rows[0]["S_Curr_Address1"].ToString();
                    student.City = ds.Tables[1].Rows[0]["S_City_Name"].ToString();
                    student.State = ds.Tables[1].Rows[0]["S_State_Name"].ToString();
                    student.Country = ds.Tables[1].Rows[0]["S_Country_Name"].ToString();
                    student.Pincode = ds.Tables[1].Rows[0]["S_Curr_Pincode"].ToString();
                    student.CustomerID = ds.Tables[1].Rows[0]["CustomerID"].ToString();
                    student.EmailID = ds.Tables[1].Rows[0]["S_Email_ID"].ToString();
                    student.ContactNo = ds.Tables[1].Rows[0]["S_Mobile_No"].ToString();

                    print.StudentDetails = student;
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    print.BatchName = ds.Tables[2].Rows[0]["S_Batch_Name"].ToString();
                    print.InvoiceNo = ds.Tables[2].Rows[0]["InvoiceNo"].ToString();
                    print.FeeScheduleDate = Convert.ToDateTime(ds.Tables[2].Rows[0]["FeeScheduleDate"]);

                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    InvoiceSummary summary = new();

                    summary.CourseName = ds.Tables[3].Rows[0]["S_Course_Name"].ToString();
                    summary.SACCode = ds.Tables[3].Rows[0]["S_SAC_Code"].ToString();
                    summary.CourseAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["Course_Amt"]);
                    summary.DiscountAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["Discount"]);
                    summary.OldTax = Convert.ToDecimal(ds.Tables[3].Rows[0]["OLD_TAX"]);
                    summary.SGSTTax = Convert.ToDecimal(ds.Tables[3].Rows[0]["S_SGST_Tax"]);
                    summary.CGSTTax = Convert.ToDecimal(ds.Tables[3].Rows[0]["S_CGST_Tax"]);
                    summary.IGSTTax = Convert.ToDecimal(ds.Tables[3].Rows[0]["S_IGST_Tax"]);
                    summary.TaxAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["TaxAmt"]);
                    summary.TotalAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["Total"]);

                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        InvoiceDetailSummary invoiceDetail = new();

                        invoiceDetail.InvoiceHeaderID = Convert.ToInt32(ds.Tables[4].Rows[i]["I_Invoice_Header_ID"]);
                        invoiceDetail.InstalmentNo = Convert.ToInt32(ds.Tables[4].Rows[i]["I_Installment_No"]);
                        invoiceDetail.InstalmentDate = Convert.ToDateTime(ds.Tables[4].Rows[i]["Dt_Installment_Date"]);
                        invoiceDetail.AmountDue = Convert.ToDecimal(ds.Tables[4].Rows[i]["DueAmount"]);
                        invoiceDetail.DiscountAmount = Convert.ToDecimal(ds.Tables[4].Rows[i]["DiscountAmount"]);
                        invoiceDetail.AdvanceAmount = Convert.ToDecimal(ds.Tables[4].Rows[i]["AdvanceAmount"]);
                        invoiceDetail.NetAmount = Convert.ToDecimal(ds.Tables[4].Rows[i]["NetAmount"]);
                        invoiceDetail.InvoiceNumber = ds.Tables[4].Rows[i]["S_Invoice_Number"].ToString();
                        invoiceDetail.InvoiceTYpe = ds.Tables[4].Rows[i]["Invoice_Type"].ToString();
                        invoiceDetail.CGST= Convert.ToDecimal(ds.Tables[4].Rows[i]["CGST"]);
                        invoiceDetail.SGST= Convert.ToDecimal(ds.Tables[4].Rows[i]["SGST"]);

                        summary.InstalmentDetails.Add(invoiceDetail);
                    }

                    print.FeeScheduleSummary = summary;
                }

                if(ds.Tables[5].Rows.Count>0)
                {
                    print.PlanName = ds.Tables[5].Rows[0]["PlanName"].ToString();
                    print.ProductName = ds.Tables[5].Rows[0]["ProductName"].ToString();
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return print;
        }

        public PrintTaxInvoice GetPrintTaxInvoiceDetails(string InvoiceNo,int ReceiptDetailID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            PrintTaxInvoice print = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@InvoiceNo", InvoiceNo);
                sqlparams[1] = new SqlParameter("@ReceiptID", ReceiptDetailID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPrintTaxInvoiceDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Brand brand = new();
                    Centre centre = new();

                    brand.BrandID = Convert.ToInt32(ds.Tables[0].Rows[0]["I_Brand_ID"]);
                    brand.BrandName = ds.Tables[0].Rows[0]["S_Brand_Name"].ToString();

                    centre.CenterID = Convert.ToInt32(ds.Tables[0].Rows[0]["I_Center_ID"]);
                    centre.CenterName = ds.Tables[0].Rows[0]["S_Center_Name"].ToString();
                    centre.Address = ds.Tables[0].Rows[0]["S_Center_Address1"].ToString();
                    centre.City = ds.Tables[0].Rows[0]["S_City_Name"].ToString();
                    centre.State = ds.Tables[0].Rows[0]["S_State_Name"].ToString();
                    centre.Country = ds.Tables[0].Rows[0]["S_Country_Name"].ToString();
                    centre.Pincode = ds.Tables[0].Rows[0]["S_Pin_Code"].ToString();
                    centre.EmailID = ds.Tables[0].Rows[0]["S_Email_ID"].ToString();
                    centre.ContactNo = ds.Tables[0].Rows[0]["S_Telephone_No"].ToString();
                    centre.GSTINNo = ds.Tables[0].Rows[0]["S_GST_Code"].ToString();
                    centre.StateCode = ds.Tables[0].Rows[0]["S_State_Code"].ToString();
                    centre.CIN = ds.Tables[0].Rows[0]["CIN"].ToString();
                    centre.PAN = ds.Tables[0].Rows[0]["PAN"].ToString();

                    print.BrandDetails = brand;
                    print.CenterDetails = centre;
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    Student student = new();

                    student.StudentDetailID = Convert.ToInt32(ds.Tables[1].Rows[0]["I_Student_Detail_ID"]);
                    student.StudentID = ds.Tables[1].Rows[0]["S_Student_ID"].ToString();
                    student.StudentName = ds.Tables[1].Rows[0]["StudentName"].ToString();
                    student.Address = ds.Tables[1].Rows[0]["S_Curr_Address1"].ToString();
                    student.City = ds.Tables[1].Rows[0]["S_City_Name"].ToString();
                    student.State = ds.Tables[1].Rows[0]["S_State_Name"].ToString();
                    student.Country = ds.Tables[1].Rows[0]["S_Country_Name"].ToString();
                    student.Pincode = ds.Tables[1].Rows[0]["S_Curr_Pincode"].ToString();
                    student.CustomerID = ds.Tables[1].Rows[0]["CustomerID"].ToString();
                    student.EmailID = ds.Tables[1].Rows[0]["S_Email_ID"].ToString();
                    student.ContactNo = ds.Tables[1].Rows[0]["S_Mobile_No"].ToString();

                    print.StudentDetails = student;
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    print.BatchName = ds.Tables[2].Rows[0]["S_Batch_Name"].ToString();
                    print.InvoiceNo = ds.Tables[2].Rows[0]["InvoiceNo"].ToString();
                    print.FeeScheduleDate = Convert.ToDateTime(ds.Tables[2].Rows[0]["FeeScheduleDate"]);

                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    InvoiceDetailSummary invoiceDetail = new();

                    invoiceDetail.InvoiceHeaderID = Convert.ToInt32(ds.Tables[3].Rows[0]["FeeScheduleID"]);
                    invoiceDetail.InstalmentNo = Convert.ToInt32(ds.Tables[3].Rows[0]["I_Installment_No"]);
                    invoiceDetail.InstalmentDate = Convert.ToDateTime(ds.Tables[3].Rows[0]["Dt_Installment_Date"]);
                    invoiceDetail.AmountDue = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_Amount_Due"]);
                    invoiceDetail.DiscountAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_Discount_Amount"]);
                    invoiceDetail.AdvanceAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_Advance_Amount"]);
                    invoiceDetail.NetAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_Net_Amount"]);
                    invoiceDetail.TotalAmount = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_Total_Amount"]);
                    invoiceDetail.CGST = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_CGST_Amount"]);
                    invoiceDetail.SGST = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_SGST_Amount"]);
                    invoiceDetail.IGST = Convert.ToDecimal(ds.Tables[3].Rows[0]["N_IGST_Amount"]);
                    invoiceDetail.InvoiceNumber = ds.Tables[3].Rows[0]["InvoiceNo"].ToString();
                    invoiceDetail.InvoiceTYpe = ds.Tables[3].Rows[0]["InvoiceType"].ToString();

                    print.InstalmentDetails = invoiceDetail;
                }

                if (ds.Tables[4].Rows.Count > 0)
                {
                    print.PlanName = ds.Tables[4].Rows[0]["PlanName"].ToString();
                    print.ProductName = ds.Tables[4].Rows[0]["ProductName"].ToString();
                    print.PaymentMode= ds.Tables[4].Rows[0]["PaymentMode"].ToString();
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return print;
        }


        public void UpdateTransactionStatus(string TransactionNo, string TransactionStatus)
        {
            DataHelper dh = new(_conn);
            //int c = 0;

            try
            {
                dh.ExecuteNonQuery("update ECOMMERCE.T_Transaction_Master set TransactionStatus=" + TransactionStatus + " where TransactionNo=" + TransactionNo, CommandType.Text);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            //return c;
        }

        public void SaveLogs(RequestLog log)
        {
            DataHelper dh = new(_conn);

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[7];

                if(log.InvokedRoute!=null)
                    sqlparams[0] = new SqlParameter("@InvokedRoute", log.InvokedRoute);
                else
                    sqlparams[0] = new SqlParameter("@InvokedRoute", DBNull.Value);

                if (log.InvokedMethod!=null)
                    sqlparams[1] = new SqlParameter("@InvokedMethod", log.InvokedMethod);
                else
                    sqlparams[1] = new SqlParameter("@InvokedMethod", DBNull.Value);

                if(log.UniqueAttributeName!=null)
                    sqlparams[2] = new SqlParameter("@UniqueAttributeName", log.UniqueAttributeName);
                else
                    sqlparams[2] = new SqlParameter("@UniqueAttributeName", DBNull.Value);

                if(log.UniqueAttributeValue!=null)
                    sqlparams[3] = new SqlParameter("@UniqueAttributeValue", log.UniqueAttributeValue);
                else
                    sqlparams[3] = new SqlParameter("@UniqueAttributeValue", DBNull.Value);

                if(log.RequestParameters!=null)
                    sqlparams[4] = new SqlParameter("@RequestParameters", log.RequestParameters);
                else
                    sqlparams[4] = new SqlParameter("@RequestParameters", DBNull.Value);

                if(log.RequestResult!=null)
                    sqlparams[5] = new SqlParameter("@RequestResult", log.RequestResult);
                else
                    sqlparams[5] = new SqlParameter("@RequestResult", DBNull.Value);

                if(log.ErrorMessage!=null)
                    sqlparams[6] = new SqlParameter("@ErrorMessage", log.ErrorMessage);
                else
                    sqlparams[6] = new SqlParameter("@ErrorMessage", DBNull.Value);


                dh.ExecuteNonQuery("ECOMMERCE.uspSaveLog", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

        }

        public List<SocialCategory> GetSocialCategoryList()
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            List<SocialCategory> categories = new();

            try
            {
                ds = dh.ExecuteDataSet("select * from T_Caste_Master where I_Status=1", CommandType.Text);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SocialCategory category = new();

                        category.SocialCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Caste_ID"]);
                        category.SocialCategoryName = ds.Tables[0].Rows[i]["S_Caste_Name"].ToString();

                        categories.Add(category);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return categories;
        }

        public List<ProductSubscription> GetCustomerMandateLinks(string CustomerID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            List<ProductSubscription> subscriptions = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetMandateLinksForLMS", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ProductSubscription subscription = new();

                        if(ds.Tables[0].Rows[i]["CustomerID"]!=DBNull.Value)
                            subscription.CustomerID = ds.Tables[0].Rows[i]["CustomerID"].ToString();

                        if(ds.Tables[0].Rows[i]["PlanID"] !=DBNull.Value)
                            subscription.PlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["PlanID"]);

                        if(ds.Tables[0].Rows[i]["PlanName"]!=DBNull.Value)
                            subscription.PlanName = ds.Tables[0].Rows[i]["PlanName"].ToString();

                        if(ds.Tables[0].Rows[i]["SubscriptionLink"]!=DBNull.Value)
                            subscription.MandateLink = ds.Tables[0].Rows[i]["SubscriptionLink"].ToString();

                        if (ds.Tables[0].Rows[i]["TransactionNo"] != DBNull.Value)
                            subscription.TransactionNo = ds.Tables[0].Rows[i]["TransactionNo"].ToString();

                        subscriptions.Add(subscription);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return subscriptions;
        }

        public int SaveProduct(Product product, string CreatedBy)
        {
            DataHelper dh = new(_conn);

            int ProductID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[12];
                sqlparams[0] = new SqlParameter("@ProductCode", product.ProductCode);
                sqlparams[1] = new SqlParameter("@ProductName", product.ProductName);
                sqlparams[2] = new SqlParameter("@ProductShortDesc", product.ShortDesc);
                sqlparams[3] = new SqlParameter("@ProductLongDesc", product.LongDesc);
                sqlparams[4] = new SqlParameter("@CourseID", product.CourseID);
                sqlparams[5] = new SqlParameter("@BrandID", product.BrandID);
                sqlparams[6] = new SqlParameter("@CategoryID", product.CategoryDetails.CategoryID);
                sqlparams[7] = new SqlParameter("@IsPublished", product.IsPublished);
                sqlparams[8] = new SqlParameter("@ValidFrom", product.ValidFrom);
                sqlparams[9] = new SqlParameter("@ValidTo", product.ValidTo);
                sqlparams[10] = new SqlParameter("@CreatedBy", CreatedBy);
                sqlparams[11] = new SqlParameter("@ProductImage", product.ProductImage);

                ProductID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSaveProduct", CommandType.StoredProcedure, sqlparams));

            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return ProductID;
        }

        public int SavePlan(Plan plan, string CreatedBy)
        {
            DataHelper dh = new(_conn);

            int PlanID = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[10];
                sqlparams[0] = new SqlParameter("@PlanCode", plan.PlanCode);
                sqlparams[1] = new SqlParameter("@PlanName", plan.PlanName);
                sqlparams[2] = new SqlParameter("@PlanDesc", plan.PlanDesc);
                sqlparams[3] = new SqlParameter("@ValidFrom", plan.ValidFrom);
                sqlparams[4] = new SqlParameter("@ValidTo", plan.ValidTo);
                sqlparams[5] = new SqlParameter("@BrandIDList", plan.BrandIDList);
                sqlparams[6] = new SqlParameter("@PlanImage", plan.PlanImage);
                sqlparams[7] = new SqlParameter("@CreatedBy", CreatedBy);
                sqlparams[8] = new SqlParameter("@LanguageID", plan.PlanLanguageID);
                sqlparams[9] = new SqlParameter("@LanguageName",plan.PlanLanguageName);


                PlanID = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspSavePlan", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return PlanID;
        }

        public void SaveProductExamCategoriesMappings(int ProductID, string ExamCategoryIDList,string CreatedBy,SqlTransaction trans, DataHelper dh)
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];

                sqlparams[0] = new SqlParameter("@ProductID", ProductID);
                sqlparams[1] = new SqlParameter("@ExamCategoryList", ExamCategoryIDList);
                sqlparams[2] = new SqlParameter("@CreatedBy", CreatedBy);

                dh.ExecuteNonQuery("ECOMMERCE.uspSaveProductExamCategoryMappings", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void SaveProductCenterFeePlanMappings(int ProductID, int CenterID,int FeePlanID, DateTime ValidFrom, DateTime ValidTo, string CreatedBy, SqlTransaction trans, DataHelper dh)
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[6];

                sqlparams[0] = new SqlParameter("@ProductID", ProductID);
                sqlparams[1] = new SqlParameter("@CenterID", CenterID);
                sqlparams[2] = new SqlParameter("@FeePlanID", FeePlanID);
                sqlparams[3] = new SqlParameter("@ValidFrom", ValidFrom);
                sqlparams[4] = new SqlParameter("@ValidTo", ValidTo);
                sqlparams[5] = new SqlParameter("@CreatedBy", CreatedBy);


                dh.ExecuteNonQuery("ECOMMERCE.uspSaveProductCenterFeePlanMappings", CommandType.StoredProcedure, trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public List<FeePlan> GetFeePlanList(int CenterID, int ProductID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new DataSet();

            List<FeePlan> feePlans = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@ProductID", ProductID);
                sqlparams[1] = new SqlParameter("@CenterID", CenterID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetFeePlanList", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        feePlans.Add(new FeePlan()
                        {
                            FeePlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Course_Fee_Plan_ID"]),
                            FeePlanName = ds.Tables[0].Rows[i]["S_Fee_Plan_Name"].ToString()
                        });
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return feePlans;
        }

        public int ActivateDeActivateProduct(int ProductID, bool Activate)
        {
            DataHelper dh = new(_conn);
            //int IsPublished = 0;
            int i = 0;

            try
            {
                //if (Activate)
                //    IsPublished = 1;
                //else
                //    IsPublished = 0;


                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@ProductID", ProductID);
                sqlparams[1] = new SqlParameter("@Flag", Activate);

                i = dh.ExecuteNonQuery("ECOMMERCE.uspActivateDeactivateProduct",CommandType.StoredProcedure,sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return i;
        }

        public int ActivateDeActivatePlan(int PlanID, bool Activate)
        {
            DataHelper dh = new(_conn);
            int IsPublished = 0;
            int i = 0;

            try
            {
                if (Activate)
                    IsPublished = 1;
                else
                    IsPublished = 0;

                i = dh.ExecuteNonQuery("UPDATE ECOMMERCE.T_Plan_Master set IsPublished=" + IsPublished.ToString() + " WHERE PlanID=" + PlanID.ToString(),
                                            CommandType.Text);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return i;
        }

        public void SavePlanProductMap(string ProductIDList, int PlanID)
        {
            DataHelper dh = new(_conn);

            //string ProductIDList = "";

            try
            {
                //foreach (var p in plans)
                //    ProductIDList = ProductIDList + p.ProductDetails.ProductID.ToString() + ",";

                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@PlanID", PlanID);
                sqlparams[1] = new SqlParameter("@ProductIDList", ProductIDList);

                dh.ExecuteNonQuery("ECOMMERCE.uspSavePlanProductMap", CommandType.StoredProcedure,sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public int ValidateUser(string LoginID, string Password)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            //bool IsValid = false;
            int userid = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@vLoginID", LoginID);
                sqlparams[1] = new SqlParameter("@vPassword", Password);
                ds = dh.ExecuteDataSet("uspGetUserLoginInformation", CommandType.StoredProcedure, sqlparams);

                if (ds != null)
                {
                    string username = null;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        username = ds.Tables[0].Rows[i]["FirstName"].ToString() + " " + ds.Tables[0].Rows[i]["MiddleName"].ToString() + " " + ds.Tables[0].Rows[i]["LastName"].ToString();
                        userid = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"]);

                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
            return userid;
        }

        public void CalculateStudentStatus(string Status, int StudentDetailID)
        {
            DataHelper dh = new(_conn);

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@Status", Status);
                sqlparams[1] = new SqlParameter("@StudentDetailID", StudentDetailID);

                dh.ExecuteNonQuery("LMS.uspCalculateStudentStatusForAPI_temp", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void InsertStudentStatusInQueue()
        {
            DataHelper dh = new(_conn);

            try
            {

                dh.ExecuteNonQuery("LMS.uspInsertStudentStatusForInterfaceAPI", CommandType.StoredProcedure);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public void UpdateEnquiryOnAdmission(int EnquiryID, SqlTransaction trans, DataHelper dh)
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@EnquiryID", EnquiryID);

                dh.ExecuteNonQuery("ECOMMERCE.uspUpdateEnquiryOnAdmission", CommandType.StoredProcedure,trans, sqlparams);
            }
            finally
            {
                if (dh != null && trans == null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public List<Order> GetOrders(string CustomerID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<Order> orders = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetALLTransactionDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Order order = new();

                        order.CustomerID = CustomerID;
                        order.PaymentType = ds.Tables[0].Rows[i]["PaymentType"].ToString();
                        order.TransactionNo = ds.Tables[0].Rows[i]["TransactionNo"].ToString();
                        order.TransactionMode = ds.Tables[0].Rows[i]["TransactionMode"].ToString();
                        order.TransactionStatus = ds.Tables[0].Rows[i]["TransactionStatus"].ToString();
                        order.TransactionDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TransactionDate"]);
                        order.TransactionAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["TransactionAmount"]);
                        order.IsCompleted = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCompleted"]);

                        if (ds.Tables.Count > 1)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                OrderDetail detail = new();

                                if (order.TransactionNo == ds.Tables[1].Rows[j]["TransactionNo"].ToString())
                                {
                                    detail.PlanID = Convert.ToInt32(ds.Tables[1].Rows[j]["PlanID"]);
                                    detail.PlanName = ds.Tables[1].Rows[j]["PlanName"].ToString();
                                    detail.PlanImage = ds.Tables[1].Rows[j]["PlanImage"].ToString();
                                    detail.ReceiptIDList = ds.Tables[1].Rows[j]["ReceiptIDList"].ToString();
                                    detail.FeeScheduleIDList = ds.Tables[1].Rows[j]["FeeScheduleIDList"].ToString();

                                    order.OrderDetails.Add(detail);
                                }
                            }
                        }

                        orders.Add(order);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return orders;
        }

        public void ResetDataForTesting(string CustomerID)
        {
            DataHelper dh = new(_conn);

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];

                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);

                dh.ExecuteNonQuery("ECOMMERCE.uspResetDataForCustomerForTesting", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


        }

        public void SaveBatchNotification(string CustomerID, int PlanID, int ProductID,int CenterID, string MobileNo, string EmailID="")
        {
            DataHelper dh = new(_conn);

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[6];
                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);
                sqlparams[1] = new SqlParameter("@PlanID", PlanID);
                sqlparams[2] = new SqlParameter("@ProductID", ProductID);
                sqlparams[3] = new SqlParameter("@MobileNo", MobileNo);
                sqlparams[4] = new SqlParameter("@EmailID", EmailID);
                sqlparams[5] = new SqlParameter("@CenterID", CenterID);

                dh.ExecuteNonQuery("ECOMMERCE.uspSaveBatchNotification", CommandType.StoredProcedure, sqlparams);
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }
        }

        public AboutBrand GetAboutBrandDetails(string ToBeDisplayedIn, int BrandID=109)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            AboutBrand aboutBrand = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@ToBeDisplayedIn", ToBeDisplayedIn);
                sqlparams[1] = new SqlParameter("@BrandID", BrandID);

                ds = dh.ExecuteDataSet("LMS.uspGetAboutBrandDetails", CommandType.StoredProcedure, sqlparams);


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    aboutBrand.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    aboutBrand.BrandID = Convert.ToInt32(ds.Tables[0].Rows[0]["BrandID"]);
                    aboutBrand.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    aboutBrand.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    aboutBrand.VideoLink = ds.Tables[0].Rows[0]["VideoLink"].ToString();
                    aboutBrand.BannerImage = ds.Tables[0].Rows[0]["BannerImage"].ToString();
                    aboutBrand.ToBeDisplayedIn = ds.Tables[0].Rows[0]["ToBeDisplayedIn"].ToString();
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return aboutBrand;
        }

        public List<Syllabus> GetSyllabusForProduct(int ProductID,int PlanID=0)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<Syllabus> syllabuses = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@PlanID", PlanID);
                sqlparams[1] = new SqlParameter("@ProductID", ProductID);


                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetSyllabusForPlanProduct", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Syllabus syllabus = new();

                        syllabus.SyllabusID = Convert.ToInt32(ds.Tables[0].Rows[i]["SyllabusID"]);
                        syllabus.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]);
                        syllabus.ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]);
                        syllabus.SyllabusName = ds.Tables[0].Rows[i]["SyllabusName"].ToString();


                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (syllabus.SyllabusID == Convert.ToInt32(ds.Tables[1].Rows[j]["SyllabusID"]))
                            {
                                SyllabusSubject subject = new();

                                subject.SubjectID = Convert.ToInt32(ds.Tables[1].Rows[j]["SubjectID"]);
                                subject.SubjectName = ds.Tables[1].Rows[j]["SubjectName"].ToString();

                                for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                                {
                                    if (subject.SubjectID == Convert.ToInt32(ds.Tables[2].Rows[k]["SubjectID"]))
                                    {
                                        SyllabusChapter chapter = new();

                                        chapter.ChapterID = Convert.ToInt32(ds.Tables[2].Rows[k]["ChapterID"]);
                                        chapter.ChapterName = ds.Tables[2].Rows[k]["ChapterName"].ToString();


                                        for (int l = 0; l < ds.Tables[3].Rows.Count; l++)
                                        {
                                            if (chapter.ChapterID == Convert.ToInt32(ds.Tables[3].Rows[l]["ChapterID"]))
                                            {
                                                SyllabusTopic topic = new();

                                                topic.TopicID = Convert.ToInt32(ds.Tables[3].Rows[l]["TopicID"]);
                                                topic.TopicName = ds.Tables[3].Rows[l]["TopicName"].ToString();


                                                chapter.TopicList.Add(topic);
                                            }
                                        }

                                        subject.ChapterList.Add(chapter);

                                    }
                                }


                                syllabus.SubjectList.Add(subject);
                            }
                        }

                        syllabuses.Add(syllabus);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return syllabuses;
        }


        public List<Schedule> GetScheduleForProduct(int ProductID, int PlanID = 0)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<Schedule> schedules = new();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@PlanID", PlanID);
                sqlparams[1] = new SqlParameter("@ProductID", ProductID);


                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetScheduleForPlanProduct", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Schedule schedule = new();

                        schedule.ScheduleID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleID"]);
                        schedule.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]);
                        schedule.ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]);
                        schedule.ScheduleName = ds.Tables[0].Rows[i]["ScheduleName"].ToString();


                        for(int p=0;p<ds.Tables[5].Rows.Count;p++)
                        {
                            if(schedule.ProductID==Convert.ToInt32(ds.Tables[5].Rows[p]["ProductID"]))
                                schedule.TotalExams = ds.Tables[5].Rows[p]["ConfigValue"].ToString();
                        }

                        for (int q = 0; q < ds.Tables[6].Rows.Count; q++)
                        {
                            if (schedule.ProductID == Convert.ToInt32(ds.Tables[6].Rows[q]["ProductID"]))
                                schedule.ExamsCovered = schedule.ExamsCovered+ds.Tables[6].Rows[q]["ExamCategoryDesc"].ToString()+", ";
                        }


                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (schedule.ScheduleID == Convert.ToInt32(ds.Tables[1].Rows[j]["ScheduleID"]))
                            {
                                ScheduleSM sm = new();

                                sm.SMID = Convert.ToInt32(ds.Tables[1].Rows[j]["SMID"]);
                                sm.SMName = ds.Tables[1].Rows[j]["SMName"].ToString();

                                for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                                {
                                    if (sm.SMID == Convert.ToInt32(ds.Tables[2].Rows[k]["SMID"]))
                                    {
                                        ScheduleSubject subject = new();

                                        subject.SubjectID = Convert.ToInt32(ds.Tables[2].Rows[k]["SubjectID"]);
                                        subject.SubjectName = ds.Tables[2].Rows[k]["SubjectName"].ToString();


                                        for (int l = 0; l < ds.Tables[3].Rows.Count; l++)
                                        {
                                            if (subject.SubjectID == Convert.ToInt32(ds.Tables[3].Rows[l]["SubjectID"]))
                                            {
                                                ScheduleTopic topic = new();

                                                topic.TopicID = Convert.ToInt32(ds.Tables[3].Rows[l]["TopicID"]);
                                                topic.TopicName = ds.Tables[3].Rows[l]["TopicName"].ToString();

                                                for (int n = 0; n < ds.Tables[4].Rows.Count; n++)
                                                {
                                                    if (topic.TopicID == Convert.ToInt32(ds.Tables[4].Rows[n]["TopicID"]))
                                                    {
                                                        ScheduleTopicBreakup breakup = new();

                                                        breakup.TopicBreakupID = Convert.ToInt32(ds.Tables[4].Rows[n]["TopicBreakupID"]);
                                                        breakup.ItemName = ds.Tables[4].Rows[n]["ItemName"].ToString();
                                                        breakup.ItemValue = ds.Tables[4].Rows[n]["ItemValue"].ToString();

                                                        topic.TopicBreakupList.Add(breakup);
                                                    }
                                                }


                                                //chapter.TopicList.Add(topic);


                                                subject.TopicList.Add(topic);
                                            }
                                        }

                                        sm.SubjectList.Add(subject);

                                    }
                                }


                                schedule.SMList.Add(sm);
                            }
                        }

                        schedules.Add(schedule);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return schedules;
        }

        public List<ExamGroup> GetExamGroups(int BrandID)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<ExamGroup> exams = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@BrandID", BrandID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetExamGroupList", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ExamGroup exam = new();

                        exam.ExamGroupID = Convert.ToInt32(ds.Tables[0].Rows[i]["ExamGroupID"]);
                        exam.ExamGroupDesc = ds.Tables[0].Rows[i]["ExamGroupDesc"].ToString();
                        exam.ExamGroupDetail= ds.Tables[0].Rows[i]["ExamGroupDetail"].ToString();

                        exams.Add(exam);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }



            return exams;
        }

        public CampaignCoupon GenerateCouponCodeForCampaign(string CampaignName, decimal MarksObtained, string CustomerID,
                                                            DateTime? ValidFrom = null, DateTime? ValidTo = null,int LanguageID=0)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();
            CampaignCoupon coupon = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[6];
                sqlparams[0] = new SqlParameter("@CampaignName", CampaignName);
                sqlparams[1] = new SqlParameter("@MarksObtained", MarksObtained);
                sqlparams[2] = new SqlParameter("@CustomerID", CustomerID);

                if (ValidFrom != null)
                    sqlparams[3] = new SqlParameter("@ValidFrom", ValidFrom);
                else
                    sqlparams[3] = new SqlParameter("@ValidFrom", DBNull.Value);

                if (ValidTo != null)
                    sqlparams[4] = new SqlParameter("@ValidTo", ValidTo);
                else
                    sqlparams[4] = new SqlParameter("@ValidTo", DBNull.Value);

                sqlparams[5]=new SqlParameter("@LanguageID", LanguageID);


                ds = dh.ExecuteDataSet("ECOMMERCE.uspGenerateCouponForCampaign", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["CouponCode"].ToString() != "")
                    {
                        coupon.CouponCode = ds.Tables[0].Rows[0]["CouponCode"].ToString();
                        coupon.LumpsumDiscountPerc = Convert.ToDecimal(ds.Tables[0].Rows[0]["LumpsumDiscount"]);
                        coupon.InstalmentDiscountPerc = Convert.ToDecimal(ds.Tables[0].Rows[0]["InstalmentDiscount"]);
                        coupon.MessageDesc = ds.Tables[0].Rows[0]["MessageDesc"].ToString();
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return coupon;
        }

        public List<CouponCard> GetCouponCards(string CustomerID, string CouponType, string CampaignName=null)
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<CouponCard> couponCards = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[3];
                sqlparams[0] = new SqlParameter("@CustomerID", CustomerID);
                sqlparams[1] = new SqlParameter("@CouponType", CouponType);

                if (CampaignName == null)
                    sqlparams[2] = new SqlParameter("@CampaignName", DBNull.Value);
                else
                    sqlparams[2] = new SqlParameter("@CampaignName", CampaignName);


                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetAllCouponList", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CouponCard card = new();

                        card.CouponCode = ds.Tables[0].Rows[i]["CouponCode"].ToString();
                        card.CampaignName = ds.Tables[0].Rows[i]["CampaignName"].ToString();
                        card.LumpsumDiscountPerc = Convert.ToDecimal(ds.Tables[0].Rows[i]["LumpsumDiscountPerc"]);
                        card.InstalmentDiscountPerc = Convert.ToDecimal(ds.Tables[0].Rows[i]["InstalmentDiscountPerc"]);
                        card.MessageDesc= ds.Tables[0].Rows[i]["MessageDesc"].ToString();

                        couponCards.Add(card);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }

            return couponCards;
        }

        public List<IssueCategory> GetIssueCategories()
        {
            DataHelper dh = new(_conn);
            DataSet ds = new();

            List<IssueCategory> categories = new();

            try
            {
                ds = dh.ExecuteDataSet("SELECT * FROM LMS.T_Issue_Category_Master WHERE StatusID=1", CommandType.Text);

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        IssueCategory category = new();

                        category.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        category.CategoryName = ds.Tables[0].Rows[i]["CategoryName"].ToString();
                        category.CategoryDesc = ds.Tables[0].Rows[i]["CategoryDesc"].ToString();
                        category.DesignatedEmailID = ds.Tables[0].Rows[i]["DesignatedEmailID"].ToString();

                        categories.Add(category);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return categories;
        }

        public int SaveCustomerIssue(string Issue, int IssueCategoryID,string Name, string StudentID, string CustomerID, 
                                            string ContactNo, string EmailID)
        {
            DataHelper dh = new(_conn);

            int c = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[7];
                sqlparams[0] = new SqlParameter("@Issue", Issue);
                sqlparams[1] = new SqlParameter("@Name", Name);
                sqlparams[2] = new SqlParameter("@StudentID", StudentID);
                sqlparams[3] = new SqlParameter("@CustomerID", CustomerID);
                sqlparams[4] = new SqlParameter("@ContactNo", ContactNo);
                sqlparams[5] = new SqlParameter("@EmailID", EmailID);
                sqlparams[6] = new SqlParameter("@IssueCategoryID", IssueCategoryID);

                c = Convert.ToInt32(dh.ExecuteScalar("LMS.uspSaveStudentIssue", CommandType.StoredProcedure, sqlparams));
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return c;
        }

        //done By susmita 

        public List<Coupon> GetCouponsPerCustomerPlan(int PlanID,String CustomerID, int ProductID, int centerID, int PaymentMode,double PurchaseAmount)
        {
            DataHelper dh = new DataHelper(_conn);
   
            DataSet ds = new();
          
            List<Coupon> coupons = new();
            string query = "";
            string studentcountquery = "";
            int perstudentcount = 0;
            int couponcat = 0;
            

            try
            {

                /* End of  Student category (3)*/

                query = "select TCM.*,PPM.*,PCM.*,TDSD.*,TCCM.CouponCategoryDesc,TCTM.CouponTypeDesc,TDSM.S_Discount_Scheme_Name,TPCM.PlanID,TDCD2.I_Course_ID" +
                    " from ECOMMERCE.T_Coupon_Master TCM" +
                    " inner join ECOMMERCE.T_Coupon_Category_Master TCCM on TCM.CouponCategoryID = TCCM.CouponCategoryID" +
                    " inner join ECOMMERCE.T_Coupon_Type_Master TCTM on TCM.CouponType = TCTM.CouponTypeID" +
                    " inner join T_Discount_Scheme_Master TDSM on TDSM.I_Discount_Scheme_ID = TCM.DiscountSchemeID" +
                    " inner join T_Discount_Scheme_Details TDSD on TDSD.I_Discount_Scheme_ID = TDSM.I_Discount_Scheme_ID" +
                    " inner join T_Discount_Center_Detail TDCD on TDCD.I_Discount_Scheme_ID=TCM.DiscountSchemeID and TDCD.I_Status=1" +
                    " inner join T_Discount_Course_Detail TDCD2 on TDCD.I_Discount_Center_Detail_ID=TDCD2.I_Discount_Centre_Detail_ID and TDCD2.I_Status=1"+
                    " inner join ECOMMERCE.T_Product_Master TPM on  TPM.CourseID=TDCD2.I_Course_ID"+
                    " inner join ECOMMERCE.T_Product_Center_Map PCM on PCM.ProductID = TPM.ProductID" +
                    " inner join ECOMMERCE.T_Plan_Coupon_Map TPCM on TCM.CouponID = TPCM.CouponID" +
                    " inner join ECOMMERCE.T_Plan_Product_Map PPM on PPM.PlanID = TPCM.PlanID and PPM.ProductID=TPM.ProductID" +
                    " inner join ECOMMERCE.T_Coupon_Brand_Map TCBM on TCBM.CouponID = TCM.CouponID"+
                    " inner join ECOMMERCE.T_Plan_Brand_Map TPBM on TPCM.PlanID=TPBM.PlanID and TPBM.StatusID=1"+
                    " where(TCM.CustomerID = '" + CustomerID + "' or TCM.CustomerID IS NULL) and TPCM.PlanID = " + PlanID + " and PPM.ProductID = " + ProductID + " and PCM.CenterID = " + centerID +
                    " and TCM.CouponCount>TCM.AssignedCount and TPCM.StatusID = 1 and TDSD.I_IsApplicableOn = " + PaymentMode + " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TPCM.ValidFrom) and(CONVERT(DATE,TPCM.ValidTo) >= CONVERT(DATE, GETDATE()) or TPCM.ValidTo IS NULL))" +
                    " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TCM.ValidFrom) and(CONVERT(DATE,TCM.ValidTo) >= CONVERT(DATE, GETDATE()) or TCM.ValidTo IS NULL))" +
                    " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TDSM.Dt_Valid_From) and(CONVERT(DATE,TDSM.Dt_Valid_To) >= CONVERT(DATE, GETDATE()) or TDSM.Dt_Valid_To IS NULL))" +
                    " and PCM.IsPublished = 1 and PCM.StatusID = 1 and TCM.StatusID = 1 and TCTM.StatusID = 1 and TDSM.I_Status = 1 and TPCM.StatusID = 1 and PPM.StatusID = 1 and TCBM.BrandID=109 and TPBM.BrandID = 109" +
                    " and TCM.CouponCategoryID=3 and (TCM.IsPrivate='false' or TCM.IsPrivate IS NULL)";//added condition for isprivate

              
                ds = dh.ExecuteDataSet(query, CommandType.Text);

              

               if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        
                        Coupon coupon = new();

                        coupon.CouponID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponID"]);
                        coupon.CouponCode = ds.Tables[0].Rows[i]["CouponCode"].ToString();
                        coupon.CouponName = ds.Tables[0].Rows[i]["CouponName"].ToString();
                        coupon.CouponDesc = ds.Tables[0].Rows[i]["CouponDesc"].ToString();
                        coupon.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        coupon.StatusID = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusID"]);
                        coupon.CouponCount = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCount"]);
                        coupon.AssignedCount = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignedCount"]);
                        coupon.CustomerCode = ds.Tables[0].Rows[i]["CustomerID"].ToString();
                        coupon.ValidFrom = Convert.ToDateTime(ds.Tables[0].Rows[i]["validFrom"].ToString());
                        coupon.ValidTo = Convert.ToDateTime(ds.Tables[0].Rows[i]["validTo"].ToString());
                        coupon.CouponTypeDetails.CouponTypeID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponType"]);
                        coupon.CouponTypeDetails.CouponTypeDesc = ds.Tables[0].Rows[i]["CouponTypeDesc"].ToString();

                        coupon.CategoryDetails.CouponCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);
                        coupon.CategoryDetails.CouponCategoryDesc = ds.Tables[0].Rows[i]["CouponCategoryDesc"].ToString();

                        
                        coupon.DiscountDetails.DiscountSchemeID = Convert.ToInt32(ds.Tables[0].Rows[i]["DiscountSchemeID"]);
                        coupon.DiscountDetails.DiscountSchemeName = ds.Tables[0].Rows[i]["S_Discount_Scheme_Name"].ToString();

                        coupon.DiscountDetails.DiscountSchemeDetails = new List<DiscountSchemeDetail>();
                        coupon.DiscountDetails.DiscountSchemeDetails.Add(new DiscountSchemeDetail()
                        {
                            DiscountSchemeDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Discount_Scheme_Detail_ID"]),
                            DiscountRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["N_Discount_Rate"]),
                            IsApplicableOn= Convert.ToInt32(ds.Tables[0].Rows[i]["I_IsApplicableOn"]),
                            FeeComponentID= (ds.Tables[0].Rows[i]["S_FeeComponents"]).ToString()


                        });



                        if (ds.Tables[0].Rows[i]["IsPrivate"] == DBNull.Value)
                            coupon.IsPrivate = false;
                        else
                            coupon.IsPrivate = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPrivate"]);


                            coupons.Add(coupon);
                    }
                }

                /* End of  Student category (3)*/

                /* End of  Plan category (1)*/
                query = "select TCM.*,PPM.*,PCM.*,TDSD.*,TCCM.CouponCategoryDesc,TCTM.CouponTypeDesc,TDSM.S_Discount_Scheme_Name,TPCM.PlanID,TDCD2.I_Course_ID" +
                " from ECOMMERCE.T_Coupon_Master TCM" +
                " inner join ECOMMERCE.T_Coupon_Category_Master TCCM on TCM.CouponCategoryID = TCCM.CouponCategoryID" +
                " inner join ECOMMERCE.T_Coupon_Type_Master TCTM on TCM.CouponType = TCTM.CouponTypeID" +
                " inner join T_Discount_Scheme_Master TDSM on TDSM.I_Discount_Scheme_ID = TCM.DiscountSchemeID" +
                " inner join T_Discount_Scheme_Details TDSD on TDSD.I_Discount_Scheme_ID = TDSM.I_Discount_Scheme_ID" +
                " inner join T_Discount_Center_Detail TDCD on TDCD.I_Discount_Scheme_ID=TCM.DiscountSchemeID and TDCD.I_Status=1" +
                " inner join T_Discount_Course_Detail TDCD2 on TDCD.I_Discount_Center_Detail_ID=TDCD2.I_Discount_Centre_Detail_ID and TDCD2.I_Status=1" +
                " inner join ECOMMERCE.T_Product_Master TPM on  TPM.CourseID=TDCD2.I_Course_ID" +
                " inner join ECOMMERCE.T_Product_Center_Map PCM on PCM.ProductID = TPM.ProductID" +
                " inner join ECOMMERCE.T_Plan_Coupon_Map TPCM on TCM.CouponID = TPCM.CouponID" +
                " inner join ECOMMERCE.T_Plan_Product_Map PPM on PPM.PlanID = TPCM.PlanID and PPM.ProductID=TPM.ProductID" +
                " inner join ECOMMERCE.T_Coupon_Brand_Map TCBM on TCBM.CouponID = TCM.CouponID" +
                " inner join ECOMMERCE.T_Plan_Brand_Map TPBM on TPCM.PlanID=TPBM.PlanID and TPBM.StatusID=1" +
                " where(TCM.CustomerID = '" + CustomerID + "' or TCM.CustomerID IS NULL) and TPCM.PlanID = " + PlanID + " and PPM.ProductID = " + ProductID + " and PCM.CenterID = " + centerID +
                " and TCM.CouponCount>TCM.AssignedCount and TPCM.StatusID = 1 and TDSD.I_IsApplicableOn = " + PaymentMode + " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TPCM.ValidFrom) and(CONVERT(DATE,TPCM.ValidTo) >= CONVERT(DATE, GETDATE()) or TPCM.ValidTo IS NULL))" +
                " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TCM.ValidFrom) and(CONVERT(DATE,TCM.ValidTo) >= CONVERT(DATE, GETDATE()) or TCM.ValidTo IS NULL))" +
                " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TDSM.Dt_Valid_From) and(CONVERT(DATE,TDSM.Dt_Valid_To) >= CONVERT(DATE, GETDATE()) or TDSM.Dt_Valid_To IS NULL))" +
                " and PCM.IsPublished = 1 and PCM.StatusID = 1 and TCM.StatusID = 1 and TCTM.StatusID = 1 and TDSM.I_Status = 1 and TPCM.StatusID = 1 and PPM.StatusID = 1 and TCBM.BrandID=109 and TPBM.BrandID = 109" +
                " and TCM.CouponCategoryID=1 and (TCM.IsPrivate='false' or TCM.IsPrivate IS NULL)";//added condition for isprivate


                ds = dh.ExecuteDataSet(query, CommandType.Text);



                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        studentcountquery = "SELECT ISNULL(COUNT(DISTINCT A.TransactionID),0) " +
                             " FROM ECOMMERCE.T_Transaction_Master A" +
                             " INNER JOIN ECOMMERCE.T_Transaction_Plan_Details B ON B.TransactionID = A.TransactionID" +
                             " INNER JOIN ECOMMERCE.T_Transaction_Product_Details C ON C.TransactionPlanDetailID = B.TransactionPlanDetailID" +
                             " WHERE A.CustomerID COLLATE DATABASE_DEFAULT = ISNULL('" + CustomerID + "', '') COLLATE DATABASE_DEFAULT" +
                             " AND A.StatusID = 1 AND A.TransactionStatus != 'Failure' AND C.CouponCode IS NOT NULL" +
                             " AND C.CouponCode COLLATE DATABASE_DEFAULT = '"+ ds.Tables[0].Rows[i]["CouponCode"].ToString() + "' COLLATE DATABASE_DEFAULT";

                         perstudentcount= Convert.ToInt32(dh.ExecuteScalar(studentcountquery, CommandType.Text));

                         couponcat = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);

                        
                             if (Convert.ToInt32(ds.Tables[0].Rows[i]["PerStudentCount"]) <= perstudentcount)
                             {
                                 continue;
                             }



                        Coupon coupon = new();

                        coupon.CouponID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponID"]);
                        coupon.CouponCode = ds.Tables[0].Rows[i]["CouponCode"].ToString();
                        coupon.CouponName = ds.Tables[0].Rows[i]["CouponName"].ToString();
                        coupon.CouponDesc = ds.Tables[0].Rows[i]["CouponDesc"].ToString();
                        coupon.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        coupon.StatusID = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusID"]);
                        coupon.CouponCount = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCount"]);
                        coupon.AssignedCount = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignedCount"]);
                        coupon.CustomerCode = ds.Tables[0].Rows[i]["CustomerID"].ToString();
                        coupon.ValidFrom = Convert.ToDateTime(ds.Tables[0].Rows[i]["validFrom"].ToString());
                        coupon.ValidTo = Convert.ToDateTime(ds.Tables[0].Rows[i]["validTo"].ToString());
                        coupon.CouponTypeDetails.CouponTypeID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponType"]);
                        coupon.CouponTypeDetails.CouponTypeDesc = ds.Tables[0].Rows[i]["CouponTypeDesc"].ToString();

                        coupon.CategoryDetails.CouponCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);
                        coupon.CategoryDetails.CouponCategoryDesc = ds.Tables[0].Rows[i]["CouponCategoryDesc"].ToString();


                        coupon.DiscountDetails.DiscountSchemeID = Convert.ToInt32(ds.Tables[0].Rows[i]["DiscountSchemeID"]);
                        coupon.DiscountDetails.DiscountSchemeName = ds.Tables[0].Rows[i]["S_Discount_Scheme_Name"].ToString();

                        coupon.DiscountDetails.DiscountSchemeDetails = new List<DiscountSchemeDetail>();
                        coupon.DiscountDetails.DiscountSchemeDetails.Add(new DiscountSchemeDetail()
                        {
                            DiscountSchemeDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Discount_Scheme_Detail_ID"]),
                            DiscountRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["N_Discount_Rate"]),
                            IsApplicableOn = Convert.ToInt32(ds.Tables[0].Rows[i]["I_IsApplicableOn"]),
                            FeeComponentID = (ds.Tables[0].Rows[i]["S_FeeComponents"]).ToString()


                        });


                        if (ds.Tables[0].Rows[i]["IsPrivate"] == DBNull.Value)
                            coupon.IsPrivate = false;
                        else
                            coupon.IsPrivate = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPrivate"]);



                        coupons.Add(coupon);
                    }
                }

                /* End of  Plan category (1)*/

                /* for Purchase category (2)*/


                query = "select TCM.*,PPM.*,PCM.*,TDSD.*,TCCM.CouponCategoryDesc,TCTM.CouponTypeDesc,TDSM.S_Discount_Scheme_Name,TPCM.PlanID,TDCD2.I_Course_ID" +
              " from ECOMMERCE.T_Coupon_Master TCM" +
              " inner join ECOMMERCE.T_Coupon_Category_Master TCCM on TCM.CouponCategoryID = TCCM.CouponCategoryID" +
              " inner join ECOMMERCE.T_Coupon_Type_Master TCTM on TCM.CouponType = TCTM.CouponTypeID" +
              " inner join T_Discount_Scheme_Master TDSM on TDSM.I_Discount_Scheme_ID = TCM.DiscountSchemeID" +
              " inner join T_Discount_Scheme_Details TDSD on TDSD.I_Discount_Scheme_ID = TDSM.I_Discount_Scheme_ID" +
              " inner join T_Discount_Center_Detail TDCD on TDCD.I_Discount_Scheme_ID=TCM.DiscountSchemeID and TDCD.I_Status=1" +
              " inner join T_Discount_Course_Detail TDCD2 on TDCD.I_Discount_Center_Detail_ID=TDCD2.I_Discount_Centre_Detail_ID and TDCD2.I_Status=1" +
              " inner join ECOMMERCE.T_Product_Master TPM on  TPM.CourseID=TDCD2.I_Course_ID" +
              " inner join ECOMMERCE.T_Product_Center_Map PCM on PCM.ProductID = TPM.ProductID" +
              " inner join ECOMMERCE.T_Plan_Coupon_Map TPCM on TCM.CouponID = TPCM.CouponID" +
              " inner join ECOMMERCE.T_Plan_Product_Map PPM on PPM.PlanID = TPCM.PlanID and PPM.ProductID=TPM.ProductID" +
              " inner join ECOMMERCE.T_Coupon_Brand_Map TCBM on TCBM.CouponID = TCM.CouponID" +
              " inner join ECOMMERCE.T_Plan_Brand_Map TPBM on TPCM.PlanID=TPBM.PlanID and TPBM.StatusID=1" +
              " where(TCM.CustomerID = '" + CustomerID + "' or TCM.CustomerID IS NULL) and TPCM.PlanID = " + PlanID + " and PPM.ProductID = " + ProductID + " and PCM.CenterID = " + centerID +
              " and TCM.CouponCount>TCM.AssignedCount and TPCM.StatusID = 1 and TDSD.I_IsApplicableOn = " + PaymentMode + " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TPCM.ValidFrom) and(CONVERT(DATE,TPCM.ValidTo) >= CONVERT(DATE, GETDATE()) or TPCM.ValidTo IS NULL))" +
              " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TCM.ValidFrom) and(CONVERT(DATE,TCM.ValidTo) >= CONVERT(DATE, GETDATE()) or TCM.ValidTo IS NULL))" +
              " and(CONVERT(DATE, GETDATE()) >= CONVERT(DATE,TDSM.Dt_Valid_From) and(CONVERT(DATE,TDSM.Dt_Valid_To) >= CONVERT(DATE, GETDATE()) or TDSM.Dt_Valid_To IS NULL))" +
              " and PCM.IsPublished = 1 and PCM.StatusID = 1 and TCM.StatusID = 1 and TCTM.StatusID = 1 and TDSM.I_Status = 1 and TPCM.StatusID = 1 and PPM.StatusID = 1 and TCBM.BrandID=109 and TPBM.BrandID = 109" +
              " and TCM.CouponCategoryID=2 and TCM.GreaterThanAmount < " + PurchaseAmount + " and (TCM.IsPrivate='false' or TCM.IsPrivate IS NULL)";//added condition for isprivate


                ds = dh.ExecuteDataSet(query, CommandType.Text);



                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        studentcountquery = "SELECT ISNULL(COUNT(DISTINCT A.TransactionID),0) " +
                             " FROM ECOMMERCE.T_Transaction_Master A" +
                             " INNER JOIN ECOMMERCE.T_Transaction_Plan_Details B ON B.TransactionID = A.TransactionID" +
                             " INNER JOIN ECOMMERCE.T_Transaction_Product_Details C ON C.TransactionPlanDetailID = B.TransactionPlanDetailID" +
                             " WHERE A.CustomerID COLLATE DATABASE_DEFAULT = ISNULL('" + CustomerID + "', '') COLLATE DATABASE_DEFAULT" +
                             " AND A.StatusID = 1 AND A.TransactionStatus != 'Failure' AND C.CouponCode IS NOT NULL" +
                             " AND C.CouponCode COLLATE DATABASE_DEFAULT = '" + ds.Tables[0].Rows[i]["CouponCode"].ToString() + "' COLLATE DATABASE_DEFAULT";

                        perstudentcount = Convert.ToInt32(dh.ExecuteScalar(studentcountquery, CommandType.Text));

                        couponcat = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);


                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["PerStudentCount"]) <= perstudentcount)
                        {
                            continue;
                        }



                        Coupon coupon = new();

                        coupon.CouponID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponID"]);
                        coupon.CouponCode = ds.Tables[0].Rows[i]["CouponCode"].ToString();
                        coupon.CouponName = ds.Tables[0].Rows[i]["CouponName"].ToString();
                        coupon.CouponDesc = ds.Tables[0].Rows[i]["CouponDesc"].ToString();
                        coupon.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        coupon.StatusID = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusID"]);
                        coupon.CouponCount = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCount"]);
                        coupon.AssignedCount = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignedCount"]);
                        coupon.CustomerCode = ds.Tables[0].Rows[i]["CustomerID"].ToString();
                        coupon.ValidFrom = Convert.ToDateTime(ds.Tables[0].Rows[i]["validFrom"].ToString());
                        coupon.ValidTo = Convert.ToDateTime(ds.Tables[0].Rows[i]["validTo"].ToString());
                        coupon.CouponTypeDetails.CouponTypeID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponType"]);
                        coupon.CouponTypeDetails.CouponTypeDesc = ds.Tables[0].Rows[i]["CouponTypeDesc"].ToString();

                        coupon.CategoryDetails.CouponCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CouponCategoryID"]);
                        coupon.CategoryDetails.CouponCategoryDesc = ds.Tables[0].Rows[i]["CouponCategoryDesc"].ToString();


                        coupon.DiscountDetails.DiscountSchemeID = Convert.ToInt32(ds.Tables[0].Rows[i]["DiscountSchemeID"]);
                        coupon.DiscountDetails.DiscountSchemeName = ds.Tables[0].Rows[i]["S_Discount_Scheme_Name"].ToString();

                        coupon.DiscountDetails.DiscountSchemeDetails = new List<DiscountSchemeDetail>();
                        coupon.DiscountDetails.DiscountSchemeDetails.Add(new DiscountSchemeDetail()
                        {
                            DiscountSchemeDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Discount_Scheme_Detail_ID"]),
                            DiscountRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["N_Discount_Rate"]),
                            IsApplicableOn = Convert.ToInt32(ds.Tables[0].Rows[i]["I_IsApplicableOn"]),
                            FeeComponentID = (ds.Tables[0].Rows[i]["S_FeeComponents"]).ToString()


                        });


                        if (ds.Tables[0].Rows[i]["IsPrivate"] == DBNull.Value)
                            coupon.IsPrivate = false;
                        else
                            coupon.IsPrivate = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPrivate"]);



                        coupons.Add(coupon);
                    }
                }



            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return coupons;
        }



        //done By Susmita 

        public List<Campaign> GetActiveCampaigns()
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<Campaign> Campaigns = new();

            try
            {

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetActiveCampaignDetails", CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Campaign CampaignDetail = new();

                        CampaignDetail.CampaignID = Convert.ToInt32(ds.Tables[0].Rows[i]["CampaignMaster_CampaignID"]);
                        CampaignDetail.CampaignName = ds.Tables[0].Rows[i]["CampaignMaster_CampaignName"].ToString();
                        CampaignDetail.CampaignDescription = ds.Tables[0].Rows[i]["CampaignMaster_CampaignDesc"].ToString();
                        CampaignDetail.CampaignBrand = Convert.ToInt32(ds.Tables[0].Rows[i]["CampaignBrand_BrandID"]);
                        CampaignDetail.CampaignBrandStatus = Convert.ToInt32(ds.Tables[0].Rows[i]["CampaignMaster_StatusID"]);

                        if (ds.Tables[0].Rows[i]["CampaignMaster_ValidFrom"] != DBNull.Value)
                            CampaignDetail.CampaignValidFrom = Convert.ToDateTime(ds.Tables[0].Rows[i]["CampaignMaster_ValidFrom"].ToString());

                        if (ds.Tables[0].Rows[i]["CampaignMaster_ValidTo"] != DBNull.Value)
                            CampaignDetail.CampaignValidTo = Convert.ToDateTime(ds.Tables[0].Rows[i]["CampaignMaster_ValidTo"].ToString());

                        if (ds.Tables.Count > 1)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                CampaignDiscountMap campaignDiscountMaps = new();

                                if (Convert.ToInt32(ds.Tables[1].Rows[j]["CampaignID"]) == CampaignDetail.CampaignID)
                                {
                                    campaignDiscountMaps.CampaignID = Convert.ToInt32(ds.Tables[1].Rows[j]["CampaignID"]);

                                    if (ds.Tables[1].Rows[j]["CampaignDiscount_ValidFrom"] != DBNull.Value)
                                        campaignDiscountMaps.CampaignDiscountMapValidFrom= Convert.ToDateTime(ds.Tables[1].Rows[j]["CampaignDiscount_ValidFrom"].ToString());
                                    if (ds.Tables[1].Rows[j]["CampaignDiscount_ValidTo"] != DBNull.Value)
                                        campaignDiscountMaps.CampaignDiscountMapValidTo= Convert.ToDateTime(ds.Tables[1].Rows[j]["CampaignDiscount_ValidTo"].ToString());
                                    
                                    campaignDiscountMaps.CampaignMessageDesc= ds.Tables[1].Rows[j]["CampaignDiscount_MessageDesc"].ToString();

                                    campaignDiscountMaps.CampaignCouponName= ds.Tables[1].Rows[j]["CampaignDiscount_CouponName"].ToString();
                                    campaignDiscountMaps.CampaignCouponPrefix = ds.Tables[1].Rows[j]["CampaignDiscount_CouponPrefix"].ToString();
                                    campaignDiscountMaps.CampaignCouponcount = Convert.ToInt32(ds.Tables[1].Rows[j]["CampaignDiscount_CouponCount"]);
                                    campaignDiscountMaps.CampaignCouponTypeID = Convert.ToInt32(ds.Tables[1].Rows[j]["CampaignDiscount_CouponTypeID"]);

                                    campaignDiscountMaps.FromMarks = Convert.ToDecimal(ds.Tables[1].Rows[j]["CampaignDiscount_FromMarks"]);
                                    campaignDiscountMaps.ToMarks= Convert.ToDecimal(ds.Tables[1].Rows[j]["CampaignDiscount_ToMarks"]);

                                    campaignDiscountMaps.CampaignDiscountMapstatus= Convert.ToInt32(ds.Tables[1].Rows[j]["CampaignDiscount_StatusID"]);

                                    if (ds.Tables[1].Rows[j]["DiscountScheme_I_Discount_Scheme_ID"] != DBNull.Value)
                                        campaignDiscountMaps.DiscountSchemeDetail.DiscountSchemeID = Convert.ToInt32(ds.Tables[1].Rows[j]["DiscountScheme_I_Discount_Scheme_ID"]);
                                    else
                                        campaignDiscountMaps.DiscountSchemeDetail.DiscountSchemeID = 0;

                                    if (ds.Tables[1].Rows[j]["DiscountScheme_S_Discount_Scheme_Name"] != DBNull.Value)
                                        campaignDiscountMaps.DiscountSchemeDetail.DiscountSchemeName = ds.Tables[1].Rows[j]["DiscountScheme_S_Discount_Scheme_Name"].ToString();
                                    else
                                        campaignDiscountMaps.DiscountSchemeDetail.DiscountSchemeName = null;

                                    if (ds.Tables[1].Rows[j]["DiscountScheme_Dt_Valid_From"] != DBNull.Value)
                                        campaignDiscountMaps.DiscountSchemeDetail.ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[j]["DiscountScheme_Dt_Valid_From"].ToString());
                                    //else
                                    //    campaignDiscountMaps.DiscountSchemeDetail.ValidFrom = DateTime.MinValue;
                                    if (ds.Tables[1].Rows[j]["DiscountScheme_Dt_Valid_To"] != DBNull.Value)
                                        campaignDiscountMaps.DiscountSchemeDetail.ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[j]["DiscountScheme_Dt_Valid_To"].ToString());
                                    //else
                                    //    campaignDiscountMaps.DiscountSchemeDetail.ValidTo = DateTime.MinValue;



                                    CampaignDetail.campaignDiscountListsDetails.Add(campaignDiscountMaps);
                                }


                            }


                        }

                        Campaigns.Add(CampaignDetail);



                    }
                }

                
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return Campaigns;
        }







        public List<RecommendedCourseProductPlan> GetRecommendedCourseLists(int CourseID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            List<RecommendedCourseProductPlan> RecommendedCourseProductPlans = new();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@CourseID", CourseID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetRecommendedCourseProductPlanDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        RecommendedCourseProductPlan RecommendedCourseDetails = new();

                        RecommendedCourseDetails.Course.CourseID= Convert.ToInt32(ds.Tables[0].Rows[i]["Recommended_Course_ID"]);
                        RecommendedCourseDetails.Course.CourseName= ds.Tables[0].Rows[i]["Recommended_Course_Name"].ToString();


                        if (ds.Tables.Count > 1)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                ProductPlan ProductDetail = new();

                                if (Convert.ToInt32(ds.Tables[1].Rows[j]["CourseID"]) == RecommendedCourseDetails.Course.CourseID)
                                {

                                    ProductDetail.ProductID= Convert.ToInt32(ds.Tables[1].Rows[j]["ProductID"]);
                                    ProductDetail.ProductCode= ds.Tables[1].Rows[j]["ProductCode"].ToString();
                                    ProductDetail.ProductName = ds.Tables[1].Rows[j]["ProductName"].ToString();
                                    ProductDetail.ProductImage= ds.Tables[1].Rows[j]["ProductImage"].ToString();
                                    ProductDetail.ShortDesc= ds.Tables[1].Rows[j]["ProductShortDesc"].ToString();

                                    if (ds.Tables[1].Rows[j]["ValidFrom"] != DBNull.Value)
                                        ProductDetail.ValidFrom = Convert.ToDateTime(ds.Tables[1].Rows[j]["ValidFrom"].ToString());
                                    if (ds.Tables[1].Rows[j]["ValidTo"] != DBNull.Value)
                                        ProductDetail.ValidTo = Convert.ToDateTime(ds.Tables[1].Rows[j]["ValidTo"].ToString());


                                    ProductDetail.CourseID= Convert.ToInt32(ds.Tables[1].Rows[j]["CourseID"]);


                                    if (ds.Tables.Count > 2)
                                    {
                                        for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                                        {
                                            Plan plan = new Plan();

                                            if (Convert.ToInt32(ds.Tables[2].Rows[k]["ProductID"]) == ProductDetail.ProductID)
                                            {
                                                plan.PlanID = Convert.ToInt32(ds.Tables[2].Rows[k]["PlanID"]);
                                                plan.PlanName = ds.Tables[2].Rows[k]["PlanName"].ToString();
                                                plan.PlanCode = ds.Tables[2].Rows[k]["PlanName"].ToString();

                                                if (ds.Tables[2].Rows[k]["ValidFrom"] != DBNull.Value)
                                                    plan.ValidFrom = Convert.ToDateTime(ds.Tables[2].Rows[k]["ValidFrom"].ToString());
                                                if (ds.Tables[2].Rows[k]["ValidTo"] != DBNull.Value)
                                                    plan.ValidTo = Convert.ToDateTime(ds.Tables[2].Rows[k]["ValidTo"].ToString());


                                                plan.IsPublished = Convert.ToBoolean(ds.Tables[2].Rows[k]["IsPublished"]);


                                                if (ds.Tables[2].Rows[k]["I_Language_ID"] != DBNull.Value)
                                                {
                                                    plan.PlanLanguageID = Convert.ToInt32(ds.Tables[2].Rows[k]["I_Language_ID"]);
                                                    plan.PlanLanguageName = ds.Tables[2].Rows[k]["I_Language_Name"].ToString();
                                                }
                                                else
                                                {
                                                    plan.PlanLanguageID = 0;
                                                    plan.PlanLanguageName = null;
                                                }
                                                    

                                                ProductDetail.PlanLists.Add(plan);

                                            }
                                        }
                                    }


                                        RecommendedCourseDetails.ProductPlanLists.Add(ProductDetail);
                                   
                                }


                            }


                        }

                        RecommendedCourseProductPlans.Add(RecommendedCourseDetails);
                      


                    }
                }


            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return RecommendedCourseProductPlans;
        }


        public List<Product> GetRecommendedProductList(int CourseID)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            List<Product> products = new List<Product>();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@CourseID", CourseID);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetRecommendedProductDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Product product = new();

                        product.ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]);
                        product.BrandID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrandID"]);
                        //product.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                        product.CourseID = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseID"]);


                        product.ProductCode = ds.Tables[0].Rows[i]["ProductCode"].ToString();
                        product.ProductName = ds.Tables[0].Rows[i]["ProductName"].ToString();
                        product.ShortDesc = ds.Tables[0].Rows[i]["ProductShortDesc"].ToString();
                        product.LongDesc = ds.Tables[0].Rows[i]["ProductLongDesc"].ToString();
                        product.ProductImage = ds.Tables[0].Rows[i]["ProductImage"].ToString();
                        product.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);
                        product.CategoryDetails.CategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CategoryID"]);
                        product.CategoryDetails.CategoryName = ds.Tables[0].Rows[i]["CategoryName"].ToString();

                        if (ds.Tables[0].Rows[i]["I_Language_ID"] == DBNull.Value || ds.Tables[0].Rows[i]["I_Language_Name"] == DBNull.Value)
                        {
                            product.ProductLangaugeID = 0;
                            product.ProductLanguageName = null;
                        }
                        else
                        {
                            product.ProductLangaugeID = Convert.ToInt32(ds.Tables[0].Rows[i]["I_Language_ID"]);
                            product.ProductLanguageName = ds.Tables[0].Rows[i]["I_Language_Name"].ToString();
                        }


                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                        {
                            if (product.ProductID == Convert.ToInt32(ds.Tables[1].Rows[j]["ProductID"]))
                            {
                                ProductConfig config = new();

                                config.ConfigID = Convert.ToInt32(ds.Tables[1].Rows[j]["ConfigID"]);
                                config.ConfigCode = ds.Tables[1].Rows[j]["ConfigCode"].ToString();
                                config.ConfigValue = ds.Tables[1].Rows[j]["ConfigValue"].ToString();
                                config.ConfigDisplayName = ds.Tables[1].Rows[j]["ConfigName"].ToString();

                                product.ProductConfigList.Add(config);
                            }
                        }

                        products.Add(product);
                    }
                }
            }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return products;
        }



        public StudentRegDetails UpdateStudentProfile(Registration reg)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();
            StudentRegDetails StudentRegDetail = new StudentRegDetails();


            try
            {
                SqlParameter[] sqlparams = new SqlParameter[14];

                sqlparams[0] = new SqlParameter("@CustomerID", reg.CustomerID);

                if(reg.FirstName != null && !String.IsNullOrEmpty(reg.FirstName))
                 sqlparams[1] = new SqlParameter("@FirstName", reg.FirstName);
                else
                 sqlparams[1] = new SqlParameter("@FirstName", DBNull.Value);

                if (reg.LastName != null && !String.IsNullOrEmpty(reg.LastName))
                    sqlparams[2] = new SqlParameter("@LastName", reg.LastName);
                else
                    sqlparams[2] = new SqlParameter("@LastName", DBNull.Value);

                if (reg.HighestEducationQualification != null && !String.IsNullOrEmpty(reg.HighestEducationQualification))
                    sqlparams[3] = new SqlParameter("@HighestEducationQualification", reg.HighestEducationQualification);
                else
                    sqlparams[3] = new SqlParameter("@HighestEducationQualification", DBNull.Value);

                if (reg.Gender != null && !String.IsNullOrEmpty(reg.Gender))
                    sqlparams[4] = new SqlParameter("@Gender", reg.Gender);
                else
                    sqlparams[4] = new SqlParameter("@Gender", DBNull.Value);

                
                sqlparams[5] = new SqlParameter("@DoB", reg.DoB);

                if (reg.City != null && !String.IsNullOrEmpty(reg.City))
                    sqlparams[6] = new SqlParameter("@City", reg.City);
                else
                    sqlparams[6] = new SqlParameter("@City", DBNull.Value);

                if (reg.State != null && !String.IsNullOrEmpty(reg.State))
                    sqlparams[7] = new SqlParameter("@State", reg.State);
                else
                    sqlparams[7] = new SqlParameter("@State", DBNull.Value);

                if (reg.Pincode != null && !String.IsNullOrEmpty(reg.Pincode))
                    sqlparams[8] = new SqlParameter("@Pincode", reg.Pincode);
                else
                    sqlparams[8] = new SqlParameter("@Pincode", DBNull.Value);

                if (reg.SecondLanguage != null && !String.IsNullOrEmpty(reg.SecondLanguage))
                    sqlparams[9] = new SqlParameter("@SecondLanguage", reg.SecondLanguage);
                else
                    sqlparams[9] = new SqlParameter("@SecondLanguage", DBNull.Value);

                if (reg.SocialCategory != null && !String.IsNullOrEmpty(reg.SocialCategory))
                    sqlparams[10] = new SqlParameter("@SocialCategory", reg.SocialCategory);
                else
                    sqlparams[10] = new SqlParameter("@SocialCategory", DBNull.Value);

                if (reg.GuardianName != null && !String.IsNullOrEmpty(reg.GuardianName))
                    sqlparams[11] = new SqlParameter("@GuardianName", reg.GuardianName);
                else
                    sqlparams[11] = new SqlParameter("@GuardianName", DBNull.Value);

                if (reg.GuardianMobileNo != null && !String.IsNullOrEmpty(reg.GuardianMobileNo))
                    sqlparams[12] = new SqlParameter("@GuardianMobileNo", reg.GuardianMobileNo);
                else
                    sqlparams[12] = new SqlParameter("@GuardianMobileNo", DBNull.Value);

                sqlparams[13] = new SqlParameter("@iBrandID", 109);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspUpdateStudentDetails", CommandType.StoredProcedure, sqlparams);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["CustomerID"] != DBNull.Value && ds.Tables[0].Rows[i]["RegID"] != DBNull.Value)
                        {
                            StudentRegDetail.CustomerID = ds.Tables[0].Rows[i]["CustomerID"].ToString();
                            StudentRegDetail.RegID = Convert.ToInt32(ds.Tables[0].Rows[i]["RegID"]);
                        }
                        else
                        {
                            StudentRegDetail.CustomerID = null;
                            StudentRegDetail.RegID = 0;
                        }


                        if (ds.Tables.Count > 1)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                if (ds.Tables[1].Rows[j]["I_Enquiry_Regn_ID"] != DBNull.Value)
                                    StudentRegDetail.EnquiryNos.Add(Convert.ToInt32(ds.Tables[1].Rows[j]["I_Enquiry_Regn_ID"]));       
                                
                            }
                               
                        }

                        if (ds.Tables.Count > 2)
                        {
                            for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            {
                                if (ds.Tables[2].Rows[j]["S_Student_ID"] != DBNull.Value)
                                    StudentRegDetail.StudentIDs.Add(ds.Tables[2].Rows[j]["S_Student_ID"].ToString());

                            }

                        }

                    }

                }

                }
            finally
            {
                if (dh != null)
                {
                    if (dh.DataConn != null)
                    {
                        dh.DataConn.Close();
                    }
                }
            }


            return StudentRegDetail;
        }






        #region PrivateMethods

        private string GeneratePaymentXML(List<PlanPayment> payments)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xTblPlanPayment;
            XmlElement xTblProductPayment;
            XmlElement xRowPlanPayment;
            XmlElement xRowProductPayment;

            xTblPlanPayment = (XmlElement)xDoc.CreateElement("TblPlanPayment");

            foreach(var p in payments)
            {
                xRowPlanPayment = (XmlElement)xDoc.CreateElement("RowPlanPayment");

                xRowPlanPayment.SetAttribute("PlanID", p.PlanID.ToString());
                xRowPlanPayment.SetAttribute("PaidAmount", p.PaidAmount.ToString());
                xRowPlanPayment.SetAttribute("PaidTax", p.PaidTax.ToString());
                xRowPlanPayment.SetAttribute("ProspectusAmt", p.ProspectusAmount.ToString());

                xTblProductPayment = (XmlElement)xDoc.CreateElement("TblProductPayment");

                foreach(var pr in p.ProductPaymentDetails)
                {
                    xRowProductPayment = (XmlElement)xDoc.CreateElement("RowProductPayment");

                    xRowProductPayment.SetAttribute("ProductID", pr.ProductID.ToString());
                    xRowProductPayment.SetAttribute("ProductCentreID", pr.ProductCentreID.ToString());
                    xRowProductPayment.SetAttribute("ProductFeePlanID", pr.ProductFeePlanID.ToString());
                    xRowProductPayment.SetAttribute("PaymentMode", pr.PaymentMode.ToString());

                    if(pr.CouponCode!=null)
                        xRowProductPayment.SetAttribute("CouponCode", pr.CouponCode);
                    else
                        xRowProductPayment.SetAttribute("CouponCode", "");
                    xRowProductPayment.SetAttribute("PaidAmount", pr.PaidAmount.ToString());
                    xRowProductPayment.SetAttribute("PaidTax", pr.PaidTax.ToString());
                    xRowProductPayment.SetAttribute("ProspectusAmount", pr.ProspectusAmount.ToString());
                    xRowProductPayment.SetAttribute("BatchID", pr.BatchID.ToString());

                    if (pr.SubscriptionDetails != null && pr.SubscriptionDetails.Authkey != null && pr.SubscriptionDetails.Authkey.Trim() != "")
                    {
                        xRowProductPayment.SetAttribute("SubscriptionPlanID", pr.SubscriptionDetails.SubscriptionPlanID.ToString());
                        xRowProductPayment.SetAttribute("AuthKey", pr.SubscriptionDetails.Authkey.ToString());
                        xRowProductPayment.SetAttribute("BillingPeriod", pr.SubscriptionDetails.BillingPeriod.ToString());
                        xRowProductPayment.SetAttribute("BillingStartDate", pr.SubscriptionDetails.BillingStartDate.ToString("yyyy-MM-dd"));
                        xRowProductPayment.SetAttribute("BillingEndDate", pr.SubscriptionDetails.BillingEndDate.ToString("yyyy-MM-dd"));
                        xRowProductPayment.SetAttribute("TotalBillingAmount", pr.SubscriptionDetails.TotalBillingAmount.ToString());
                    }
                    else
                    {
                        xRowProductPayment.SetAttribute("SubscriptionPlanID", "");
                        xRowProductPayment.SetAttribute("AuthKey", "");
                        xRowProductPayment.SetAttribute("BillingPeriod", "0");
                        xRowProductPayment.SetAttribute("BillingStartDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        xRowProductPayment.SetAttribute("BillingEndDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        xRowProductPayment.SetAttribute("TotalBillingAmount", "0");
                    }


                    xTblProductPayment.AppendChild(xRowProductPayment);
                }

                xRowPlanPayment.AppendChild(xTblProductPayment);
                xTblPlanPayment.AppendChild(xRowPlanPayment);
            }

            xDoc.AppendChild(xTblPlanPayment);


            return xDoc.InnerXml;
        }


        

        #endregion
    }
}
