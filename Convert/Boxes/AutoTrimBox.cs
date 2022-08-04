namespace PDFManipulate.Boxes
{
   
    public class AutoTrimBox : AbstractPageBox
    {
        private double _bleedsFrom;
        private double _bleedsTo;

        public double BleedsFrom
        {
            get =>_bleedsFrom;
            set => _bleedsFrom = value < 0 ? 0 : value;
        }

        public double BleedsTo
        {
            get => _bleedsTo;
            set => _bleedsTo = value < _bleedsFrom ? _bleedsFrom : value;
        }
    }
}
