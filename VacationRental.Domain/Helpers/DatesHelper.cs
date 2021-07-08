using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.Helpers
{
    public static class DatesHelper
    {
        /// <summary>
        /// Validates if 2 Dates overlap each other
        /// </summary>
        /// <param name="startX">Date 1: Start Date</param>
        /// <param name="endX">Date 1: End Date</param>
        /// <param name="startY">Date 2: Start Date</param>
        /// <param name="endY">Date 2: End Date</param>
        /// <returns></returns>
        public static bool TimesOverlap(DateTime startX, DateTime endX, DateTime startY, DateTime endY) 
        {
            return ((startX <= startY.Date && endX > startY.Date)
                    || (startX < endY && endX >= endY)
                    || (startX > startY && endX < endY));
        }
    }
}
