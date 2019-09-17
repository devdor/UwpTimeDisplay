using Prism.Mvvm;
using Windows.UI.Xaml.Media;

namespace TimeDisplayApp.Clock {
    public abstract class AbstractBaseItem : BindableBase {
        #region Fields and Properties        
        Brush _foregroundBrush;
        public Brush ForegroundBrush {
            get => _foregroundBrush;
            set => SetProperty(ref _foregroundBrush, value);
        }

        readonly Brush _foregroundBrushOn;
        readonly Brush _foregroundBrushOff;

        Brush _backgroundBrush;
        public Brush BackgroundBrush {
            get => _backgroundBrush;
            set => SetProperty(ref _backgroundBrush, value);
        }

        double _opacity = 1.0;
        public double Opacity {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }

        bool _isActive = false;
        public bool IsActive {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
        #endregion
        public AbstractBaseItem(Brush foregroundBrushOn, Brush foregroundBrushOff) {
            this._foregroundBrushOn = foregroundBrushOn;
            this._foregroundBrushOff = foregroundBrushOff;
            this.ForegroundBrush = foregroundBrushOn;
        }

        public void SetState(bool isActive) {

            this.IsActive = isActive;
            this.ForegroundBrush = isActive ? this._foregroundBrushOn : this._foregroundBrushOff;
        }
    }
}
