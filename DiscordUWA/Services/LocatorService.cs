﻿using Discord.WebSocket;
using DiscordUWA.Interfaces;
using DiscordUWA.ViewModels;
using DiscordUWA.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using Windows.UI.Xaml.Controls;

namespace DiscordUWA.Services {
    public class LocatorService {

        public LocatorService() {
            NavigationService navService = new NavigationService(); 

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            navService.Configure("login", typeof(LoginPanel));
            navService.Configure("main", typeof(MainPage));
            navService.Configure("userProfile", typeof(UserProfile));
            navService.Configure("pinnedMessages", typeof(PinnedMessages));

            DiscordSocketClient discordClient = new DiscordSocketClient();
            SimpleIoc.Default.Register(() => discordClient);

            SimpleIoc.Default.Register(() => navService);
            SimpleIoc.Default.Register<SettingsService>();
            SimpleIoc.Default.Register<SecretService>();

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ServerViewModel>();
            SimpleIoc.Default.Register<UserProfileViewModel>();
            SimpleIoc.Default.Register<PinnedMessagesViewModel>();
        }

        public static LoginViewModel LoginView => ServiceLocator.Current.GetInstance<LoginViewModel>();
        public static ServerViewModel ServerView => ServiceLocator.Current.GetInstance<ServerViewModel>();
        public static UserProfileViewModel UserProfileView => ServiceLocator.Current.GetInstance<UserProfileViewModel>();
        public static PinnedMessagesViewModel PinnedMessagesView => ServiceLocator.Current.GetInstance<PinnedMessagesViewModel>();

        public static INavServiceExtend NavigationService  => ServiceLocator.Current.GetInstance<NavigationService>();
        public static ISettingsService SettingsService =>  ServiceLocator.Current.GetInstance<SettingsService>();
        public static ISecretService SecretService => ServiceLocator.Current.GetInstance<SecretService>();

        public static DiscordSocketClient DiscordSocketClient  => ServiceLocator.Current.GetInstance<DiscordSocketClient>();
    }
}
