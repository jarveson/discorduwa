﻿using Discord;
using Discord.Rest;
using Discord.WebSocket;
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

            DiscordSocketClient discordClient = new DiscordSocketClient();
            //DiscordRestClient discordRestClient = new DiscordRestClient();

            navService.Configure("login", typeof(LoginPanel));
            navService.Configure("main", typeof(MainPage));

            SimpleIoc.Default.Register(() => navService);
            SimpleIoc.Default.Register(() => discordClient);
            //SimpleIoc.Default.Register(() => discordRestClient);
            SimpleIoc.Default.Register<SettingsService>();

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ServerViewModel>();
        }

        public static LoginViewModel LoginView => ServiceLocator.Current.GetInstance<LoginViewModel>();
        public static ServerViewModel ServerView => ServiceLocator.Current.GetInstance<ServerViewModel>();

        public static INavServiceExtend NavigationService  => ServiceLocator.Current.GetInstance<NavigationService>();
        public static ISettingsService SettingsService =>  ServiceLocator.Current.GetInstance<SettingsService>();

        public static DiscordSocketClient DiscordSocketClient  => ServiceLocator.Current.GetInstance<DiscordSocketClient>();
        //public static DiscordRestClient DiscordRestClient => ServiceLocator.Current.GetInstance<DiscordRestClient>();
    }
}