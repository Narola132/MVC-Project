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
    public class UsersController : Controller
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);
        }
        // GET: Users
        public ActionResult Index()
        {
            var usersList = new List<UsersViewModel>();
            try
            {
                ModelState.Clear();
                usersList = GetAllUsers();
            }
            catch (Exception ex)
            {
                usersList = new List<UsersViewModel>();
                ViewBag.Message = ex;
            }
            return View(usersList);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public bool AddUser(UsersViewModel obj)
        {
            connection();
            SqlCommand com = new SqlCommand("AddNewUserDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@firstname", obj.firstname);
            com.Parameters.AddWithValue("@lastname", obj.lastname);
            com.Parameters.AddWithValue("@address", obj.address);
            com.Parameters.AddWithValue("@city", obj.city);
            com.Parameters.AddWithValue("@state", obj.state);
            com.Parameters.AddWithValue("@Dateofbirth", obj.Dateofbirth);
            com.Parameters.AddWithValue("@Joiningdate", obj.Joiningdate);
            com.Parameters.AddWithValue("@modifed_date", DateTime.Now);
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

        public List<UsersViewModel> GetAllUsers()
        {
            connection();
            List<UsersViewModel> userList = new List<UsersViewModel>();
            SqlCommand com = new SqlCommand("GetUsers", con);
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
        //To Update Employee details    
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
            com.Parameters.AddWithValue("@modifed_date", DateTime.Now);
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
        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(UsersViewModel obj, FormCollection frm)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (AddUser(obj)) ViewBag.Message = "Employee details added successfully";
                }
                if(frm != null && frm["ISpartial"] != null)
                {
                    var isPartial = Convert.ToBoolean(frm["ISpartial"].ToString());
                    if (!isPartial)
                        return RedirectToAction("Index");
                    else return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                
            }
            return View();
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            var uDetail = new UsersViewModel();
            try
            {
                var userDetail = GetAllUsers().Find(user => user.ID == id);
                if (userDetail != null && userDetail.ID > 0)
                    uDetail = userDetail;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
            }
            return View(uDetail);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UsersViewModel obj)
        {
            try
            {
                // TODO: Add update logic here
                UpdateUser(obj);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
