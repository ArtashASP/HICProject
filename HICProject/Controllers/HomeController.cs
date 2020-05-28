using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HICProject.Models;
using WebApplication1.Models.HIC;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using HICProject.Models.HIC;
using Microsoft.AspNetCore.Authentication;

namespace HICProject.Controllers
{
    public class HomeController : Controller
    {
        private PatientContext db;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(PatientContext context,
                            SignInManager<ApplicationUser> signInManager)
        {
            db = context;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPatient(Patients patient)
        {
            patient.Email = User.Identity.Name;
            db.Patients.Add(patient);
            db.SaveChanges();
            return View("start");
        }

        [HttpGet]
        public IActionResult AddStaff()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStaff(Staff staff)
        {
            staff.Email = User.Identity.Name;
            db.Staffs.Add(staff);
            db.SaveChanges();
            return View("start");
        }

        [HttpGet]
        public IActionResult AddAppointment(int id)
        {
            ViewData["Id"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult AddAppointment(Appointment appointment, int id)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var a = from p in db.Patients.ToList()
                    where p.Email == userName
                    select p.Id;
            appointment.Created = DateTime.Now;
            appointment.patientId = a.FirstOrDefault();
            appointment.doctorId = id;
            appointment.Id = 0;
            appointment.Status = Status.Proposed.ToString();

            switch (appointment.Priority)
            {
                case "0":
                    appointment.Priority = Priority.Low.ToString();
                    break;

                case "1":
                    appointment.Priority = Priority.Middle.ToString();
                    break;

                case "2":
                    appointment.Priority = Priority.High.ToString();
                    break;
            }
            db.Appointments.Add(appointment);

            db.SaveChanges();

            return View("start");
        }

        [HttpGet]
        public IActionResult AddPatientsMedication()
        {
            ViewData["Patients"] = db.Patients.ToList();
            ViewData["Medications"] = db.Medications.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientsMedication(Patients_Medication medication)
        {
            db.Patients_Medications.Add(medication);
            await db.SaveChangesAsync();
            return View("Index");
        }

        [HttpGet]
        public IActionResult AddMedication()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMedication(Medication medication)
        {
            db.Medications.Add(medication);
            db.SaveChanges();
            return View("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Patient()
        {
            List<Patients> p = db.Patients.ToList();
            return View(p);
        }

        public IActionResult Staff()
        {
            List<Staff> p = db.Staffs.ToList();
            return View(p);
        }

        [AllowAnonymous]
        public IActionResult Start()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DoctorsList()
        {
            List<Staff> doc = db.Staffs.ToList();
            return View(doc);
        }

        public IActionResult Schedule()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var appoint = db.Appointments.ToList();
            var docs = from s in db.Staffs.ToList()
                       where s.Email == userName
                       select s.Id;
            List<Appointment> result = new List<Appointment>();
            foreach (var a in appoint)
            {
                foreach (var d in docs)
                {
                    if (a.doctorId == d)
                    {
                        result.Add(a);
                    }
                }
            }
            return View(result);
        }

        public IActionResult ManageAppointment(int patientId)
        {
            var docs = from s in db.Appointments.ToList()
                       where s.patientId == patientId
                       select s;
            /* var pat = from s in db.Staffs.ToList()
                       where s.Id == patientId
                       select s;*/

            return View(docs);
        }

        /* [HttpPost]
         public IActionResult ManageAppointment(int patientId)
         {
             var docs = from s in db.Appointments.ToList()
                        where s.patientId == patientId
                        select s;
             *//* var pat = from s in db.Staffs.ToList()
                        where s.Id == patientId
                        select s;*//*

             return View(docs);
         }*/

        public IActionResult PatientAppointment()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var pat = from s in db.Patients.ToList()
                      where s.Email == userName
                      select s.Id;
            var appoint = from s in db.Appointments.ToList()
                          where s.patientId == pat.First()
                          select s.Status;
            return View();
        }

        [Route("api/ajax")]
        public void getAjax(string selval, int inp1)
        {
            /*var appoint = from s in db.Appointments.ToList()
                          where s.patientId == inp1
                          select s;*/
            Appointment appoint = db.Appointments.FirstOrDefault(x => x.patientId == inp1);
            appoint.Status = selval;
            db.Appointments.Update(appoint);
            db.SaveChanges();
        }
    }
}