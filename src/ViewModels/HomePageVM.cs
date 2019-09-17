using Prism.Mvvm;
using System.Collections.Generic;
using TimeDisplayApp.Clock;
using Windows.UI.Xaml.Media;

namespace TimeDisplayApp.ViewModels {
    public class HomePageVM : BindableBase {
        #region Fields and Properties
        int _numRows;
        public int NumRows {
            get => _numRows;
            set => SetProperty(ref _numRows, value);
        }

        int _numColumns;
        public int NumColumns {
            get => _numColumns;
            set => SetProperty(ref _numColumns, value);
        }

        Dictionary<string, CharItem> _letters;
        public Dictionary<string, CharItem> Letters {
            get => _letters;
            set => SetProperty(ref _letters, value);
        }

        Dictionary<int, MinuteItem> _minutes;
        public Dictionary<int, MinuteItem> Minutes {
            get => _minutes;
            set => SetProperty(ref _minutes, value);
        }

        Brush _foregroundBrushOn;
        public Brush ForegroundBrushOn {
            get => _foregroundBrushOn;
            set => SetProperty(ref _foregroundBrushOn, value);
        }

        Brush _foregroundBrushOff;
        public Brush ForegroundBrushOff {
            get => _foregroundBrushOff;
            set => SetProperty(ref _foregroundBrushOff, value);
        }

        Brush _itemBackgroundBrush;
        public Brush ItemBackgroundBrush {
            get => _itemBackgroundBrush;
            set => SetProperty(ref _itemBackgroundBrush, value);
        }


        List<WordType> _wordList;
        public List<WordType> WordList {
            get => _wordList;
            set => _wordList = value;
        }
        #endregion

        public HomePageVM() {
        }

        public void Init(List<string> template) {

            this.NumRows = template.Count;
            this.NumColumns = template[0].Length;

            this.Letters = new Dictionary<string, CharItem>();
            for (int i = 0; i < NumRows; i++) {
                for (int j = 0; j < NumColumns; j++) {

                    this.Letters.Add(
                        $"{i}_{j}",
                        new CharItem(
                            this.ForegroundBrushOn,
                            this.ForegroundBrushOff,
                            template[i][j].ToString()) {
                            BackgroundBrush = ItemBackgroundBrush
                        });
                }
            }

            this.Minutes = new Dictionary<int, MinuteItem>();
            this.Minutes.Add(0, new MinuteItem(this.ForegroundBrushOn, this.ForegroundBrushOff));
            this.Minutes.Add(1, new MinuteItem(this.ForegroundBrushOn, this.ForegroundBrushOff));
            this.Minutes.Add(2, new MinuteItem(this.ForegroundBrushOn, this.ForegroundBrushOff));
            this.Minutes.Add(3, new MinuteItem(this.ForegroundBrushOn, this.ForegroundBrushOff));
        }

        public void SetStateAll(bool isActive) {

            foreach (var letter in this.Letters) {
                letter.Value.SetState(isActive);
            }

            foreach (var digit in this.Minutes) {
                digit.Value.SetState(isActive);
            }
        }

        public void ShowCurrentTime(int hours, int minutes, LanguageType language) {

            this.SetStateAll(false);
            this.SetMinutes(hours, minutes, language);
        }

