// Decompiled with JetBrains decompiler
// Type: SURAKSHA.Models.ViewModel.UserViewModel
// Assembly: SURAKSHA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E464D3B-167A-40E6-AF30-5871BFBC1363
// Assembly location: C:\Users\soura\OneDrive\Desktop\SURAKSHA.dll

using System;


#nullable enable
namespace SURAKSHA.Models.ViewModel
{
  public class UserViewModel
  {
    public long EmpID { get; set; }

    public string Emp_Name { get; set; }

    public string NAME { get; set; }

    public long ROLE_ID { get; set; }

    public string ROLE_NAME { get; set; }

    public bool Remember_Me { get; set; }

    public long ID { get; set; }

    public long OFFICE_ID { get; set; }

    public string AccessToken { get; set; }
  }
}
