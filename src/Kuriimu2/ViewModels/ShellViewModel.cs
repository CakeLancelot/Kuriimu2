﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Kontract.Interfaces.Common;
using Kontract.Interfaces.Font;
using Kontract.Interfaces.Image;
using Kontract.Interfaces.Text;
using Kore;
using Kore.Utilities;
using Kuriimu2.Interfaces;
using Microsoft.Win32;

namespace Kuriimu2.ViewModels
{
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        #region Private

        private IWindowManager _wm = new WindowManager();
        private List<IScreen> _windows = new List<IScreen>();
        private KoreManager _kore = new KoreManager();

        #endregion

        public ShellViewModel()
        {
            DisplayName = "Kuriimu2";

            // Load passed-in file
            // TODO: Somehow handle multiple files via delayed asynchronous loading
            if (AppBootstrapper.Args.Length > 0 && File.Exists(AppBootstrapper.Args[0]))
                LoadFile(AppBootstrapper.Args[0]);
        }

        public void ExitMenu()
        {
            if ((ActiveItem as IFileEditor)?.KoreFile.HasChanges ?? false)
                ; //ConfirmLossOfChanges();
            Application.Current.Shutdown();
        }

        public async void OpenButton()
        {
            var ofd = new OpenFileDialog { Filter = _kore.FileFilters, Multiselect = true };
            if (ofd.ShowDialog() != true) return;

            foreach (var file in ofd.FileNames)
                await LoadFile(file);
        }

        //public async void OpenTypeButton()
        //{
        //    var pe = new OpenTypeViewModel(_kore)
        //    {
        //        Title = "Open File by Type",
        //        Message = ""
        //    };
        //    _windows.Add(pe);

        //    if (_wm.ShowDialog(pe) == true)
        //    {
        //        await LoadFile(pe.SelectedFilePath, pe.SelectedFormatType);
        //    }
        //}

        public async void FileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;

            foreach (var file in files)
                await LoadFile(file);
        }

        public bool SaveButtonsEnabled => (ActiveItem as IFileEditor)?.KoreFile.Adapter is ISaveFiles;

        public void SaveButton()
        {
            SaveFile();
        }

        public void SaveAsButton()
        {
            var filter = "Any File (*.*)|*.*";

            if (ActiveItem is IFileEditor editor)
            {
                filter = editor.KoreFile.Filter;

                var sfd = new SaveFileDialog { FileName = editor.KoreFile.StreamFileInfo.FileName, Filter = filter };
                if (sfd.ShowDialog() != true) return;

                SaveFile(sfd.FileName);
            }
        }

        public void DebugButton()
        {
            //_kore.Debug();
        }

        #region ToolBar Visibility

        // Text
        public Visibility TextEditorToolsVisible => ActiveItem is ITextEditor ? Visibility.Visible : Visibility.Hidden;
        public Visibility TextEditorCanExportFiles => ActiveItem is ITextEditor text ? (text.TextEditorCanExportFiles ? Visibility.Visible : Visibility.Hidden) : Visibility.Hidden;
        public Visibility TextEditorCanImportFiles => ActiveItem is ITextEditor text ? (text.TextEditorCanImportFiles ? Visibility.Visible : Visibility.Hidden) : Visibility.Hidden;

        #endregion

        public void TextEditorExportFile()
        {
            var editor = (IFileEditor)ActiveItem;
            if (!(editor.KoreFile.Adapter is ITextAdapter adapter)) return;

            var creators = _kore.GetAdapters<ITextAdapter>().Where(a => a is ICreateFiles && a is IAddEntries);

            var sfd = new SaveFileDialog
            {
                FileName = Path.GetFileName(editor.KoreFile.StreamFileInfo.FileName) + Common.GetAdapterExtension(creators.First()),
                InitialDirectory = Path.GetDirectoryName(editor.KoreFile.StreamFileInfo.FileName),
                Filter = Common.GetAdapterFilters(creators)
            };
            if (sfd.ShowDialog() != true) return;

            Text.ExportFile(adapter, creators.Skip(sfd.FilterIndex - 1).First(), sfd.FileName);
        }

        public void TextEditorImportFile()
        {
            var editor = (IFileEditor)ActiveItem;
            if (!(editor.KoreFile.Adapter is ITextAdapter adapter)) return;

            var ofd = new OpenFileDialog { Filter = _kore.FileFiltersByType<ITextAdapter>("All Supported Text Files") };
            if (ofd.ShowDialog() != true) return;

            editor.KoreFile.HasChanges = _kore.ImportFile(adapter, ofd.FileName);
        }

        // Tabs
        public void TabChanged(SelectionChangedEventArgs args)
        {
            // General
            NotifyOfPropertyChange(() => SaveButtonsEnabled);

            // Text Editor
            NotifyOfPropertyChange(() => TextEditorToolsVisible);
            NotifyOfPropertyChange(() => TextEditorCanExportFiles);
            NotifyOfPropertyChange(() => TextEditorCanImportFiles);
        }

        public void CloseTab(IScreen tab)
        {
            tab.TryClose();
            switch (tab)
            {
                case IFileEditor editor:
                    _kore.CloseFile(editor.KoreFile);
                    break;
            }
        }

        public void CloseAllTabs()
        {
            for (var i = Items.Count - 1; i >= 0; i--)
                CloseTab(Items[i]);
        }

        #region Private Methods

        private async Task LoadFile(string filename)
        {
            KoreFileInfo kfi = null;

            try
            {
                await Task.Run(() => { kfi = _kore.LoadFile(new KoreLoadInfo(File.OpenRead(filename), filename)); });
            }
            catch (LoadFileException ex)
            {
                MessageBox.Show(ex.ToString(), "Open File", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ActivateTab(kfi);
        }

        private void ActivateTab(KoreFileInfo kfi)
        {
            if (kfi == null) return;

            switch (kfi.Adapter)
            {
                case ITextAdapter txt2:
                    ActivateItem(new TextEditor2ViewModel(_kore, kfi));
                    break;
                case IImageAdapter img:
                    ActivateItem(new ImageEditorViewModel(_kore, kfi));
                    break;
                case IFontAdapter fnt:
                    ActivateItem(new FontEditorViewModel(kfi));
                    break;
            }
        }

        /// <summary>
        /// The global save method that handles the various editor types.
        /// </summary>
        /// <param name="filename">The target file name to save as.</param>
        private void SaveFile(string filename = "")
        {
            var currentTab = ActiveItem as IFileEditor;
            try
            {
                if (currentTab == null)
                    return;

                if (!currentTab.KoreFile.HasChanges && filename == string.Empty)
                    return;

                var ksi = new KoreSaveInfo(currentTab.KoreFile, "temp") { NewSaveFile = filename };
                var savedKfi = _kore.SaveFile(ksi);

                if (savedKfi.ParentKfi != null)
                    savedKfi.ParentKfi.HasChanges = true;

                currentTab.KoreFile = savedKfi;

                // Handle archive editors.
                // TODO: Port the win forms code for this behaviour to WPF MVVM
                if (ActiveItem is IArchiveEditor archiveEditor)
                {
                    //archiveEditor.UpdateChildTabs(savedKfi);
                    //archiveEditor.UpdateParent();
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        #endregion

        public override void TryClose(bool? dialogResult = null)
        {
            for (var i = _windows.Count - 1; i >= 0; i--)
            {
                var scr = _windows[i];
                scr.TryClose(dialogResult);
                _windows.Remove(scr);
            }
            base.TryClose(dialogResult);
        }
    }
}
