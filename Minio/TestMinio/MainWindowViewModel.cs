using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Minio;
using MinioTest.entities;
using MinioTest.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioTest
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private FileItem _file;
        [ObservableProperty]
        private ObservableCollection<FileItem> _files;
        [ObservableProperty]
        private string _uploadFilePath;

        [RelayCommand]
        private async Task Upload()
        {
            IMinioClient minioClient = MinioUtil.GetMinioClient();
            await MinioUtil.UploadFile(minioClient, "test-szc", UploadFilePath);
            await RefreshFileListView();
        }

        [RelayCommand]
        private void SelectUploadFile()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择上传文件"
            };
            if (dialog.ShowDialog() == true)
            {
                UploadFilePath = dialog.FileName;
            }
        }

        [RelayCommand]
        private async Task RefreshFileListView()
        {
            IMinioClient minioClient = MinioUtil.GetMinioClient();
            List<FileItem> fileItems = await MinioUtil.GetFileItems(minioClient, "test-szc");
            Files = new ObservableCollection<FileItem>(fileItems);
        }

        [RelayCommand]
        private async Task Download()
        {
            if(File == null)
            {
                return;
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "选择保存路径"
            };
            if(dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            IMinioClient minioClient = MinioUtil.GetMinioClient();
            await MinioUtil.DownloadFile(minioClient, "test-szc", File.FileName, dialog.SelectedPath);
        }

        [RelayCommand]
        private async Task Remove()
        {
            if (File == null)
            {
                return;
            }
            IMinioClient minioClient = MinioUtil.GetMinioClient();
            await MinioUtil.RemoveFile(minioClient, "test-szc", File.FileName);
            await RefreshFileListView();
        }

        public MainWindowViewModel()
        {
            RefreshFileListView();
        }
    }
}
