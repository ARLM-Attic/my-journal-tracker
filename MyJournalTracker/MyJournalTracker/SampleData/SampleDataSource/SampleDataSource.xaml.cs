﻿//      *********    DIESE DATEI DARF NICHT GEÄNDERT WERDEN     *********
//      Diese Datei wurde von einem Designwerkzeug erstellt. Änderungen
//      dieser Datei können Fehler verursachen.
namespace Expression.Blend.SampleData.SampleDataSource
{
	using System; 

// Um den Speicherbedarf der Beispieldaten in der Produktionsanwendung deutlich zu reduzieren, legen Sie
// die Konstante für bedingte Kompilierung DISABLE_SAMPLE_DATA fest, und deaktivieren Sie die Beispieldaten zur Laufzeit.
#if DISABLE_SAMPLE_DATA
	internal class SampleDataSource { }
#else

	public class SampleDataSource : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleDataSource()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/MyJournalTracker;component/SampleData/SampleDataSource/SampleDataSource.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private ItemCollection _Collection = new ItemCollection();

		public ItemCollection Collection
		{
			get
			{
				return this._Collection;
			}
		}
	}

	public class Item : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _Property1 = string.Empty;

		public string Property1
		{
			get
			{
				return this._Property1;
			}

			set
			{
				if (this._Property1 != value)
				{
					this._Property1 = value;
					this.OnPropertyChanged("Property1");
				}
			}
		}

		private bool _Property2 = false;

		public bool Property2
		{
			get
			{
				return this._Property2;
			}

			set
			{
				if (this._Property2 != value)
				{
					this._Property2 = value;
					this.OnPropertyChanged("Property2");
				}
			}
		}
	}

	public class ItemCollection : System.Collections.ObjectModel.ObservableCollection<Item>
	{ 
	}
#endif
}