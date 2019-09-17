using System;
using TimeDisplayApp.Clock;
using TimeDisplayApp.ViewModels;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TimeDisplayApp {
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class HomePage : Page {
        #region Fields and Properties
        DateTime? _dtLastEffectRender;
        TimeSpan EFFECT_INTERVAL = TimeSpan.FromSeconds(50);
        #endregion

        public HomePage() {
            this.InitializeComponent();

            var vm = new HomePageVM() {
                ForegroundBrushOn = Util.GetSolidColorBrush(AppSettings.FontColorOn),
                ForegroundBrushOff = Util.GetSolidColorBrush(AppSettings.FontColorOff),
                ItemBackgroundBrush = Util.GetSolidColorBrush(AppSettings.ItemBackgroundColor)
            };
            this.DataContext = vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {

            if (this.DataContext == null
                || !(this.DataContext is HomePageVM))
                return;

            if (AppSettings.LastUpdated == null) {

                AppSettings.SetDefault(ElementTheme.Dark);
                AppSettings.SaveValue(AppSettingsType.SettingsLastUpdated, DateTime.Now.ToString());
            }

            Util.SetTheme(AppSettings.Theme);
            var vm = this.DataContext as HomePageVM;
            vm.ForegroundBrushOn = Util.GetSolidColorBrush(AppSettings.FontColorOn);
            vm.ForegroundBrushOff = Util.GetSolidColorBrush(AppSettings.FontColorOff);
            vm.ItemBackgroundBrush = Util.GetSolidColorBrush(AppSettings.ItemBackgroundColor);


            this._dtLastEffectRender = null;
            vm.Init(ClockData.ContentDe);
            this.InitLetterClock(vm);
            vm.SetStateAll(false);

            var navToCompletedTimer = new DispatcherTimer();
            navToCompletedTimer.Tick += NavToCompletedTimer_Tick;
            navToCompletedTimer.Interval = TimeSpan.FromMilliseconds(1200);
            navToCompletedTimer.Start();
        }

        private void NavToCompletedTimer_Tick(object sender, object e) {

            if (sender != null
                && sender is DispatcherTimer) {

                ((DispatcherTimer)sender).Stop();

                var vm = this.DataContext as HomePageVM;
                this.InitTimer(vm);
            }
        }

        void InitLetterClock(HomePageVM vm) {

            if (vm == null)
                throw new ArgumentNullException("MainViewModel");

            Random rand = new Random();
            var rectWidthAndHeight = Util.GetMaxWidthAndHeight(AppSettings.FontSize);
            for (int i = 0; i < vm.NumRows; i++) {

                var spName = $"spTow_{i}";
                var spRow = new StackPanel() {
                    Orientation = Orientation.Horizontal,
                    Name = spName
                };

                for (int j = 0; j < vm.NumColumns; j++) {

                    // Textblock
                    var tbLetter = new TextBlock() {
                        FontSize = AppSettings.FontSize,
                        FontWeight = FontWeights.Bold,
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Width = rectWidthAndHeight + AppConstValues.CHAR_WIDTH_MARGIN,
                        FontFamily = new FontFamily(AppSettings.FontName),
                        Foreground = Util.GetSolidColorBrush(AppSettings.FontColorOn)
                    };

                    BindingOperations.SetBinding(
                        tbLetter,
                        TextBlock.TextProperty,
                        new Binding {
                            Source = vm.Letters,
                            Path = new PropertyPath($"[{i}_{j}].Content"),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });

                    BindingOperations.SetBinding(
                        tbLetter,
                        TextBlock.ForegroundProperty,
                        new Binding {
                            Source = vm.Letters,
                            Path = new PropertyPath($"[{i}_{j}].ForegroundBrush"),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });

                    // Border
                    var border = new Border() {
                        BorderBrush = AppSettings.IsItemBorderVisible ? Util.GetSolidColorBrush(AppConstValues.DEFAULT_BORDER_BRUSH) : Util.GetSolidColorBrush(AppConstValues.DEFAULT_TRANSPARENCY_BRUSH),
                        BorderThickness = new Thickness(0.5),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(1)
                    };

                    // Background
                    BindingOperations.SetBinding(
                        border,
                        Border.BackgroundProperty,
                        new Binding {
                            Source = vm.Letters,
                            Path = new PropertyPath($"[{i}_{j}].BackgroundBrush"),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });

                    // Opacity                    
                    BindingOperations.SetBinding(
                        border,
                        Border.OpacityProperty,
                        new Binding {
                            Source = vm.Letters,
                            Path = new PropertyPath($"[{i}_{j}].Opacity"),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });

                    border.Child = tbLetter;
                    spRow.Children.Add(border);
                }

                this.spLetters.Children.Add(spRow);
            }

            // Minutes Canvas
            var canvBorderSize = AppSettings.FontSize / 5;
            if (canvBorderSize < 2)
                canvBorderSize = 2;

            for (int n = 0; n < 4; n++) {

                var canv = new Canvas {
                    Width = canvBorderSize,
                    Height = canvBorderSize,
                    Margin = new Thickness(canvBorderSize * 1.2f),

                    Background = Util.GetSolidColorBrush(AppSettings.FontColorOn)
                };

                BindingOperations.SetBinding(
                    canv,
                    Canvas.BackgroundProperty, new Binding {
                        Source = vm.Minutes,
                        Path = new PropertyPath($"[{n}].ForegroundBrush"),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

                // Opacity                    
                BindingOperations.SetBinding(
                    canv,
                    Border.OpacityProperty,
                    new Binding {
                        Source = vm.Minutes,
                        Path = new PropertyPath($"[{n}].Opacity"),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

                // Opacity Animation
                var dAnimation = new DoubleAnimation() {
                    AutoReverse = false,
                    EnableDependentAnimation = true,
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(rand.Next(600, 2400)))
                };

                Storyboard.SetTarget(dAnimation, canv);
                Storyboard.SetTargetProperty(dAnimation, "Opacity");
                var sb = new Storyboard();
                sb.Children.Add(dAnimation);
                sb.Begin();

                this.spMinutes.Children.Add(canv);
            }
        }

        private void DispatcherTimer_Tick(object sender, object e) {

            if (this.DataContext != null
                && this.DataContext is HomePageVM) {

                var vm = this.DataContext as HomePageVM;

                var dt = DateTime.Now;
                vm.ShowCurrentTime(dt.Hour, dt.Minute, LanguageType.LANGUAGE_DE_DE);


                Random rand = new Random();
                if (rand.NextDouble() >= 0.5)
                    this.RunEffect(EffectType.Mode1, 1, 0);
                else
                    this.RunEffect(EffectType.Mode2, 1, 0);
            }
        }

        void InitTimer(HomePageVM vm) {

            if (vm == null)
                throw new ArgumentNullException("HomePageVM");

            var dt = DateTime.Now;
            vm.ShowCurrentTime(dt.Hour, dt.Minute, LanguageType.LANGUAGE_DE_DE);

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        void RunEffect(EffectType mode, double from, double to) {

            if (!AppSettings.IsRenderEffektActive)
                return;

            if (spLetters.Children == null)
                return;

            if (this._dtLastEffectRender != null) {

                if ((DateTime.Now - this._dtLastEffectRender) < EFFECT_INTERVAL) {
                    return;
                }
            }

            Random rand = new Random();
            double delay = 200;
            foreach (StackPanel spRow in this.spLetters.Children) {

                if (spRow.Children == null)
                    continue;

                foreach (Border border in spRow.Children) {

                    DoubleAnimation dAnimation = null;
                    switch (mode) {
                        case EffectType.Mode1:
                            dAnimation = new DoubleAnimation() {
                                AutoReverse = true,
                                EnableDependentAnimation = true,
                                From = from,
                                To = to,
                                Duration = new Duration(TimeSpan.FromMilliseconds(rand.Next(600, 2400)))
                            };
                            break;
                        case EffectType.Mode2:
                            dAnimation = new DoubleAnimation() {
                                AutoReverse = true,
                                EnableDependentAnimation = true,
                                From = from,
                                To = to,
                                BeginTime = TimeSpan.FromMilliseconds(delay),
                                Duration = new Duration(TimeSpan.FromMilliseconds(1200)),
                            };

                            delay += 10;
                            break;
                    }

                    Storyboard.SetTarget(dAnimation, border);
                    Storyboard.SetTargetProperty(dAnimation, "Opacity");
                    var sb = new Storyboard();
                    sb.Children.Add(dAnimation);
                    sb.Begin();
                }
            }

            this._dtLastEffectRender = DateTime.Now;
        }
    }
}
