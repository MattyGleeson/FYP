using Hotel_Booking_System.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel_Booking_System.Controllers.ControllerExtensions
{
    public abstract class MessageControllerBase : Controller
    {
        public MessageControllerBase()
        {
            Toastr = new Toastr();
        }
        public Toastr Toastr { get; set; }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            return Toastr.AddToastMessage(title, message, toastType);
        }
    }
}