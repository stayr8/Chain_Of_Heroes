using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Binding
{
    protected ViewModel TargetModelObject;
    protected PropertyInfo Property;
    protected UnityAction<object> UpdateCallback;

    protected bool Initialized { get; set; }

    public ViewModel ViewModel
    {
        get
        {
            return TargetModelObject;
        }
    }
    public string PropertyName
    {
        get
        {
            return Property.Name;
        }
    }

    public static Binding Create(ViewModel targetModelObject, PropertyInfo property, UnityAction<object> callback)
    {
        Binding newBinding = new Binding();

        newBinding.TargetModelObject = targetModelObject;
        newBinding.Property = property;
        newBinding.UpdateCallback = callback;

        targetModelObject.RegisterBinding(newBinding);

        return newBinding;
    }


    public void Update()
    {
        UpdateCallback?.Invoke(Initialized);

        Initialized = true;
    }


}

public class BindingManager
{
    public static Binding Bind(ViewModel targetModelObject, string propertyName, UnityAction<object> callback, bool updateAfterBind = true)
    {
        PropertyInfo property = targetModelObject.GetType().GetProperty(propertyName);
        if (property == null)
        {
            Debug.LogError($"Property를 찾을 수 없음 : {targetModelObject} {propertyName}");
        }

        var binding = Binding.Create(targetModelObject, property, callback);
        if (updateAfterBind)
        {
            binding?.Update();
        }

        return binding;
    }

    public static void Unbind(ViewModel targetModelObject, Binding binding)
    {
        targetModelObject.UnregisterBinding(binding);
    }
}


public class ViewModel
{
    protected Dictionary<string, List<Binding>> Bindings = new Dictionary<string, List<Binding>>();

    public void RegisterBinding(Binding binding)
    {
        string name = binding.PropertyName;

        List<Binding> bindings;
        if (Bindings.TryGetValue(name, out bindings) == false)
        {
            bindings = new List<Binding>();
            Bindings.Add(name, bindings);
        }

        bindings.Add(binding);
    }

    public void UnregisterBinding(Binding binding)
    {
        List<Binding> bindings;
        if (Bindings.TryGetValue(binding.PropertyName, out bindings))
        {
            bindings.Remove(binding);
        }
    }

    protected bool Set<T>(ref T propertyObject, T value, bool forceUpdate = false,
        [CallerMemberName] string propertyName = null)
    {
        if (forceUpdate == false && Equals(propertyObject, value))
        {
            return false;
        }

        propertyObject = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        List<Binding> bindings;

        if (Bindings.TryGetValue(propertyName, out bindings))
        {
            foreach (Binding binding in bindings)
            {
                binding.Update();
            }
        }
    }
}
