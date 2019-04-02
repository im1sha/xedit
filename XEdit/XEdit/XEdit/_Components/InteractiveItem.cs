//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin.Forms;

//namespace XEdit.Components
//{

//    [ContentProperty("ItemContent")]
//    public class InteractiveItem : ContentView
//    {
//        private ScrollView _scrollview;
//        private StackLayout _stacklayout { get; set; }
//        public InteractiveItem()
//        {
//            _stacklayout = new StackLayout();
//            _scrollview = new ScrollView()
//            {
//                Content = _stacklayout
//            };
//            Content = _scrollview;
//        }
//        public static readonly BindableProperty ItemContentProperty 
//            = BindableProperty.Create(
//                "ItemContent",
//                typeof(DataTemplate), 
//                typeof(InteractiveItem), 
//                default(ElementTemplate));

//        public DataTemplate ItemContent
//        {
//            get { return (DataTemplate)GetValue(ItemContentProperty); }
//            set { SetValue(ItemContentProperty, value); }
//        }


//        private ScrollOrientation _scrollOrientation;
//        public ScrollOrientation Orientation
//        {
//            get
//            {
//                return _scrollOrientation;
//            }
//            set
//            {
//                _scrollOrientation = value;
//                _stacklayout.Orientation
//                    = value == ScrollOrientation.Horizontal 
//                    ? StackOrientation.Horizontal
//                    : StackOrientation.Vertical;
//                _scrollview.Orientation = value;
//            }
//        }

//        public static readonly BindableProperty ItemsSourceProperty 
//            = BindableProperty.Create(
//                "ItemsSource",
//                typeof(IEnumerable), 
//                typeof(InteractiveItem),
//                default(IEnumerable), 
//                propertyChanged: GetEnumerator);

//        public IEnumerable ItemsSource
//        {
//            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
//            set { SetValue(ItemsSourceProperty, value); }
//        }

//        private static void GetEnumerator(BindableObject bindable, object oldValue, object newValue)
//        {
//            foreach (object child in (newValue as IEnumerable))
//            {
//                View view = (View)(bindable as InteractiveItem).ItemContent.CreateContent();
//                view.BindingContext = child;
//                (bindable as InteractiveItem)._stacklayout.Children.Add(view);
//            }
//        }
//    }

//}