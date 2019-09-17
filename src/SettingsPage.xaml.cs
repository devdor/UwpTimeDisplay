using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimeDisplayApp {

    public sealed partial class SettingsPage : Page {
        #region Fields and Properties
        public string Version {
            get {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        public List<Tuple<string, FontFamily>> FontList {
            get;
        } = new List<Tuple<string, FontFamily>>() {
            new Tuple<string, FontFamily>("Arial", new FontFamily("Arial")),
            new Tuple<string, FontFamily>("Comic Sans MS", new FontFamily("Comic Sans MS")),
            new Tuple<string, FontFamily>("Courier New", new FontFamily("Courier New")),
            new Tuple<string, FontFamily>("Segoe UI", new FontFamily("Segoe UI")),
            new Tuple<string, FontFamily>("Times New Roman", new FontFamily("Times New Roman")),
            new Tuple<string, FontFamily>("Verdana", new FontFamily("Verdana"))
        };

        public List<double> FontSizeList {
            get;
        } = new List<double>() {
            8,
            9,
            10,
            11,
            12,
            14,
            16,
            18,
            20,
            24,
            28,
            36,
            48,
            72
        };
        #endregion

        public SettingsPage() {
            this.InitializeComponent();

            Loaded += OnSettingsPageLoaded;
            this.lnkResetSettings.Click += LnkResetSettings_Click1;
            this.colCtrlBackground.FlyoutClosed += ColCtrlBackground_FlyoutClosed;
            this.colCtrlOff.FlyoutClosed += ColCtrlOff_FlyoutClosed;
            this.colCtrlOn.FlyoutClosed += ColCtrlOn_FlyoutClosed;
            this.cmbFontName.SelectionChanged += CmbFontName_SelectionChanged;
            this.cmbFontSize.SelectionChanged += CmbFontSize_SelectionChanged;
            this.toggleSwBorder.Toggled += ToggleSwBorder_Toggled;
            this.toggleSwEffects.Toggled += ToggleSwEffects_Toggled;
        }

        private void ToggleSwEffects_Toggled(object sender, RoutedEventArgs e) {
            if (!(sender is ToggleSwitch))
                return;

            if (AppSettings.IsRenderEffektActive != ((ToggleSwitch)sender).IsOn) {

                AppSettings.SaveValue(AppSettingsType.IsRenderEffectActive, ((ToggleSwitch)sender).IsOn.ToString());
            }
        }

        private void ToggleSwBorder_Toggled(object sender, RoutedEventArgs e) {

            if (!(sender is ToggleSwitch))
                return;

            if (AppSettings.IsItemBorderVisible != ((ToggleSwitch)sender).IsOn) {

                AppSettings.SaveValue(AppSettingsType.IsItemBorderVisible, ((ToggleSwitch)sender).IsOn.ToString());
            }
        }

        private void CmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (this.cmbFontSize.SelectedItem != null) {

                if (this.cmbFontSize.SelectedItem is string) {

                    if (Double.TryParse(this.cmbFontSize.SelectedItem.ToString(), out double fontSize)) {

                        AppSettings.SaveValue(AppSettingsType.FontSize, fontSize);
                    }
                    else {
                        AppSettings.SaveValue(AppSettingsType.FontSize, AppConstValues.DEFAULT_FONT_SIZE);
                    }
                }
                else if (this.cmbFontSize.SelectedItem is double) {

                    double fontSize = (double)this.cmbFontSize.SelectedItem;
                    if (fontSize != AppSettings.FontSize) {

                        AppSettings.SaveValue(AppSettingsType.FontSize, fontSize);
                    }
                }
            }
        }

        private void CmbFontName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (this.cmbFontName.SelectedItem != null) {

                Tuple<string, FontFamily> selItem = cmbFontName.SelectedItem as Tuple<string, FontFamily>;
                if (!selItem.Item1.Equals(AppSettings.FontName)) {

                    AppSettings.SaveValue(AppSettingsType.FontName, selItem.Item1);
                }
            }
        }

        private void ColCtrlOn_FlyoutClosed(object sender, EventArgs e) {
            if (this.colCtrlOn.SelectedBrush != null) {
                var colBrushOn = this.colCtrlOn.SelectedBrush as SolidColorBrush;
                var tmpColor = colBrushOn.Color.ToString();

                if (AppSettings.FontColorOn != tmpColor) {
                    AppSettings.SaveValue(AppSettingsType.FontColorOn, tmpColor);
                }
            }
        }

        private void ColCtrlOff_FlyoutClosed(object sender, EventArgs e) {
            if (this.colCtrlOff.SelectedBrush != null) {
                var colBrushOff = this.colCtrlOff.SelectedBrush as SolidColorBrush;
                var tmpColor = colBrushOff.Color.ToString();

                if (AppSettings.FontColorOff != tmpColor) {
                    AppSettings.SaveValue(AppSettingsType.FontColorOff, tmpColor);
                }
            }
        }

        private void ColCtrlBackground_FlyoutClosed(object sender, EventArgs e) {
            if (this.colCtrlBackground.SelectedBrush != null) {
                var colBrushBackground = this.colCtrlBackground.SelectedBrush as SolidColorBrush;
                var tmpColor = colBrushBackground.Color.ToString();

                if (AppSettings.ItemBackgroundColor != tmpColor) {
                    AppSettings.SaveValue(AppSettingsType.ItemBackgroundColor, tmpColor);
                }
            }
        }

        private async void LnkResetSettings_Click1(object sender, RoutedEventArgs e) {

            var resLoader = ResourceLoader.GetForCurrentView();
            MessageDialog showDialog = new MessageDialog(
                resLoader.GetString("ConfirmResetSettings"));

            showDialog.Commands.Add(
                new UICommand("Yes") {
                    Id = 0,
                    Label = resLoader.GetString("Yes")
                });

            showDialog.Commands.Add(
                new UICommand("No") {
                    Id = 1,
                    Label = resLoader.GetString("No")
                });

            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;

            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0) {

                AppSettings.SetDefault(ElementTheme.Dark);
                this.SetCurrentSettings();
            }
        }

        private void OnSettingsPageLoaded(object sender, RoutedEventArgs e) {
            this.SetCurrentSettings();
        }

        void SetCurrentSettings() {
            this.SetCurrentSettings(
                App.RootTheme,
                AppSettings.FontName,
                AppSettings.FontSize,
                AppSettings.FontColorOn,
                AppSettings.FontColorOff,
                AppSettings.ItemBackgroundColor,
                AppSettings.IsItemBorderVisible,
                AppSettings.IsRenderEffektActive);
        }

        void SetCurrentSettings(ElementTheme theme, string fontName, double fontSize, string fontColorOn, string fontColorOff, string itemBackColor, bool isBorderVisible, bool isRenderEffectActive) {

            // Theme
            var currentTheme = theme;
            (ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme.ToString())).IsChecked = true;

            // FontName
            if (String.IsNullOrEmpty(fontName))
                fontName = AppConstValues.DEFAULT_FONT_NAME;
            this.cmbFontName.SelectedItem = this.FontList.FirstOrDefault(obj => obj.Item1.Equals(fontName));

            // Font Size
            this.cmbFontSize.Text = fontSize.ToString();

            // Font Colors
            this.colCtrlOn.SelectedBrush = !String.IsNullOrEmpty(fontColorOn) ? Util.GetSolidColorBrush(fontColorOn) : Util.GetSolidColorBrush(AppConstValues.DEFAULT_FONT_COLOR_ON);
            this.colCtrlOff.SelectedBrush = !String.IsNullOrEmpty(fontColorOff) ? Util.GetSolidColorBrush(fontColorOff) : Util.GetSolidColorBrush(AppConstValues.DEFAULT_FONT_COLOR_OFF);

            // Item BackgroundColor
            this.colCtrlBackground.SelectedBrush = !String.IsNullOrEmpty(itemBackColor) ? Util.GetSolidColorBrush(itemBackColor) : Util.GetSolidColorBrush(AppConstValues.DEFAULT_ITEM_BACKGROUND_COLOR);

            this.toggleSwBorder.IsOn = isBorderVisible;
            this.toggleSwEffects.IsOn = isRenderEffectActive;
        }

        private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e) {

            var selectedThemeTag = ((RadioButton)sender)?.Tag?.ToString();
            if (selectedThemeTag != null) {

                var themeType = Util.GetEnum<ElementTheme>(selectedThemeTag);
                if (themeType != AppSettings.Theme) {
                    AppSettings.SaveValue(AppSettingsType.AppTheme, selectedThemeTag);
                }

                Util.SetTheme(themeType);
            }
        }
    }
}
