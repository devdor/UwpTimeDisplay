using System;
using System.Reflection;
using TimeDisplayApp.Clock;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimeDisplayApp {
    public class Util {
        public static SolidColorBrush GetSolidColorBrush(string hex) {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }

        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }

        public static double GetMaxWidthAndHeight(double fontSize) {

            double maxWidth = 0;
            double maxHeight = 0;

            var tmpList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach (char tmpChar in tmpList) {

                var tb = new TextBlock {
                    Text = tmpChar.ToString(),
                    FontSize = fontSize
                };
                tb.Measure(
                    new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                if (tb.ActualHeight > maxHeight) {
                    maxHeight = tb.ActualHeight;
                };

                if (tb.ActualWidth > maxWidth) {
                    maxWidth = tb.ActualWidth;
                };
            }

            return Math.Ceiling(
                Math.Max(maxWidth, maxHeight));
        }

        public static string GetReadableString(WordType word) {
            switch (word) {
                case WordType.VOR:
                    return "vor";
                case WordType.NACH:
                    return "nach";
                case WordType.ESIST:
                    return null;
                case WordType.UHR:
                    return "Uhr";
                case WordType.FUENF:
                    return "5";
                case WordType.ZEHN:
                    return "10";
                case WordType.VIERTEL:
                    return "viertel";
                case WordType.ZWANZIG:
                    return "20";
                case WordType.HALB:
                    return "halb";
                case WordType.DREIVIERTEL:
                    return "3/4";
                case WordType.H_EIN:
                    return "ein";
                case WordType.H_EINS:
                    return "1";
                case WordType.H_ZWEI:
                    return "2";
                case WordType.H_DREI:
                    return "3";
                case WordType.H_VIER:
                    return "4";
                case WordType.H_FUENF:
                    return "5";
                case WordType.H_SECHS:
                    return "6";
                case WordType.H_SIEBEN:
                    return "7";
                case WordType.H_ACHT:
                    return "8";
                case WordType.H_NEUN:
                    return "9";
                case WordType.H_ZEHN:
                    return "10";
                case WordType.H_ELF:
                    return "11";
                case WordType.H_ZWOELF:
                    return "12";
            }

            return null;
        }

        public static void SetTheme(ElementTheme themeType) {

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            App.RootTheme = themeType;

            if (themeType == ElementTheme.Dark) {
                titleBar.ButtonForegroundColor = Colors.Gray;
            }
            else if (themeType == ElementTheme.Light) {
                titleBar.ButtonForegroundColor = Colors.Black;
            }
            else {
                if (Application.Current.RequestedTheme == ApplicationTheme.Dark) {
                    titleBar.ButtonForegroundColor = Colors.Gray;
                }
                else {
                    titleBar.ButtonForegroundColor = Colors.Black;
                }
            }
        }
    }
}
