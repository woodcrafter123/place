﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Modules.AnchorDataModule
// Assembly: TerrariaServer, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: 5CBA2320-074B-43F7-8CDC-BF1E2B81EE4B
// Assembly location: C:\Users\kevzhao\Downloads\TerrariaServer.exe

using Terraria.DataStructures;

namespace Terraria.Modules
{
  public class AnchorDataModule
  {
    public AnchorData top;
    public AnchorData bottom;
    public AnchorData left;
    public AnchorData right;
    public bool wall;

    public AnchorDataModule(AnchorDataModule copyFrom = null)
    {
      if (copyFrom == null)
      {
        this.top = new AnchorData();
        this.bottom = new AnchorData();
        this.left = new AnchorData();
        this.right = new AnchorData();
        this.wall = false;
      }
      else
      {
        this.top = copyFrom.top;
        this.bottom = copyFrom.bottom;
        this.left = copyFrom.left;
        this.right = copyFrom.right;
        this.wall = copyFrom.wall;
      }
    }
  }
}
