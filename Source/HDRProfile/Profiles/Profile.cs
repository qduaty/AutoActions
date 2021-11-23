﻿using AutoHDR.Profiles.Actions;
using CodectoryCore.UI.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoHDR.Profiles
{
    public class Profile : BaseViewModel, IEquatable<Profile>
    {

        public enum ProfileActionListType
        {
            Started,
            Closed,
            GotFocus,
            LostFocus
        }

        [XmlIgnore]
        public RelayCommand AddStartedActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand AddClosedActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand AddGotFocusActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand AddLostFocusActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand<ProfileActionBase> RemoveProfileActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand<ProfileActionBase> RemoveStartedActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand<ProfileActionBase> RemoveClosedActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand<ProfileActionBase> RemoveGotFocusActionCommand { get; private set; }
        [XmlIgnore]
        public RelayCommand<ProfileActionBase> RemoveLostFocusActionCommand { get; private set; }


        public Profile()
        {
            AddStartedActionCommand = new RelayCommand(() => AddProfileAction(ProfileActionListType.Started));
            AddClosedActionCommand = new RelayCommand(() => AddProfileAction(ProfileActionListType.Closed));
            AddGotFocusActionCommand = new RelayCommand(() => AddProfileAction(ProfileActionListType.GotFocus));
            AddLostFocusActionCommand = new RelayCommand(() => AddProfileAction(ProfileActionListType.LostFocus));
            RemoveProfileActionCommand = new RelayCommand<ProfileActionBase>((pa) => RemoveProfileAction(pa));

            RemoveStartedActionCommand = new RelayCommand<ProfileActionBase>((pa) => RemoveProfileAction(ProfileActionListType.Started, pa));
            RemoveClosedActionCommand = new RelayCommand<ProfileActionBase>((pa) => RemoveProfileAction(ProfileActionListType.Closed, pa));
            RemoveGotFocusActionCommand = new RelayCommand<ProfileActionBase>((pa) => RemoveProfileAction(ProfileActionListType.GotFocus, pa));
            RemoveLostFocusActionCommand = new RelayCommand<ProfileActionBase>((pa) => RemoveProfileAction(ProfileActionListType.LostFocus, pa));
            PropertyChanged += Profile_PropertyChanged;
        }

        private void Profile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            Globals.Instance.SaveSettings();
        }

        private string _name = "-";
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private ListOfProfileActions _applicationStarted = new ListOfProfileActions();
        public ListOfProfileActions ApplicationStarted
        {
            get { return _applicationStarted; }
            set { _applicationStarted = value; OnPropertyChanged(); }
        }

        private ListOfProfileActions _applicationClosed = new ListOfProfileActions();
        public ListOfProfileActions ApplicationClosed
        {
            get { return _applicationClosed; }
            set { _applicationClosed = value; OnPropertyChanged(); }
        }

        private ListOfProfileActions _applicationGotFocus = new ListOfProfileActions();

        public ListOfProfileActions ApplicationGotFocus
        {
            get { return _applicationGotFocus; }
            set { _applicationGotFocus = value; OnPropertyChanged(); }
        }

        private ListOfProfileActions _applicationLostFocus = new ListOfProfileActions();

        public ListOfProfileActions ApplicationLostFocus
        {
            get { return _applicationLostFocus; }
            set { _applicationLostFocus = value; OnPropertyChanged(); }
        }

        public void AddProfileAction(ProfileActionListType listType)
        {

            ProfileActionAdder adder = new ProfileActionAdder();
            adder.DialogService = DialogService;
            adder.OKClicked += (o, e) =>
            {
                switch (listType)
                {
                    case ProfileActionListType.Started:
                        ApplicationStarted.Add(adder.ProfileAction);
                        break;
                    case ProfileActionListType.Closed:
                        ApplicationClosed.Add(adder.ProfileAction);
                        break;
                    case ProfileActionListType.GotFocus:
                        ApplicationGotFocus.Add(adder.ProfileAction);
                        break;
                    case ProfileActionListType.LostFocus:
                        ApplicationLostFocus.Add(adder.ProfileAction);
                        break;

                }
            };
            if (DialogService != null)
                DialogService.ShowDialogModal(adder, new System.Drawing.Size(640, 450));
        }

        public void RemoveProfileAction(ProfileActionBase profileAction)
        {
            if (ApplicationStarted.Contains(profileAction))
                ApplicationStarted.Remove(profileAction);
            if (ApplicationClosed.Contains(profileAction))
                ApplicationClosed.Remove(profileAction);
            if (ApplicationGotFocus.Contains(profileAction))
                ApplicationGotFocus.Remove(profileAction);
            if (ApplicationLostFocus.Contains(profileAction))
                ApplicationLostFocus.Remove(profileAction);
        }


        public void RemoveProfileAction(ProfileActionListType listType, ProfileActionBase profileAction)
        {
            switch (listType)
            {
                case ProfileActionListType.Started:
                    ApplicationStarted.Remove(profileAction);
                    break;
                case ProfileActionListType.Closed:
                    ApplicationClosed.Remove(profileAction);
                    break;
                case ProfileActionListType.GotFocus:
                    ApplicationGotFocus.Remove(profileAction);
                    break;
                case ProfileActionListType.LostFocus:
                    ApplicationLostFocus.Remove(profileAction);
                    break;

            }

        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Profile);
        }

        public bool Equals(Profile other)
        {
            return other != null &&
                   Name == other.Name &&
                   EqualityComparer<ListOfProfileActions>.Default.Equals(ApplicationStarted, other.ApplicationStarted) &&
                   EqualityComparer<ListOfProfileActions>.Default.Equals(ApplicationClosed, other.ApplicationClosed) &&
                   EqualityComparer<ListOfProfileActions>.Default.Equals(ApplicationGotFocus, other.ApplicationGotFocus) &&
                   EqualityComparer<ListOfProfileActions>.Default.Equals(ApplicationLostFocus, other.ApplicationLostFocus);
        }

        public override int GetHashCode()
        {
            int hashCode = 210938521;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<ListOfProfileActions>.Default.GetHashCode(ApplicationStarted);
            hashCode = hashCode * -1521134295 + EqualityComparer<ListOfProfileActions>.Default.GetHashCode(ApplicationClosed);
            hashCode = hashCode * -1521134295 + EqualityComparer<ListOfProfileActions>.Default.GetHashCode(ApplicationGotFocus);
            hashCode = hashCode * -1521134295 + EqualityComparer<ListOfProfileActions>.Default.GetHashCode(ApplicationLostFocus);
            return hashCode;
        }
    }
}
