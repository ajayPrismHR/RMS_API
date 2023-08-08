// Decompiled with JetBrains decompiler
// Type: SURAKSHA.Models.ModelSmsAPI
// Assembly: SURAKSHA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E464D3B-167A-40E6-AF30-5871BFBC1363
// Assembly location: C:\Users\soura\OneDrive\Desktop\SURAKSHA.dll


#nullable enable
namespace SURAKSHA.Models
{
  public class ModelSmsAPI
  {
    private string _smsApiURL = AppSettingsHelper.Setting("SMSAPI:smsApiURL");
    private string _appid = AppSettingsHelper.Setting("SMSAPI:appid");
    private string _userid = AppSettingsHelper.Setting("SMSAPI:userid");
    private string _pass = AppSettingsHelper.Setting("SMSAPI:pass");
    private string _contenttype = AppSettingsHelper.Setting("SMSAPI:contenttype");
    private string _from = AppSettingsHelper.Setting("SMSAPI:from");
    private string _alert = AppSettingsHelper.Setting("SMSAPI:alert");
    private string _selfid = AppSettingsHelper.Setting("SMSAPI:selfid");
    private string _to;
    private string _smstext;

    public string SmsApiURL => this._smsApiURL;

    public string Appid => this._appid;

    public string UserId => this._userid;

    public string Pass => this._pass;

    public string Contenttype => this._contenttype;

    public string From => this._from;

    public string Alert => this._alert;

    public string Selfid => this._selfid;

    public string To
    {
      get => this._to;
      set => this._to = value;
    }

    public string Smstext
    {
      get => this._smstext;
      set => this._smstext = value;
    }
  }
}
