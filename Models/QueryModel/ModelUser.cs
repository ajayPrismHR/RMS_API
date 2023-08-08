// Decompiled with JetBrains decompiler
// Type: SURAKSHA.Models.QueryModel.ModelUser
// Assembly: SURAKSHA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E464D3B-167A-40E6-AF30-5871BFBC1363
// Assembly location: C:\Users\soura\OneDrive\Desktop\SURAKSHA.dll

using System.Collections.Generic;


#nullable enable
namespace SURAKSHA.Models.QueryModel
{
  public class ModelUser : UserRequestQueryModel
  {
    public Int64 EmpID { get; set; }
    public string Emp_Name { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public long Mobile_NO { get; set; }
    public string Email { get; set; }
    public string Photo { get; set; }
    public int RoleID { get; set; }
    public Int64 ManagerId { get; set; }
    public Int64 OfficeID { get; set; }
    public List<ModelRole> RolesCollection { get; set; }
    public string Action { get; set; }

    }

    public class ModelRole
    {
        public int ID { get; set; }
        public string ROLE_NAME { get; set; }
    }
}
