using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HICProject.Models
{
    public enum AppointmentEnum
    {
        Proposed,
        Pending,
        Booked,
        Arrived,
        Fulfilled,
        Cancelled,
        Noshow,
        EnteredInError,
        CheckedIn,
        Waitlist
    }
}
