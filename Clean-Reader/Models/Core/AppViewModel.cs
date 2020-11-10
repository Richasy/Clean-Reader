﻿using System;
using Lib.Share.Enums;
using Yuenov.SDK;
using Richasy.Helper.UWP;
using System.Threading.Tasks;

namespace Clean_Reader.Models.Core
{
    public partial class AppViewModel
    {
        public AppViewModel()
        {
            _yuenovClient = new YuenovClient();
            _yuenovClient.SetOpenToken("e89309f4-6cd8-4a45-90de-922e7d71455a");
        }
        
        public async Task OneDriveInit()
        {
            string token = App.Tools.App.GetLocalSetting(SettingNames.OneDriveAccessToken, "");
            if (string.IsNullOrEmpty(token))
                _onedrive = new OneDriveHelper(_clientId, _scopes);
            else
            {
                _onedrive = new OneDriveHelper(_clientId, _scopes, token);
                int now = App.Tools.App.GetNowSeconds();
                int expiry = Convert.ToInt32(App.Tools.App.GetLocalSetting(SettingNames.OneDriveExpiryTime, "0"));
                if (now >= expiry)
                {
                    var result = await _onedrive.RefreshTokenAsync();
                    if (result != null)
                    {
                        App.Tools.App.WriteLocalSetting(SettingNames.OneDriveAccessToken, result.AccessToken);
                        App.Tools.App.WriteLocalSetting(SettingNames.OneDriveExpiryTime, App.Tools.App.DateToTimeStamp(result.ExpiresOn.DateTime).ToString());
                    }
                }
            }
        }
    }
}
