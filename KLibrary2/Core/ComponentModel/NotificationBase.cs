using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Keiho.ComponentModel
{
    public abstract class NotificationBase : INotifyPropertyChanged
    {
        readonly Dictionary<string, object> propertyValues = new Dictionary<string, object>();
        readonly Dictionary<string, string[]> propertiesToNotifyMap;

        public event PropertyChangedEventHandler PropertyChanged;

        protected NotificationBase()
        {
            propertiesToNotifyMap = GetType()
                .GetProperties()
                .SelectMany(p => (DependentOnAttribute[])p.GetCustomAttributes(typeof(DependentOnAttribute), true), (p, d) => new { Target = p.Name, Source = d.PropertyName })
                .GroupBy(d => d.Source, d => d.Target)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        protected T GetValue<T>(string propertyName)
        {
            return propertyValues.ContainsKey(propertyName) ? (T)propertyValues[propertyName] : GetDefaultValue<T>(propertyName);
        }

        protected void SetValue<T>(string propertyName, T value)
        {
            var currentValue = GetValue<T>(propertyName);
            if (object.Equals(value, currentValue)) return;

            propertyValues[propertyName] = value;
            RaisePropertyChanged(propertyName);
        }

        T GetDefaultValue<T>(string propertyName)
        {
            var a = (DefaultValueAttribute)GetType()
                .GetProperty(propertyName)
                .GetCustomAttributes(typeof(DefaultValueAttribute), true)
                .SingleOrDefault();
            return a != null ? (T)a.Value : default(T);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
            if (propertiesToNotifyMap.ContainsKey(propertyName))
            {
                foreach (var target in propertiesToNotifyMap[propertyName])
                {
                    OnPropertyChanged(target);
                }
            }
        }

        public void AddPropertyChangedHandler(string propertyName, Action action)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            if (action == null) throw new ArgumentNullException("action");

            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    action();
                }
            };
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class DependentOnAttribute : Attribute
    {
        public string PropertyName { get; private set; }

        public DependentOnAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
