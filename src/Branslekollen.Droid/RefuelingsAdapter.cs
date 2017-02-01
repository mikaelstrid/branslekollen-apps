using Android.App;
using Android.Views;
using Android.Widget;
using Branslekollen.Core.Domain.Models;

namespace Branslekollen.Droid
{
    public class RefuelingsAdapter : BaseAdapter<Refueling>
    {
        private readonly Refueling[] _items;
        private readonly Activity _context;

        public RefuelingsAdapter(Activity context, Refueling[] items)
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Refueling this[int position] => _items[position];

        public override int Count => _items.Length;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.RefuelingRowLayout, null);
            var refueling = _items[position];
            view.FindViewById<TextView>(Resource.Id.DateTextView).Text = refueling.RefuelingDate.ToString("dd MMM");
            view.FindViewById<TextView>(Resource.Id.VolumeTextView).Text = $"{refueling.NumberOfLiters:N2} l";
            view.FindViewById<TextView>(Resource.Id.TotalPriceTextView).Text = $"{refueling.NumberOfLiters * refueling.PricePerLiter:N0} kr";
            return view;
        }
    }
}