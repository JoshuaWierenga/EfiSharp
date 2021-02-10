// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using EfiSharp;

namespace System
{
    //TODO Fix Exceptions
    [StructLayout(LayoutKind.Auto)]
    public readonly struct DateTime
    {
        // Number of 100ns ticks per time unit
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        // Number of days in a non-leap year
        private const int DaysPerYear = 365;
        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097

        private const int DatePartYear = 0;
        private const int DatePartDayOfYear = 1;
        private const int DatePartMonth = 2;
        private const int DatePartDay = 3;

        private const ulong TicksMask = 0x3FFFFFFFFFFFFFFF;

        //TODO Support DateTimeKind
        // The data is stored as an unsigned 64-bit integer
        //   Bits 01-62: The value of 100-nanosecond ticks where 0 represents 1/1/0001 12:00am, up until the value
        //               12/31/9999 23:59:59.9999999
        //   Bits 63-64: A four-state value that describes the DateTimeKind value of the date time, with a 2nd
        //               value for the rare case where the date time is local, but is in an overlapped daylight
        //               savings time hour and it is in daylight savings time. This allows distinction of these
        //               otherwise ambiguous local times and prevents data loss when round tripping from Local to
        //               UTC time.
        private readonly ulong _dateData;

        // Constructs a DateTime from a given year, month, day, hour,
        // minute, and second.
        //
        public DateTime(int year, int month, int day, int hour, int minute, int second)
        {
            //TODO Support IsValidTimeWithLeapSeconds(int, int, int, int, int, int, DateTimeKind)
            //if (second != 60)
            {
                _dateData = DateToTicks(year, month, day) + TimeToTicks(hour, minute, second);
            }
            /*else
            {
                this = new(year, month, day, hour, minute, 59);
                //ValidateLeapSecond();
            }*/
        }
       
        private ulong UTicks => _dateData & TicksMask;

        // Returns the tick count corresponding to the given year, month, and day.
        // Will check the if the parameters are valid.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong DateToTicks(int year, int month, int day)
        {
            if (year < 1 || year > 9999 || month < 1 || month > 12 || day < 1)
            {
                return 0;
                //ThrowHelper.ThrowArgumentOutOfRange_BadYearMonthDay();
            }

            uint[] s_daysToMonth365 = {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
            uint[] s_daysToMonth366 = {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

            uint[] days = IsLeapYear(year) ? s_daysToMonth366 : s_daysToMonth365;
            if ((uint)day > days[month] - days[month - 1])
            {
                return 0;
                //ThrowHelper.ThrowArgumentOutOfRange_BadYearMonthDay();
            }

            uint n = DaysToYear((uint)year) + days[month - 1] + (uint)day - 1;

            s_daysToMonth365.Dispose();
            s_daysToMonth366.Dispose();
            //TODO: Check if this dispose is required, depends if memory is copied when days is made or just referenced
            days.Dispose();

            return n * (ulong)TicksPerDay;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint DaysToYear(uint year)
        {
            uint y = year - 1;
            uint cent = y / 100;
            return y * (365 * 4 + 1) / 4 - cent + cent / 4;
        }

        // Return the tick count corresponding to the given hour, minute, second.
        // Will check the if the parameters are valid.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong TimeToTicks(int hour, int minute, int second)
        {
            if ((uint)hour >= 24 || (uint)minute >= 60 || (uint)second >= 60)
            {
                return 0;
                //ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
            }

            int totalSeconds = hour * 3600 + minute * 60 + second;
            return (uint)totalSeconds * (ulong)TicksPerSecond;
        }

        //TODO Add GetDate(out int, out int, out int)
        // Returns a given date part of this DateTime.This method is used
        // to compute the year, day-of-year, month, or day part.
        private int GetDatePart(int part)
        {
            // n = number of days since 1/1/0001
            uint n = (uint)(UTicks / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            uint y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            uint y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4) y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            uint y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            uint y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4) y1 = 3;
            // If year was requested, compute and return it
            if (part == DatePartYear)
            {
                return (int)(y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1);
            }
            // n = day number within year
            n -= y1 * DaysPerYear;
            // If day-of-year was requested, return it
            if (part == DatePartDayOfYear) return (int)n + 1;

            uint[] s_daysToMonth365 = {
                0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
            uint[] s_daysToMonth366 = {
                0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

            // Leap year calculation looks different from IsLeapYear since y1, y4,
            // and y100 are relative to year 1, not year 0
            uint[] days = y1 == 3 && (y4 != 24 || y100 == 3) ? s_daysToMonth366 : s_daysToMonth365;
            // All months have less than 32 days, so n >> 5 is a good conservative
            // estimate for the month
            uint m = (n >> 5) + 1;
            // m = 1-based month number
            while (n >= days[m]) m++;
            // If month was requested, return it
            if (part == DatePartMonth) return (int)m;
            // Return 1-based day-of-month
            int result = (int)(n - days[m - 1] + 1);

            s_daysToMonth365.Dispose();
            s_daysToMonth366.Dispose();
            //TODO: Check if this dispose is required, depends if memory is copied when days is made or just referenced
            days.Dispose();
            return result;
        }

        // Returns the day-of-month part of this DateTime. The returned
        // value is an integer between 1 and 31.
        //
        public int Day => GetDatePart(DatePartDay);

        // Returns the hour part of this DateTime. The returned value is an
        // integer between 0 and 23.
        //
        public int Hour => (int)((uint)(UTicks / TicksPerHour) % 24);

        // Returns the minute part of this DateTime. The returned value is
        // an integer between 0 and 59.
        //
        public int Minute => (int)((UTicks / TicksPerMinute) % 60);

        // Returns the month part of this DateTime. The returned value is an
        // integer between 1 and 12.
        //
        public int Month => GetDatePart(DatePartMonth);

        // Returns a DateTime representing the current date and time. The
        // resolution of the returned value depends on the system timer.
        public static DateTime Now
        {
            get
            {
                unsafe
                {
                    UefiApplication.SystemTable->RuntimeServices->GetTime(out EFI_TIME time);
                    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
                }
            }
        }

        // Returns the second part of this DateTime. The returned value is
        // an integer between 0 and 59.
        //
        public int Second => (int)((UTicks / TicksPerSecond) % 60);

        // Returns the year part of this DateTime. The returned value is an
        // integer between 1 and 9999.
        //
        public int Year => GetDatePart(DatePartYear);

        // Checks whether a given year is a leap year. This method returns true if
        // year is a leap year, or false if not.
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeapYear(int year)
        {
            if (year < 1 || year > 9999)
            {
                return false;
                //ThrowHelper.ThrowArgumentOutOfRange_Year();
            }

            if ((year & 3) != 0) return false;
            if ((year & 15) == 0) return true;
            // return true/false not the test result https://github.com/dotnet/runtime/issues/4207
            return (uint)year % 25 != 0 ? true : false;
        }
    }
}
