﻿using LibraryData.Models;
using System;
using System.Collections.Generic;

namespace LibraryServices
{
    public class DataHelpers
    {
        public static List<string> HumanizeBizHours(IEnumerable<BranchHours> branchHours)
        {
            var hours = new List<string>();
            foreach(var time in branchHours)
            {
                var day = HumnizDay(time.DayOfWeek);
                var openTime = HumanizeTime(time.OpenTime);
                var closeTime = HumanizeTime(time.CloseTime);

                var timeEntry = $"{day} {openTime} to {closeTime}";
                hours.Add(timeEntry);

            }
            return hours;
        }

        public static string HumnizDay(int number)
        {
            //Our data Correlates 1->Sunday ,so substract
            return Enum.GetName(typeof(DayOfWeek),number -1);
        }
        public static string HumanizeTime(int time)
        {
            return TimeSpan.FromHours(time).ToString("hh':'mm");

        }
    }
}
