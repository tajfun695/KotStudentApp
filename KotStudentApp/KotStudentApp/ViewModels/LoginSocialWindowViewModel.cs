﻿using KotStudentApp.Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace KotStudentApp
{
    class LoginSocialWindowViewModel : ViewModelBase
    {
        #region Private Variables

        private Window mWindow;

        private WindowResizer wWindowResizer;

        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        private int mOutMargin = 8;

        private string FacebookURI = $"https://www.facebook.com/dialog/oauth?client_id=388432308196913&redirect_uri=https://www.facebook.com/connect/login_success.html&display=popup&scope=public_profile+email+user_education_history&response_type=token";

        #endregion

        #region Public Properties

        /// <summary>
        /// MainWindow title text!
        /// </summary>
        public string Title { get; set; } = "Facebook";

        public Uri NaviUri { get; set; }

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 950;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 700;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder => Borderless ? 0 : 6;

        /// <summary>
        /// Margin Outer of MainWindow
        /// </summary>
        public int OutMarginSize
        {
            get => mWindow.WindowState == WindowState.Maximized ? 0 : mOutMargin;
            set => mOutMargin = value;
        }

        /// <summary>
        /// Setting padding MainWindow
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OutMarginSize);

        /// <summary>
        /// Margin Outer of MainWindow
        /// </summary>
        public Thickness OutMarginSizeThickness { get { return new Thickness(OutMarginSize); } }

        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// TitibleBar height
        /// </summary>
        public int TitleHeight { get; set; } = 36;

        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        #endregion

        #region Commands

        /// <summary>
        /// Minimalization click
        /// </summary>
        public ICommand MinimalizeCommand { get; set; }

        /// <summary>
        /// Maximalization click
        /// </summary>
        public ICommand MaximalizationCommand { get; set; }

        /// <summary>
        /// Close click
        /// </summary>
        public ICommand CloseCommand { get; set; }

        #endregion

        #region CTORS
        public LoginSocialWindowViewModel(Window window, LoginSocialTypes LoginType)
        {
            mWindow = window;

            //Kind of social medial
            switch(LoginType)
            {
                case LoginSocialTypes.Facebook:
                    Title = "Facebook";
                    NaviUri = new Uri(FacebookURI);
                    break;

                case LoginSocialTypes.Google:
                    Title = "Google";
                    break;

                case LoginSocialTypes.Twitter:
                    Title = "Twitter";
                    break;
            }

            #region Commands
            MinimalizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximalizationCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            #endregion

            wWindowResizer = new WindowResizer(mWindow);

            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResizer();
            };

            wWindowResizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResizer();
            };

        }
        #endregion

        #region Helpers

        private void WindowResizer()
        {

            OnPropertyChanged(nameof(ResizeBorder));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OutMarginSize));
            OnPropertyChanged(nameof(OutMarginSizeThickness));
        }

        #endregion

    }
}
