// Decompiled with JetBrains decompiler
// Type: SURAKSHA.Models.ViewModel.WeatherForecast
// Assembly: SURAKSHA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E464D3B-167A-40E6-AF30-5871BFBC1363
// Assembly location: C:\Users\soura\OneDrive\Desktop\SURAKSHA.dll

using System;


#nullable enable
namespace SURAKSHA.Models.ViewModel
{
  public class WeatherForecast
  {
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int) ((double) this.TemperatureC / 0.5556);

    public string? Summary { get; set; }
  }
}
