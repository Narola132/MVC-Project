using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShraddhaDemo.Models
{
    public class DemoListViewModel
    {
        public UsersViewModel UsersViewModel { get; set; }
        public List<UsersViewModel> UsersListViewModel { get; set; }
    }
    public class UsersViewModel
    {

        [Display(Name = "Id")]
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First name")]
        public string firstname { get; set; }
        [Display(Name = "Last name")]

        [Required(ErrorMessage = "Last name is required.")]
        public string lastname { get; set; }
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string address { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? Dateofbirth { get; set; }
        [Display(Name = "Joining Date")]
        public DateTime? Joiningdate { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? modifed_date { get; set; }
    }
    public class JsonResponse
    {
        public int status { get; set; }
        public string Message { get; set; }
        public string ExtraProperty { get; set; }
        public long CommonId { get; set; }
        public dynamic Data { get; set; }
        public string StatusChange { get; set; }
    }
}