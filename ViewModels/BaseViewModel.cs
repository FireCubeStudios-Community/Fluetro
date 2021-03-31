using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Fluetro.ViewModels
{
	/// <summary>
	/// A base View Model that fires <see cref="PropertyChanged"/> events as needed
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged, IDisposable
	{
		#region Private Members

		/// <summary>
		/// A List that contains name of properties that have been already set
		/// </summary>
		private IList<string> _setOnceProperties = new List<string>();

		#endregion

		#region Protected Members

		/// <summary>
		/// A global lock for property checks so prevent locking on different instances of expressions.
		/// Considering how fast this check will always be it isn't an issue to globally lock all callers.
		/// </summary>
		protected object PropertyValueCheckLock = new object();

		#endregion

		/// <summary>
		/// The event that is fired when any child property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

		/// <summary>
		/// Call this to fire a <see cref="PropertyChanged"/> event
		/// </summary>
		/// <param name="propertyName"></param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "") => this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		#region Command Helpers

		/// <summary>
		/// Runs a command if the updating flag is not set.
		/// If the flag is true (indicating the function is already running) then the action is not run.
		/// If the flag is false (indicating no running function) then the action is run.
		/// Once the action is finished if it was run, then the flag is reset to false
		/// </summary>
		/// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
		/// <param name="action">The action to run if the command is not already running</param>
		/// <returns></returns>
		protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
		{
			// Lock to ensure single access to check
			lock (PropertyValueCheckLock)
			{
				// Check if the flag property is true (meaning the function is already running)
				if (updatingFlag.GetPropertyValue())
					return;

				// Set the property flag to true to indicate we are running
				updatingFlag.SetPropertyValue(true);
			}

			try
			{
				// Run the passed in action
				await action();
			}
			finally
			{
				// Set the property flag back to false now it's finished
				updatingFlag.SetPropertyValue(false);
			}
		}

		/// <summary>
		/// Runs a command if the updating flag is not set.
		/// If the flag is true (indicating the function is already running) then the action is not run.
		/// If the flag is false (indicating no running function) then the action is run.
		/// Once the action is finished if it was run, then the flag is reset to false
		/// </summary>
		/// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
		/// <param name="action">The action to run if the command is not already running</param>
		/// <typeparam name="T">The type the action returns</typeparam>
		/// <returns></returns>
		protected async Task<T> RunCommandAsync<T>(Expression<Func<bool>> updatingFlag, Func<Task<T>> action, T defaultValue = default)
		{
			// Lock to ensure single access to check
			lock (this.PropertyValueCheckLock)
			{
				// Check if the flag property is true (meaning the function is already running)
				if (updatingFlag.GetPropertyValue())
					return defaultValue;

				// Set the property flag to true to indicate we are running
				updatingFlag.SetPropertyValue(true);
			}

			try
			{
				// Run the passed in action
				return await action();
			}
			finally
			{
				// Set the property flag back to false now it's finished
				updatingFlag.SetPropertyValue(false);
			}
		}

		#endregion

		#region Protected Helpers

		/// <summary>
		/// Sets the <paramref name="field"/> with provided <paramref name="value"/> and fires <see cref="OnPropertyChanged(string)"/>
		/// <br/>
		/// and calls <see cref="OnPropertyChanged(string)"/>
		/// </summary>
		/// <typeparam name="T">The property type</typeparam>
		/// <param name="field">The field to update</param>
		/// <param name="value">The value to update the property with</param>
		/// <param name="propertyName"></param>
		/// <returns>true if property has been updated; otherwise false</returns>
		protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			this.OnPropertyChanged(propertyName);

			return true;
		}

		/// <summary>
		/// Sets the <paramref name="property"/> with provided <paramref name="value"/> once, and prevents from being set another time
		/// <br/>
		/// and calls <see cref="OnPropertyChanged(string)"/>
		/// </summary>
		/// <typeparam name="T">The property type</typeparam>
		/// <param name="field">The field to update</param>
		/// <param name="value">The value to update the property with</param>
		/// <param name="propertyName"></param>
		/// <returns>true if property has been updated; otherwise false</returns>
		protected virtual bool SetOnce<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			if (this._setOnceProperties.Contains(propertyName))
			{
				return false;
			}

			this._setOnceProperties.Add(propertyName);

			return this.SetProperty(ref field, value, propertyName);
		}

		#endregion

		#region IDisposable

		public virtual void Dispose()
		{
			_setOnceProperties?.Clear();
			_setOnceProperties = null;
		}

		#endregion
	}

	/// <summary>
	/// A helper for expressions
	/// </summary>
	public static class ExpressionHelpers
	{
		/// <summary>
		/// Compiles an expression and gets the functions return value
		/// </summary>
		/// <typeparam name="T">The type of return value</typeparam>
		/// <param name="lambda">The expression to compile</param>
		/// <returns></returns>
		public static T GetPropertyValue<T>(this Expression<Func<T>> lambda) => lambda.Compile().Invoke();

		/// <summary>
		/// Sets the underlying properties value to the given value
		/// from an expression that contains the property
		/// </summary>
		/// <typeparam name="T">The type of value to set</typeparam>
		/// <param name="lambda">The expression</param>
		/// <param name="value">The value to set the property to</param>
		public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
		{
			// Convert a lambda () =>  some.Property, to some.Property
			MemberExpression expression = (lambda as LambdaExpression).Body as MemberExpression;

			// Get the property information so we can set is
			PropertyInfo propertyInfo = (PropertyInfo)expression.Member;
			object target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

			propertyInfo.SetValue(target, value);
		}
	}
}