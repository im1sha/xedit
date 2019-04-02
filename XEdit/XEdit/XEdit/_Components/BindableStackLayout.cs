//using System;
//using Xamarin.Forms;
//using System.Collections.Specialized;
//using System.Collections.ObjectModel;

//namespace XEdit.Components
//{
//    class BindableStackLayout : StackLayout
//    {
//        public static readonly BindableProperty ItemsProperty
//            = BindableProperty.Create(
//                nameof(Items), 
//                typeof(ObservableCollection<View>), 
//                typeof(BindableStackLayout),
//                null,
//                propertyChanged: (bindable, oldValue, newValue)=>
//                {
//                    (newValue as ObservableCollection<View>).CollectionChanged += (coll, arg) =>
//                    {
//                        switch (arg.Action)
//                        {
//                            case NotifyCollectionChangedAction.Add:

//                                foreach (var v in arg.NewItems)
//                                    (bindable as BindableStackLayout).Children.Add((View)v);
//                                break;

//                            case NotifyCollectionChangedAction.Remove:

//                                foreach (var v in arg.NewItems)
//                                    (bindable as BindableStackLayout).Children.Remove((View)v);
//                                break;

//                            case NotifyCollectionChangedAction.Move:

//                                throw new System.NotImplementedException();

//                            case NotifyCollectionChangedAction.Replace:

//                                throw new System.NotImplementedException();

//                        }
//                    };
//                });


//        public ObservableCollection<View> Items
//        {
//            get
//            {
//                return (ObservableCollection<View>)GetValue(ItemsProperty);
//            }
//            set
//            {
//                SetValue(ItemsProperty, value);
//            }
//        }
//    }
//}
