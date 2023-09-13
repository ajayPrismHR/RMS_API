// Decompiled with JetBrains decompiler
// Type: SURAKSHA.Models.QueryModel.EmpSignUP
// Assembly: SURAKSHA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E464D3B-167A-40E6-AF30-5871BFBC1363
// Assembly location: C:\Users\soura\OneDrive\Desktop\SURAKSHA.dll


#nullable enable
using Microsoft.AspNetCore.Mvc;
using RMS_API.Models.QueryModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SURAKSHA.Models.QueryModel
{
  public class MasterUserRegistration
  {
        [NotNull]
        public string Userinfo { get; set; } = string.Empty;

        //[NotNull]
        //public ModelFile file { get; set; }

        [NotNull]
        public IFormFile ImageFile { get; set; }

    }
public class UserRegistration
    {
        public string registrationID { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string MailID { get; set; }
        public string Address { get; set; }
        public string Area_Code { get; set; }
        public string Image { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }

        public string Gender { get; set; }
        public DateTime DOB { get; set; }
    }

}
