using System;
using Windows.UI.Xaml.Media;

namespace TimeDisplayApp.Clock {
    public class CharItem : AbstractBaseItem {
        #region Fields and Properties
        string _content;
        public string Content {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        #endregion
        public CharItem(Brush foregroundBrushOn, Brush foregroundBrushOff, string content)
            : base(foregroundBrushOn, foregroundBrushOff) {

            if (String.IsNullOrEmpty(content))
                throw new ArgumentNullException("Content");

            this.Content = content;
        }
    }
}
