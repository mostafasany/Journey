﻿using System;
using System.Collections.Generic;
using Journey.Models.Post;
using Journey.Services.Buisness.Measurment;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class UpdateMeasurmentPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountMeasurmentService _accountMeasurmentService;

        public UpdateMeasurmentPageViewModel(IUnityContainer container, IAccountMeasurmentService accountMeasurmentService) :
            base(container)
        {
            _accountMeasurmentService = accountMeasurmentService;
        }

        private List<ScaleMeasurment> _measuremnts;
        public List<ScaleMeasurment> Measuremnts
        {
            get => _measuremnts;
            set => SetProperty(ref _measuremnts, value);
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ShowProgress();
                Measuremnts = await _accountMeasurmentService.GetMeasurmentsAsync();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                HideProgress();
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion

        #region Commands

        #region OnContinueCommand


        public DelegateCommand OnContinueCommand => new DelegateCommand(OnContinue);


        private async void OnContinue()
        {
            try
            {
                if (IsProgress())
                    return;
                ShowProgress();
                if (Measuremnts == null)
                    return;

                var measurments = await _accountMeasurmentService.UpdateScaleMeasurments(Measuremnts);

                NavigationService.GoBack(measurments, "Measurments");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                HideProgress();
            }

        }


        #endregion

        #region OnBackCommand


        public DelegateCommand OnBackCommand => new DelegateCommand(OnBack);


        private async void OnBack()
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                HideProgress();
            }

        }


        #endregion

        #endregion
    }
}