        void SetMinutes(int hours, int minutes, LanguageType language) {
            this.SetWord(WordType.ESIST);

            if (minutes.ToString().EndsWith("1")
                || minutes.ToString().EndsWith("6")) {
                this.Minutes[0].SetState(true);
            }
            else if (minutes.ToString().EndsWith("2")
                || minutes.ToString().EndsWith("7")) {
                this.Minutes[0].SetState(true);
                this.Minutes[1].SetState(true);
            }
            else if (minutes.ToString().EndsWith("3")
                || minutes.ToString().EndsWith("8")) {
                this.Minutes[0].SetState(true);
                this.Minutes[1].SetState(true);
                this.Minutes[2].SetState(true);
            }
            else if (minutes.ToString().EndsWith("4")
                || minutes.ToString().EndsWith("9")) {
                this.Minutes[0].SetState(true);
                this.Minutes[1].SetState(true);
                this.Minutes[2].SetState(true);
                this.Minutes[3].SetState(true);
            }

            while (hours < 0) {
                hours += 24;
            }
            while (hours > 23) {
                hours -= 24;
            }

            //this.SetHours(hours, minutes == 0);

            switch (minutes / 5) {
                case 0:
                    // glatte Stunde
                    this.SetHours(hours, true, language);
                    break;
                case 1:
                    // 5 nach
                    this.SetWord(WordType.FUENF);
                    this.SetWord(WordType.NACH);
                    this.SetHours(hours, false, language);
                    break;
                case 2:
                    // 10 nach
                    this.SetWord(WordType.ZEHN);
                    this.SetWord(WordType.NACH);
                    this.SetHours(hours, false, language);
                    break;
                case 3:
                    // viertel nach
                    if ((language == LanguageType.LANGUAGE_DE_SW) || (language == LanguageType.LANGUAGE_DE_SA)) {
                        this.SetWord(WordType.VIERTEL);
                        this.SetHours(hours + 1, false, language);
                    }
                    else {
                        this.SetWord(WordType.VIERTEL);
                        this.SetWord(WordType.NACH);
                        this.SetHours(hours, false, language);
                    }
                    break;
                case 4:
                    // 20 nach
                    if (language == LanguageType.LANGUAGE_DE_SA) {
                        this.SetWord(WordType.ZEHN);
                        this.SetWord(WordType.VOR);
                        this.SetWord(WordType.HALB);
                        this.SetHours(hours + 1, false, language);
                    }
                    else {
                        this.SetWord(WordType.ZWANZIG);
                        this.SetWord(WordType.NACH);
                        this.SetHours(hours, false, language);
                    }
                    break;
                case 5:
                    // 5 vor halb
                    this.SetWord(WordType.FUENF);
                    this.SetWord(WordType.VOR);
                    this.SetWord(WordType.HALB);
                    this.SetHours(hours + 1, false, language);
                    break;
                case 6:
                    // halb
                    this.SetWord(WordType.HALB);
                    this.SetHours(hours + 1, false, language);
                    break;
                case 7:
                    // 5 nach halb
                    this.SetWord(WordType.FUENF);
                    this.SetWord(WordType.NACH);
                    this.SetWord(WordType.HALB);
                    this.SetHours(hours + 1, false, language);
                    break;
                case 8:
                    // 20 vor
                    if (language == LanguageType.LANGUAGE_DE_SA) {
                        this.SetWord(WordType.ZEHN);
                        this.SetWord(WordType.NACH);
                        this.SetWord(WordType.HALB);
                        this.SetHours(hours + 1, false, language);
                    }
                    else {
                        this.SetWord(WordType.ZWANZIG);
                        this.SetWord(WordType.VOR);
                        this.SetHours(hours + 1, false, language);
                    }
                    break;
                case 9:
                    // viertel vor
                    if ((language == LanguageType.LANGUAGE_DE_SW)
                        || (language == LanguageType.LANGUAGE_DE_BA)
                        || (language == LanguageType.LANGUAGE_DE_SA)) {

                        this.SetWord(WordType.DREIVIERTEL);
                        this.SetHours(hours + 1, false, language);
                    }
                    else {
                        this.SetWord(WordType.VIERTEL);
                        this.SetWord(WordType.VOR);
                        this.SetHours(hours + 1, false, language);
                    }
                    break;
                case 10:
                    // 10 vor
                    this.SetWord(WordType.ZEHN);
                    this.SetWord(WordType.VOR);
                    this.SetHours(hours + 1, false, language);
                    break;
                case 11:
                    // 5 vor
                    this.SetWord(WordType.FUENF);
                    this.SetWord(WordType.VOR);
                    this.SetHours(hours + 1, false, language);
                    break;
            }
        }

        void SetHours(int hours, bool isExact, LanguageType language) {

            switch (language) {
                case LanguageType.LANGUAGE_DE_BA:
                case LanguageType.LANGUAGE_DE_DE:
                case LanguageType.LANGUAGE_DE_SA:
                case LanguageType.LANGUAGE_DE_SW: {

                        if (isExact) {
                            SetWord(WordType.UHR);
                        };

                        switch (hours) {
                            case 0:
                            case 12:
                            case 24:
                                SetWord(WordType.H_ZWOELF);
                                break;
                            case 1:
                            case 13:
                                if (isExact) {
                                    SetWord(WordType.H_EIN);
                                }
                                else {
                                    SetWord(WordType.H_EINS);
                                }
                                break;
                            case 2:
                            case 14:
                                SetWord(WordType.H_ZWEI);
                                break;
                            case 3:
                            case 15:
                                SetWord(WordType.H_DREI);
                                break;
                            case 4:
                            case 16:
                                SetWord(WordType.H_VIER);
                                break;
                            case 5:
                            case 17:
                                SetWord(WordType.H_FUENF);
                                break;
                            case 6:
                            case 18:
                                SetWord(WordType.H_SECHS);
                                break;
                            case 7:
                            case 19:
                                SetWord(WordType.H_SIEBEN);
                                break;
                            case 8:
                            case 20:
                                SetWord(WordType.H_ACHT);
                                break;
                            case 9:
                            case 21:
                                SetWord(WordType.H_NEUN);
                                break;
                            case 10:
                            case 22:
                                SetWord(WordType.H_ZEHN);
                                break;
                            case 11:
                            case 23:
                                SetWord(WordType.H_ELF);
                                break;
                        }
                    }
                    break;
            }
        }

        void SetWord(WordType word, bool isActive = true) {

            if (this.WordList == null)
                this.WordList = new List<WordType>();

            this.WordList.Add(word);

            foreach (var letterIndex in ClockData.CharIndexList(word)) {
                this.Letters[letterIndex].SetState(isActive);
            }
        }
    }
}
