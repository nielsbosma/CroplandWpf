﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CroplandWpf.Helpers
{
	public enum DateIntervalType
	{
		Today,
		Yesterday,
		Last7Days,
		Last14Days,
		Last30Days,
		ThisWeek,
		LastWeek,
		ThisMonth,
		LastMonth
	}

	public struct DateInterval
	{
		public DateTime Date1;
		public DateTime Date2;
		public string Format;

		public DateInterval(DateTime dt1, DateTime dt2, string format = "dd MMMM yyyy")
		{
			Date1 = dt1;
			Date2 = dt2;
			Format = format;
		}

		public string GetFormattedValue(string dateFormat)
		{
			string dateString1 = String.IsNullOrWhiteSpace(dateFormat) ? Date1.ToString() : Date1.ToString(dateFormat);
			string dateString2 = String.IsNullOrWhiteSpace(dateFormat) ? Date2.ToString() : Date2.ToString(dateFormat);
			return String.Format("{0} - {1}", dateString1, dateString2);
		}

		public override string ToString()
		{
			return String.Format("{0} - {1}", Date1.ToString(Format), Date2.ToString(Format));
		}
	}

	public static class DateTimeHelper
	{
		public static void GetInterval(DateIntervalType intervalType, out DateTime dateTime1, out DateTime dateTime2)
		{
			DateTime dt1 = default(DateTime),
					 dt2 = default(DateTime);

			DateTime now = DateTime.Now;

			switch (intervalType)
			{
				case DateIntervalType.Today:
					dt1 = now.Date;
					dt2 = dt1.AddDays(1.0).AddSeconds(-1.0);
					break;

				case DateIntervalType.Yesterday:
					dt1 = now.Date.AddDays(-1.0).Date;
					dt2 = now.Date.AddSeconds(-1.0);
					break;

				case DateIntervalType.ThisWeek:
					dt1 = now.Date.AddDays(-1 * (int)DateTime.Now.DayOfWeek).AddDays(1.0);
					dt2 = now.Date.AddDays(7 - (int)now.DayOfWeek).AddDays(1.0).AddSeconds(-1.0);
					break;

				case DateIntervalType.ThisMonth:
					dt1 = new DateTime(now.Year, now.Month, 1);
					dt2 = dt1.AddMonths(1).AddSeconds(-1.0);
					break;

				case DateIntervalType.LastMonth:
					dt1 = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
					dt2 = new DateTime(now.Year, now.Month, 1).AddSeconds(-1);
					break;

				case DateIntervalType.LastWeek:
					dt1 = now.Date.AddDays(-1 * (int)now.DayOfWeek).AddDays(-6);
					dt2 = dt1.AddDays(7).AddSeconds(-1.0);
					break;

				case DateIntervalType.Last7Days:
					dt1 = now.Date.AddDays(-7.0);
					dt2 = now.Date.AddDays(1.0).AddSeconds(-1.0);
					break;

				case DateIntervalType.Last14Days:
					dt1 = now.Date.AddDays(-14.0);
					dt2 = now.Date.AddDays(1.0).AddSeconds(-1.0);
					break;

				case DateIntervalType.Last30Days:
					dt1 = now.Date.AddDays(-30.0);
					dt2 = now.Date.AddDays(1.0).AddSeconds(-1.0);
					break;
				default: break;
			}

			dateTime1 = dt1;
			dateTime2 = dt2;
		}

		public static DateTime NormalizeIntervalStart(DateTime dt)
		{
			if (dt.TimeOfDay.TotalSeconds != 0)
				dt = dt.AddSeconds(-dt.TimeOfDay.TotalSeconds);
			return dt;
		}

		public static DateTime NormalizeIntervalEnd(DateTime dt)
		{
			TimeSpan ts = new TimeSpan(23, 59, 59);
			if (dt.TimeOfDay != ts)
				dt = dt.AddSeconds((ts - dt.TimeOfDay).TotalSeconds);
			return dt;
		}
	}

	public class DateIntervalPreset
	{
		public DateIntervalType Type { get; private set; }

		public string FriendlyName { get; set; }

		public DateTime Date1 { get; private set; }

		public DateTime Date2 { get; private set; }

		public DateIntervalPreset(DateIntervalType type, string friendlyName = "")
		{
			Type = type;
			FriendlyName = friendlyName;
			DateTimeHelper.GetInterval(Type, out DateTime dt1, out DateTime dt2);
			Date1 = dt1;
			Date2 = dt2;
		}

		public void GetValues(out DateTime dateTime1, out DateTime dateTime2)
		{
			dateTime1 = Date1;
			dateTime2 = Date2;
		}

		public bool Equals(DateTime dt1, DateTime dt2)
		{
			return dt1.Equals(Date1) && dt2.Equals(Date2);
		}

		public override string ToString()
		{
			return String.Format("{0} - {1}", Date1, Date2);
		}
	}
}
