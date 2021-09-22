using ShraddhaDemo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShraddhaDemo.Controllers
{
    public class DemoResultController : Controller
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);
        }
        public ActionResult DemoResult()
        {
            var viewModel = new DemoListViewModel();
            return View(viewModel);
        }

        public ActionResult GetDataEntry(int id = 0)
        {
            var message = "";
            var useModel = new UsersViewModel();
            try
            {
                if (id > 0)
                    ViewBag.IsDataEntry = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                    message = ex.InnerException.Message;
            }
            ViewBag.Message = message;
            return PartialView("_AddDataEntry", useModel);
        }

        [HttpPost]
        public ActionResult InsertUpdateDataEntry(UsersViewModel objUser)
        {
            var JsonResponse = new JsonResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    if (objUser.ID > 0)
                    {
                        objUser.modifed_date = DateTime.Now;
                        var result = UpdateUser(objUser);
                        if (result)
                        {
                            JsonResponse.status = 1;
                            JsonResponse.Message = "Data has been updated successfully";
                        }
                        else JsonResponse.Message = "Something has issue";
                    }
                    else
                    {
                        objUser.created_date = DateTime.Now;
                        objUser.modifed_date = DateTime.Now;
                        var result = AddUser(objUser);
                        if (result)
                        {
                            JsonResponse.status = 1;
                            JsonResponse.Message = "Data has been entered successfully";
                        }
                        else JsonResponse.Message = "Something has issue";
                    }
                }
                else
                {
                    JsonResponse.Message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                JsonResponse.Message = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                    JsonResponse.Message = ex.InnerException.Message;
            }
            return Json(JsonResponse, JsonRequestBehavior.AllowGet);
        }
        public bool AddUser(UsersViewModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("AddUserDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@firstname", obj.firstname);
            com.Parameters.AddWithValue("@lastname", obj.lastname);
            com.Parameters.AddWithValue("@address", obj.address);
            com.Parameters.AddWithValue("@city", obj.city);
            com.Parameters.AddWithValue("@state", obj.state);
            com.Parameters.AddWithValue("@Dateofbirth", obj.Dateofbirth);
            com.Parameters.AddWithValue("@Joiningdate", obj.Joiningdate);
            com.Parameters.AddWithValue("@modified_date", DateTime.Now);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateUser(UsersViewModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("UpdateUsersDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@firstname", obj.firstname);
            com.Parameters.AddWithValue("@ID", obj.ID);
            com.Parameters.AddWithValue("@lastname", obj.lastname);
            com.Parameters.AddWithValue("@address", obj.address);
            com.Parameters.AddWithValue("@city", obj.city);
            com.Parameters.AddWithValue("@state", obj.state);
            com.Parameters.AddWithValue("@Dateofbirth", obj.Dateofbirth);
            com.Parameters.AddWithValue("@Joiningdate", obj.Joiningdate);
            //com.Parameters.AddWithValue("@modifed_date", DateTime.Now);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult ShowResultList()
        {
            var list = new List<UsersViewModel>();
            try
            {
                var dataList = GetAllUsers();
                if (dataList != null && dataList.Any())
                    list = dataList;
            }
            catch (Exception ex)
            {
            }
            return PartialView("_ShowResults", list);
        }
        public List<UsersViewModel> GetAllUsers()
        {
            connection();
            List<UsersViewModel> userList = new List<UsersViewModel>();
            SqlCommand com = new SqlCommand("GetUser", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {
                userList.Add(
                    new UsersViewModel
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        firstname = Convert.ToString(dr["firstname"]),
                        lastname = Convert.ToString(dr["lastname"]),
                        city = Convert.ToString(dr["city"]),
                        address = Convert.ToString(dr["address"]),
                        state = Convert.ToString(dr["state"]),
                        Dateofbirth = (dr["Dateofbirth"] != DBNull.Value ? Convert.ToDateTime(dr["Dateofbirth"]) : new DateTime()),
                        Joiningdate = (dr["Joiningdate"] != DBNull.Value ? Convert.ToDateTime(dr["Joiningdate"]) : new DateTime())
                    }
               );
            }

            return userList;
        }
    }
}