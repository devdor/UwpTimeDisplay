using System;
using Windows.Storage;
using Windows.UI.Xaml;

namespace TimeDisplayApp {
    public class AppSettings {
        #region Fields and Properties
        public static DateTime? LastUpdated {
            get {
                var result = ReadByKey<string>(AppSettingsType.SettingsLastUpdated);
                if (!String.IsNullOrEmpty(result)) {

                    if (DateTime.TryParse(result, out DateTime dateTime)) {
                        return dateTime;
                    }
                }

                return null;
            }
        }

        public static string FontName {
            get {
                var result = ReadByKey<string>(AppSettingsType.FontName);
                if (!String.IsNullOrEmpty(result)) {
                    return result;
                }

                return AppConstValues.DEFAULT_FONT_NAME;
            }
        }

        public static double FontSize {
            get {
                var result = ReadByKey<double>(AppSettingsType.FontSize);
                if (result > 0) {
                    return result;
                }

                return AppConstValues.DEFAULT_FONT_SIZE;
            }
        }

        public static string FontColorOn {
            get {
                var result = ReadByKey<string>(AppSettingsType.FontColorOn);
                if (!String.IsNullOrEmpty(result)) {
                    return result;
                }

                return AppConstValues.DEFAULT_FONT_COLOR_ON;
            }
        }

        public static string FontColorOff {
            get {
                var result = ReadByKey<string>(AppSettingsType.FontColorOff);
                if (!String.IsNullOrEmpty(result)) {
                    return result;
                }

                return AppConstValues.DEFAULT_FONT_COLOR_OFF;
            }
        }

        public static string ItemBackgroundColor {
            get {
                var result = ReadByKey<string>(AppSettingsType.ItemBackgroundColor);
                if (!String.IsNullOrEmpty(result)) {
                    return result;
                }

                return AppConstValues.DEFAULT_ITEM_BACKGROUND_COLOR;
            }
        }

        public static bool IsItemBorderVisible {
            get {
                var result = ReadByKey<string>(AppSettingsType.IsItemBorderVisible);
                if (!String.IsNullOrEmpty(result)) {

                    if (Boolean.TryParse(result, out bool bResult)) {

                        return bResult;
                    }
                }

                return AppConstValues.DEFAULT_IS_ITEM_BORDER_VISIBLE;
            }
        }

        public static bool IsRenderEffektActive {
            get {
                var result = ReadByKey<string>(AppSettingsType.IsRenderEffectActive);
                if (!String.IsNullOrEmpty(result)) {

                    if (Boolean.TryParse(result, out bool bResult)) {

                        return bResult;
                    }
                }

                return AppConstValues.DEFAULT_IS_RENDER_EFFECT_ACTIVE;
            }
        }

        public static ElementTheme Theme {
            get {
                var result = ReadByKey<string>(AppSettingsType.AppTheme);
                if (!String.IsNullOrEmpty(result)) {
                    return Util.GetEnum<ElementTheme>(result);
                }

                return ElementTheme.Default;
            }
        }
        #endregion

        static T ReadByKey<T>(AppSettingsType appSettingsType) {

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values != null
                && localSettings.Values.ContainsKey(GetKeyName(appSettingsType))) {

                var result = localSettings.Values[GetKeyName(appSettingsType)];
                if (result != null
                    && result is T) {

                    return (T)result;
                }
            }

            return default;
        }

        static string GetKeyName(AppSettingsType appSettingsType) {
            return appSettingsType.ToString().ToUpper();
        }

        public static void SaveValue(AppSettingsType appSettingsType, object value) {

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[GetKeyName(appSettingsType)] = value;
        }

        public static void SetDefault(ElementTheme themeType) {

            SaveValue(AppSettingsType.FontName, AppConstValues.DEFAULT_FONT_NAME);
            SaveValue(AppSettingsType.FontSize, AppConstValues.DEFAULT_FONT_SIZE);
            SaveValue(AppSettingsType.ItemBackgroundColor, AppConstValues.DEFAULT_ITEM_BACKGROUND_COLOR);
            SaveValue(AppSettingsType.IsItemBorderVisible, AppConstValues.DEFAULT_IS_ITEM_BORDER_VISIBLE);
            SaveValue(AppSettingsType.IsRenderEffectActive, AppConstValues.DEFAULT_IS_RENDER_EFFECT_ACTIVE);
            SaveValue(AppSettingsType.AppTheme, themeType.ToString());

            switch (themeType) {
                case ElementTheme.Dark: {

                        SaveValue(AppSettingsType.FontColorOff, AppConstValues.TEMPLATE_DARK_COL_OFF);
                        SaveValue(AppSettingsType.FontColorOn, AppConstValues.TEMPLATE_DARK_COL_ON);
                    }
                    break;
                case ElementTheme.Light: {

                        SaveValue(AppSettingsType.FontColorOff, AppConstValues.TEMPLATE_LIGHT_COL_OFF);
                        SaveValue(AppSettingsType.FontColorOn, AppConstValues.TEMPLATE_LIGHT_COL_ON);
                    }
                    break;
                default: {

                        SaveValue(AppSettingsType.FontColorOff, AppConstValues.DEFAULT_FONT_COLOR_OFF);
                        SaveValue(AppSettingsType.FontColorOn, AppConstValues.DEFAULT_FONT_COLOR_ON);
                    }
                    break;

            }
        }
    }
}
