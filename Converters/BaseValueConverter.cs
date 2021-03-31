using System;
using System.Globalization;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Fluetro.Converters
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T">The type of this value converter</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region Private Members

        ///<summary>
        /// A single static instance of this value converter
        ///</summary>
        private static T Converter = null;

        #endregion

        #region Markup Extention Methods

        /// <summary>
        /// Provides a static instance of the value converter 
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns></returns>
       protected override object ProvideValue() => Converter ??= new T();

        #endregion

        #region Value Converter Methods

        /// <summary>
        /// Method to convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, string language);

        /// <summary>
        /// Method to convert a value back to it's source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, string language);

        #endregion
    }
}
