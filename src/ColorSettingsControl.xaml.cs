using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimeDisplayApp {
    public sealed partial class ColorSettingsControl : UserControl {
        #region Fields and Properties
        public Brush SelectedBrush {
            get {
                return (Brush)GetValue(SelectedBrushProperty);
            }

            set {
                SetValue(SelectedBrushProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush",
                typeof(Brush),
                typeof(ColorSettingsControl), null);


        public string InfoText {
            get {
                return (string)GetValue(InfoTextProperty);
            }

            set {
                SetValue(InfoTextProperty, value);
            }
        }

        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register("InfoText",
                typeof(String),
                typeof(ColorSettingsControl), null);
        #endregion

        #region Events
        public event EventHandler<EventArgs> FlyoutClosed;
        #endregion

        public ColorSettingsControl() {
            this.InitializeComponent();
            this.flyout.Opening += Flyout_Opening;
            this.flyout.Closed += Flyout_Closed;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnOk.Click += BtnOk_Click;
        }

        private void Flyout_Closed(object sender, object e) {
            this.FlyoutClosed?.Invoke(sender, new EventArgs());
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {

            this.SelectedBrush = new SolidColorBrush(this.colPicker.Color);
            this.flyout.Hide();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            this.flyout.Hide();
        }

        private void Flyout_Opening(object sender, object e) {

            if (this.SelectedBrush != null) {
                this.colPicker.Color = ((SolidColorBrush)this.SelectedBrush).Color;
            }
        }
    }
}
