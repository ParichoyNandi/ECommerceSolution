using Entities;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Numerics;
using Utilities;

namespace ECommPortalDataAccess
{
    public class ECommPortalDBAccess : IEcommPortalDBAccess
    {
        private string _conn;

        public ECommPortalDBAccess(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("EcommPortalConnectionString");
        }
        public Plan GetPlanDetails(int planId)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            Plan plan = new Plan();

            int j = 0;
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@PlanID",planId);

                ds = dh.ExecuteDataSet("ECOMMERCE.uspGetPlanDetailsForAdmin", CommandType.StoredProcedure, sqlparams);

                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        plan.PlanID = Convert.ToInt32(ds.Tables[0].Rows[i]["PlanID"]);
                        plan.PlanName = Convert.ToString(ds.Tables[0].Rows[i]["PlanName"]);
                        plan.ValidTo = Convert.ToDateTime(ds.Tables[0].Rows[i]["ValidTo"]);
                        plan.IsPublished = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPublished"]);
                        plan.PlanLanguageName = Convert.ToString(ds.Tables[0].Rows[i]["I_Language_Name"]);
                        plan.PlanCode = Convert.ToString(ds.Tables[0].Rows[i]["PlanCode"]);
                    }


                    plan.PlanImage = Convert.ToString(ds.Tables[1].Rows[0]["ConfigValue"]);
                    plan.PlanCourseDuration = Convert.ToString(ds.Tables[1].Rows[1]["ConfigValue"]);
                    plan.PlanPrice = Convert.ToString(ds.Tables[1].Rows[2]["ConfigValue"]);
                    plan.PlanDiscountedPrice = Convert.ToString(ds.Tables[1].Rows[3]["ConfigValue"]);
                    plan.PlanDesc= Convert.ToString(ds.Tables[1].Rows[4]["ConfigValue"]);
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

            return plan;

        }
        public int UpdatePlanDetails(Plan plan)
        {
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            int i = 0;

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];


                sqlparams[0] = new SqlParameter("@PlanId", plan.PlanID);

                if (plan.PlanName != null)
                    sqlparams[1] = new SqlParameter("@PlanName", plan.PlanName);
                else
                    sqlparams[1] = new SqlParameter("@PlanName", DBNull.Value);

                
                    sqlparams[2] = new SqlParameter("@ValidTo", plan.ValidTo);

                    sqlparams[3] = new SqlParameter("@IsPublished", 1);

             


                i = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspUpdatePlanDetailsForAdmin", CommandType.StoredProcedure, sqlparams));


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
       

        public int UpdatePlanConfig(Plan plan)
        {
            int i = 0;
            DataHelper dh = new DataHelper(_conn);
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] sqlparams = new SqlParameter[6];
                sqlparams[0] = new SqlParameter("@PlanId", plan.PlanID);
                sqlparams[1] = new SqlParameter("@Img", plan.PlanImage);
                sqlparams[2] = new SqlParameter("@CourseDuration", plan.PlanCourseDuration);
                sqlparams[3] = new SqlParameter("@Price", plan.PlanPrice);
                sqlparams[4] = new SqlParameter("@DiscountedPrice", plan.PlanDiscountedPrice);
                sqlparams[5] = new SqlParameter("@Summary", plan.PlanDesc);



                i = Convert.ToInt32(dh.ExecuteScalar("ECOMMERCE.uspUpdatePlanConfigForAdmin", CommandType.StoredProcedure, sqlparams));
                
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

    }
}