using Microsoft.Services.Store.Engagement;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace TimeDisplayApp {
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();

            var resLoader = ResourceLoader.GetForCurrentView();
            this.navView.Loaded += NavView_Loaded;
            this.navView.ItemInvoked += NavView_ItemInvoked;
            this.navView.PaneOpening += NavView_PaneOpening;

            if (StoreServicesFeedbackLauncher.IsSupported()) {
                this.navItemFeedback.Visibility = Visibility.Visible;
            }

            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150IconWithBadge);

            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private void NavView_PaneOpening(NavigationView sender, object args) {

            foreach (NavigationViewItemBase navItem in this.navView.MenuItems) {

                if (navItem is NavigationViewItem
                    && navItem.Content is TextBlock) {

                    var txtBlock = navItem.Content as TextBlock;
                    if (txtBlock.Tag != null) {

                        switch (txtBlock.Tag.ToString()) {
                            case "TAG_FULLSCREEN": {
                                    var resLoader = ResourceLoader.GetForCurrentView();

                                    var view = ApplicationView.GetForCurrentView();
                                    txtBlock.Text = view.IsFullScreenMode ? resLoader.GetString("NavItemFullscreenExit") : resLoader.GetString("NavItemFullscreen");
                                    navItemFullscreen.Icon = view.IsFullScreenMode ? new SymbolIcon(Symbol.BackToWindow) : new SymbolIcon(Symbol.FullScreen);
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {

            if (args.IsSettingsInvoked) {

                rootFrame.Navigate(typeof(SettingsPage),
                    null,
                    new SlideNavigationTransitionInfo() {
                        Effect = SlideNavigationTransitionEffect.FromRight
                    });
            }
            else {

                if (!(args.InvokedItem is TextBlock))
                    return;

                var txtBlock = args.InvokedItem as TextBlock;
                if (txtBlock.Tag != null) {

                    switch (txtBlock.Tag.ToString()) {
                        case "TAG_HOME": {
                                rootFrame.Navigate(typeof(HomePage),
                                    null,
                                    new SlideNavigationTransitionInfo() {
                                        Effect = SlideNavigationTransitionEffect.FromRight
                                    });
                            }
                            break;
                        case "TAG_FULLSCREEN": {
                                var view = ApplicationView.GetForCurrentView();
                                if (view.IsFullScreenMode) {
                                    view.ExitFullScreenMode();
                                }
                                else {
                                    if (view.TryEnterFullScreenMode()) {
                                    }
                                    else {
                                        //rootPage.NotifyUser("Failed to enter full screen mode", NotifyType.ErrorMessage);
                                    }
                                }
                            }
                            break;
                        case "TAG_FEEDBACK": {

                                var launcher = StoreServicesFeedbackLauncher.GetDefault();
                                launcher.LaunchAsync();
                            }
                            break;
                    }
                }
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e) {

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            var currentTheme = App.RootTheme.ToString();
            bool darkTheme = false;

            switch (currentTheme) {
                case "Dark":
                    darkTheme = true;
                    break;
                case "Default":
                    if (Application.Current.RequestedTheme == ApplicationTheme.Dark) {
                        darkTheme = true;
                    }
                    break;
            }
            if (darkTheme) {
                titleBar.ButtonForegroundColor = Colors.White;
            }
            else {
                titleBar.ButtonForegroundColor = Colors.Black;
            }

            foreach (NavigationViewItemBase navItem in this.navView.MenuItems) {

                if (navItem is NavigationViewItem
                    && navItem.Content is TextBlock) {

                    var txtBlock = navItem.Content as TextBlock;
                    if (txtBlock.Tag != null) {

                        switch (txtBlock.Tag.ToString()) {
                            case "TAG_HOME": {

                                    rootFrame.Navigate(typeof(HomePage),
                                        null,
                                        new SlideNavigationTransitionInfo() {
                                            Effect = SlideNavigationTransitionEffect.FromRight
                                        });

                                    this.navView.SelectedItem = navItem;
                                    this.navView.IsPaneOpen = false;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
