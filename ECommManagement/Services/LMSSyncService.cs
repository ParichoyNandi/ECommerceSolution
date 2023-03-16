﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommManagement.Services
{
    public class LMSSyncService
    {
        private readonly string _baseurl="http://192.168.1.213:85/LMS_TEST/";
        private readonly string _authkey= "feji84nrk389393jf!76766";
        public async Task AddNewStudent(int StudentID=0)
        {
            try
            {
                HttpClient client = new HttpClient();

                string uri = _baseurl + "api/LMSSync/AddStudent/?StudentID="+StudentID.ToString();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", _authkey);

            
                await client.GetAsync(uri);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateStudentBatch(int StudentID = 0)
        {
            try
            {
                HttpClient client = new HttpClient();

                string uri = _baseurl + "api/LMSSync/UpdateStudentBatch/?StudentID=" + StudentID.ToString();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", _authkey);


                await client.GetAsync(uri);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
