using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Branslekollen.Droid
{
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        private Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            var datePickerFragment = new DatePickerFragment {_dateSelectedHandler = onDateSelected};
            return datePickerFragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var now = DateTime.Now;
            var dialog = new DatePickerDialog(Activity, this, now.Year, now.Month, now.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            var selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _dateSelectedHandler(selectedDate);
        }
    }
}