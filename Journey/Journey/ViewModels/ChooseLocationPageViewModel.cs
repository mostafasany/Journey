using System;
using System.Collections.Generic;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ChooseLocationPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IFacebookService _facebookService;
        private readonly ILocationService _locationService;

        public ChooseLocationPageViewModel(IUnityContainer container, ILocationService locationService,
            IFacebookService facebookService) :
            base(container)
        {
            _locationService = locationService;
            _facebookService = facebookService;
        }

        #region Methods

        public override async void Intialize()
        {
            try
            {
                ShowProgress();

                var position = await _locationService.ObtainMyLocationAsync();
                if (position != null)
                    Locations = await _facebookService.GetLocationsAsync(Name, position.Lat, position.Lng);

                SelectedLocation = null;
                base.Intialize();
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
            finally
            {
                HideProgress();
            }
        }

        #endregion

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Intialize();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private List<Location> originalLocations;

        private List<Location> locations;

        public List<Location> Locations
        {
            get => locations;
            set
            {
                locations = value;
                RaisePropertyChanged();
            }
        }

        private Location selectedLocation;

        public Location SelectedLocation
        {
            get => selectedLocation;
            set
            {
                if (selectedLocation == value)
                    return;
                selectedLocation = value;
                RaisePropertyChanged();
                if (value != null)
                    OnSelectedLocationCommand.Execute(value);
            }
        }

        #endregion

        #region Commands

        #region OnSelectedLocationCommand

        public DelegateCommand<Location> OnSelectedLocationCommand => new DelegateCommand<Location>(OnSelectedLocation);

        private async void OnSelectedLocation(Location selectedLocation)
        {
            NavigationService.GoBack(selectedLocation, "Location");
        }

        #endregion

        #region OnSearchCommand

        //public DelegateCommand OnSearchCommand => new DelegateCommand(OnSearch);

        //private async void OnSearch()
        //{
        //    try
        //    {
        //        IsBusy = true;
        //        // var position = await locationService.ObtainMyLocationAsync();
        //        if (string.IsNullOrEmpty(Name))
        //            Locations = originalLocations;
        //        else
        //            Locations = originalLocations.Where(a => a.Name.IndexOf(Name,0,System.StringComparison.OrdinalIgnoreCase)>0).ToList();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        #endregion

        #region OnCloseCommand

        public DelegateCommand OnCloseCommand => new DelegateCommand(OnClose);

        private async void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #endregion
    }
}