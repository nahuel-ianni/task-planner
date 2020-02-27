using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace SystemIntegration.Managers
{
    /// <summary>
    /// Manager that allows the creation and modification of appointments on the
    /// Windows 10 calendar system.
    /// </summary>
    public static class CalendarManager
    {
        /// <summary>
        /// Edits the new appointment asynchronous.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>Id of the created appointment.</returns>
        public static async Task<string> EditNewAppointmentAsync(Appointment appointment)
        {
            return await AppointmentManager.ShowEditNewAppointmentAsync(appointment);
        }

        /// <summary>
        /// Edits the new appointment asynchronous.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="startTime">The start time.</param>
        /// <returns>Id of the created appointment.</returns>
        public static async Task<string> EditNewAppointmentAsync(string subject, DateTime startTime)
        {
            var appointment = GetAppointment(subject, startTime);

            return await EditNewAppointmentAsync(appointment);
        }

        /// <summary>
        /// Gets a new appointment.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="details">The details.</param>
        /// <param name="location">The location.</param>
        /// <param name="duration">The duration in minutes.</param>
        /// <param name="reminder">The reminder in minutes.</param>
        /// <param name="appointmentSensitivity">The appointment sensitivity.</param>
        /// <param name="appointmentBusyStatus">The appointment busy status.</param>
        /// <returns>A new appointment instance.</returns>
        public static Appointment GetAppointment(
            string subject,
            DateTime startTime,
            string details = null,
            string location = null,
            double duration = 0,
            double reminder = 0,
            AppointmentSensitivity appointmentSensitivity = AppointmentSensitivity.Private,
            AppointmentBusyStatus appointmentBusyStatus = AppointmentBusyStatus.Free)
        {
            var appointment = new Appointment();
            appointment.Subject = subject;
            appointment.StartTime = startTime;

            appointment.Details = string.IsNullOrEmpty(details) ? string.Empty : details;
            appointment.Location = string.IsNullOrEmpty(location) ? string.Empty : location;
            appointment.Sensitivity = appointmentSensitivity;
            appointment.BusyStatus = appointmentBusyStatus;
            appointment.Duration = TimeSpan.FromMinutes(duration);
            appointment.Reminder = TimeSpan.FromMinutes(reminder);

            return appointment;
        }
    }
}
