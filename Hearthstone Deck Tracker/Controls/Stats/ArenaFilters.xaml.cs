﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Enums;

#endregion

namespace Hearthstone_Deck_Tracker.Controls.Stats
{
	/// <summary>
	/// Interaction logic for ArenaFilters.xaml
	/// </summary>
	public partial class ArenaFilters : UserControl
	{
		private readonly bool _initialized;
		private Action _updateCallback;

		public ArenaFilters(Action updateCallback = null)
		{
			InitializeComponent();
			ComboBoxTimeframe.ItemsSource = Enum.GetValues(typeof(DisplayedTimeFrame));
			ComboBoxTimeframe.SelectedItem = Config.Instance.ArenaStatsTimeFrameFilter;
			ComboBoxClass.ItemsSource =
				Enum.GetValues(typeof(HeroClassStatsFilter)).Cast<HeroClassStatsFilter>().Select(x => new HeroClassStatsFilterWrapper(x));
			ComboBoxClass.SelectedItem = new HeroClassStatsFilterWrapper(Config.Instance.ArenaStatsClassFilter);
			ComboBoxRegion.ItemsSource = Enum.GetValues(typeof(RegionAll));
			ComboBoxRegion.SelectedItem = Config.Instance.ArenaStatsRegionFilter;
			if(updateCallback != null)
				SetUpdateCallback(updateCallback);
			_initialized = true;
		}

		internal void SetUpdateCallback(Action callback)
		{
			if(_updateCallback == null)
				_updateCallback = callback;
		}

		private void ComboBoxTimeframe_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(!_initialized)
				return;
			Config.Instance.ArenaStatsTimeFrameFilter = (DisplayedTimeFrame)ComboBoxTimeframe.SelectedItem;
			Config.Save();
			_updateCallback?.Invoke();
		}

		private void ComboBoxClass_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(!_initialized)
				return;
			Config.Instance.ArenaStatsClassFilter = ((HeroClassStatsFilterWrapper)ComboBoxClass.SelectedItem).HeroClass;
			Config.Save();
			_updateCallback?.Invoke();
		}

		private void DatePickerCustomTimeFrame_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if(!_initialized)
				return;
			_updateCallback?.Invoke();
		}

		private void ComboBoxRegion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(!_initialized)
				return;
			Config.Instance.ArenaStatsRegionFilter = (RegionAll)ComboBoxRegion.SelectedItem;
			Config.Save();
			_updateCallback?.Invoke();
		}

		private void CheckBoxArchived_OnChecked(object sender, RoutedEventArgs e)
		{
			if(!_initialized)
				return;
			_updateCallback?.Invoke();
		}

		private void CheckBoxArchived_OnUnchecked(object sender, RoutedEventArgs e)
		{
			if(!_initialized)
				return;
			_updateCallback?.Invoke();
		}

		public void Reset()
		{
			new List<string>
			{
				nameof(Config.Instance.ArenaStatsTimeFrameFilter),
				nameof(Config.Instance.ArenaStatsTimeFrameCustomStart),
				nameof(Config.Instance.ArenaStatsTimeFrameCustomEnd),
				nameof(Config.Instance.ArenaStatsClassFilter),
				nameof(Config.Instance.ArenaStatsRegionFilter),
				nameof(Config.Instance.ArenaStatsIncludeArchived)
			}.ForEach(Config.Instance.Reset);
			Config.Save();
		}
	}
}